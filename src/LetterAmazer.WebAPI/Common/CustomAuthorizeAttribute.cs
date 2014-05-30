using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Data.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using LetterAmazer.Business.Services.Domain.Api;

namespace LetterAmazer.WebAPI.Common
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        
        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        private const string AuthenticationInValidMessage = "Your Token Could Not Be Authenticated";
        private const string AuthenticationValidMessage = "Your Token Has Been Authenticated";
        private const int InValidStringChecker = 1;

        //REFACTOR: MH I don't actually think these need to be public for testing purposes 
        public string AccessTokenHeaderName = "X-ApiAccessKey";
        public string AudienceKey;
        public string PrivateKey;
        public string ScreatKeyHeaderName = "X-ApiAccessSecret";
        public string Token = "Invalid Token";
        public string currentRole = "NONE";


        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (HeaderContainsToken(actionContext.Request) || QueryStringContainsToken(actionContext.Request))
                {

                    if (IsValidHeader(actionContext.Request)==false)
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                        return;
                    }
                    else
                    {
                        if(this.Roles.ToUpper().Contains(this.currentRole) == false)
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.NonAuthoritativeInformation);
                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                    return;
                }
            }
            catch (Exception)
            {
              //  actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }
        private bool QueryStringContainsToken(HttpRequestMessage message)
        {
            var query = message.RequestUri.ParseQueryString();
            var requestedAccessToken = query[AccessTokenHeaderName];
            var requestedRelayToken = query[ScreatKeyHeaderName];

            if (requestedAccessToken != null)
            {
                Token = requestedAccessToken;

                return true;
            }

            if (requestedRelayToken != null)
            {
                Token = requestedRelayToken;

                return true;
            }

            return false;
        }

        private bool HeaderContainsToken(HttpRequestMessage message)
        {
            IEnumerable<string> apiTokenHeaderValues;

            if (message.Headers.TryGetValues(AccessTokenHeaderName, out apiTokenHeaderValues))
            {
                var apiTokenHeaderValue = apiTokenHeaderValues.First();

                if (apiTokenHeaderValue.Length > InValidStringChecker)
                {
                    Token = string.Format("{0}", apiTokenHeaderValue);

                    return true;
                }
            }

            if (message.Headers.TryGetValues(ScreatKeyHeaderName, out apiTokenHeaderValues))
            {
                var apiTokenHeaderValue = apiTokenHeaderValues.First();

                if (apiTokenHeaderValue.Length > InValidStringChecker)
                {
                    Token = string.Format("{0}", apiTokenHeaderValue);

                    return true;
                }
            }

            return false;
        }

        private bool IsValidHeader(HttpRequestMessage message)
        {

            IEnumerable<string> apiTokenHeaderKeyValues;
            message.Headers.TryGetValues(AccessTokenHeaderName, out apiTokenHeaderKeyValues);

            IEnumerable<string> apiTokenHeaderSecreatValues;
            message.Headers.TryGetValues(ScreatKeyHeaderName, out apiTokenHeaderSecreatValues);
            // TODO : we need to find some way to make this initiallization on correct way
            LetterAmazerEntities entity = new LetterAmazerEntities();
            IOrganisationFactory factory = new OrganisationFactory(entity);
            IOrganisationService organisationService = new OrganisationService(entity, factory);
            var apiAccess = organisationService.GetApiKeys(apiTokenHeaderKeyValues.First(), apiTokenHeaderSecreatValues.First());

            if (apiAccess.Role != null)
            {
                if (apiAccess.Role != Role.NONE)
                {
                    currentRole = apiAccess.Role.ToString();
                    System.Web.HttpContext.Current.Cache["OrganizationId"] = apiAccess.OrganisationId;
                }
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}