using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Bonos.Api.Rest.Helper
{
    /// <summary>
    /// Token validator for Authorization Request using a DelegatingHandler
    /// </summary>
    public class TokenValidationHandler : DelegatingHandler
    {
        //Log
        com.mercaderia.bono.Utilidades.Auditoria.Logger log = com.mercaderia.bono.Utilidades.Auditoria.Logger.Instancia;
        public string BearerToken1 { get; private set; }

        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {

            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            if (isCorsRequest)
            {
                if (isPreflightRequest)
                {
                    return HttpResponseIsPreflightRequest(request, Origin, cancellationToken);
                    
                }
                else
                {
                    if (!request.RequestUri.ToString().ToLower().Contains("/api/public/"))
                    {
                        return VerificarRequest(request, cancellationToken);
                    }
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = t.Result;
                        resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                        return resp;
                    });
                }
            }
            else
            {
                return HttpResponseIsNotCorsRequest(request, cancellationToken);       

            }
        }

        /// <summary>
        /// Construye HttpResponseMessage si IsNotCorsRequest
        /// </summary>
        private Task<HttpResponseMessage> HttpResponseIsNotCorsRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.RequestUri.ToString().ToLower().Contains("/api/public/"))
            {
                return VerificarRequestLocal(request, cancellationToken);
            }
            else
            {
                return base.SendAsync(request, cancellationToken);
            }
        }

        /// <summary>
        /// Construye HttpResponseMessage si isPreflightRequest
        /// </summary>
        private Task<HttpResponseMessage> HttpResponseIsPreflightRequest(HttpRequestMessage request, string Origin, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew<HttpResponseMessage>(() =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                if (accessControlRequestMethod != null)
                {
                    response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                }

                string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
                if (!string.IsNullOrEmpty(requestedHeaders))
                {
                    response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                }

                return response;
            }, cancellationToken);
        }

        /// <summary>
        /// Verifica si el metodo es público y valida token
        /// </summary>
            private Task<HttpResponseMessage> VerificarRequestLocal(HttpRequestMessage request, CancellationToken cancellationToken)
            {

                try
                {
                    if (!ValidateToken(request.Headers))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);

                        return tsc.Task;

                    }
                }
                catch (Exception ex)
                {
                    log.EscribirLogError("Error durante la validación de token", ex);
                    throw ex;
                }

                return base.SendAsync(request, cancellationToken);

            }


            /// <summary>
            /// Verifica si el metodo es público y valida token
            /// </summary>
            private Task<HttpResponseMessage> VerificarRequest(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                try
                {
                    if (!ValidateToken(request.Headers))
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                        var tsc = new TaskCompletionSource<HttpResponseMessage>();
                        tsc.SetResult(response);

                        return tsc.Task.ContinueWith<HttpResponseMessage>(t =>
                        {
                            HttpResponseMessage resp = t.Result;
                            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                            return resp;
                        });

                    }
                }
                catch (Exception ex)
                {
                    log.EscribirLogError("Error durante la validación de token", ex);
                    throw ex;
                }

                return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                {
                    HttpResponseMessage resp = t.Result;
                    resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                    return resp;
                });

            }


            private bool ValidateToken(HttpRequestHeaders header)
            {
                try
                {

                    string token = string.Empty;
                    IEnumerable<string> authzHeaders;
                    if (!header.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
                    {
                        return false;
                    }

                    BearerToken1 = authzHeaders.ElementAt(0);
                    token = BearerToken1.StartsWith("Bearer ") ? BearerToken1.Substring(7) : BearerToken1;


                    var credentials = GoogleCredential.FromFile(ConfigurationManager.AppSettings["GOOGLE_APPLICATION_CREDENTIALS"]);
                    FirebaseApp defaultApp = null;
                    try
                    {
                        defaultApp = FirebaseApp.Create(new AppOptions()
                        {
                            Credential = credentials
                        });
                    }
                    catch (Exception)
                    {
                        defaultApp = FirebaseApp.DefaultInstance;
                    }

                    var defaultAuth = FirebaseAuth.GetAuth(defaultApp);
                    defaultAuth = FirebaseAuth.DefaultInstance;
                    FirebaseToken decodedToken = defaultAuth.VerifyIdTokenAsync(token).Result;

                    string uid = decodedToken.Uid;
                    return true;

                }
                catch (Exception ex)
                {
                    log.EscribirLogError("Error durante la validación de token", ex);
                    return false;
                }
            }



        }
    }