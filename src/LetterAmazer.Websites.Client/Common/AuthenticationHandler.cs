using LetterAmazer.Business.Services.Factory;
using LetterAmazer.Business.Services.Factory.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using LetterAmazer.Data.Repository.Data;

namespace LetterAmazer.Websites.Client.Common
{
    /// <summary>
    /// A Class that looks at all HTTP requests for tokens.
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
            LetterAmazerEntities entity = new LetterAmazerEntities();
            IOrganisationFactory organisationFactory = new OrganisationFactory(entity);

            var dbapiAccess = organisationFactory.GetApiKeys(new Business.Services.Domain.Api.ApiKeys() { ApiKey = apiTokenHeaderValues.First(), ApiSecret = apiTokenHeaderValues.Last() });

            if(dbapiAccess != null)
            {
                   
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}