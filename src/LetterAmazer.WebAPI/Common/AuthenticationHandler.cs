using LetterAmazer.Business.Services.Domain.Organisation;
using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using LetterAmazer.Business.Services.Services;
using LetterAmazer.Business.Services.Services.Caching;
using LetterAmazer.Data.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace LetterAmazer.WebAPI.Common
{
    /// <summary>
    /// A Class that looks at all HTTP requests for tokens. This class need to remove.
    /// </summary>
    public class AuthenticationHandler : DelegatingHandler
    {
        private const string AuthenticationInValidMessage = "Your Token Could Not Be Authenticated";
        private const string AuthenticationValidMessage = "Your Token Has Been Authenticated";
        private const int InValidStringChecker = 10;

        //REFACTOR: MH I don't actually think these need to be public for testing purposes 
        public string AccessTokenHeaderName = "X-ApiAccessKey";
        public string AudienceKey;
        public string PrivateKey;
        public string ScreatKeyHeaderName = "X-ApiAccessSecret";
        public string Token = "Invalid Token";
        private IOrganisationService organisationService;

        public AuthenticationHandler()
        {
        }
        public AuthenticationHandler(IOrganisationService organisationService)
        {
            organisationService = organisationService;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            if (HeaderContainsToken(request) || QueryStringContainsToken(request))
            {

                Token = string.Format("{0}", Token);

                
                var inValidResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent(AuthenticationInValidMessage)
                };
                var tscInValid = new TaskCompletionSource<HttpResponseMessage>();
                tscInValid.SetResult(inValidResponse);

                return tscInValid.Task;
            }

            return base.SendAsync(request, cancellationToken);
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

            IEnumerable<string> apiTokenHeaderValues;
            message.Headers.TryGetValues(AccessTokenHeaderName, out apiTokenHeaderValues);
            message.Headers.TryGetValues(ScreatKeyHeaderName, out apiTokenHeaderValues);

            // TODO : we need to find some way to make this initiallization on correct way
            LetterAmazerEntities entity = new LetterAmazerEntities();
            IOrganisationFactory factory = new OrganisationFactory(entity);
            IOrganisationService organisationService = new OrganisationService(entity, factory, new HttpCacheService());
            var apiAccess = organisationService.GetApiKeys(apiTokenHeaderValues.First(),apiTokenHeaderValues.Last());

            if (apiAccess != null)
            {
                System.Web.HttpContext.Current.Cache["OrganizationId"] = apiAccess.OrganisationId;
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}