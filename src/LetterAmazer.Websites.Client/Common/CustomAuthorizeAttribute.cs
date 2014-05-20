using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
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

namespace LetterAmazer.Websites.Client.Common
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
                        if(this.Roles.Contains(this.currentRole) == false)
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                            return;
                        }
                        else
                        {
                            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                            actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
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
            LetterAmazerEntities entity = new LetterAmazerEntities();
            IOrganisationFactory organisationFactory = new OrganisationFactory(entity);

            var dbapiAccess = organisationFactory.GetApiKeys(new Business.Services.Domain.Api.ApiKeys() { ApiKey = apiTokenHeaderKeyValues.First(), ApiSecret = apiTokenHeaderSecreatValues.First() });
            //var dbapiAccess = organisationFactory.GetApiKeys(new Business.Services.Domain.Api.ApiKeys() { ApiKey = "abc", ApiSecret = "123456" });    
            if (dbapiAccess != null)
            {
                if (string.IsNullOrEmpty(dbapiAccess.Role)==false)
                {
                    currentRole = dbapiAccess.Role;
                    System.Web.HttpContext.Current.Session["OrganizationId"] = dbapiAccess.OrganisationId;
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