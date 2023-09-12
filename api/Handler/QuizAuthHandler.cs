using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using quiz.Data;
using System.Net.Http.Headers;
using System.Text;
using quiz.Model;
using System.Security.Claims;

namespace quiz.Handler
{
    public class QuizAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IQuizRepo _repository;

        public QuizAuthHandler(
            IQuizRepo repository,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _repository = repository;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return AuthenticateResult.Fail("Authorization header not found.");
            }
            else
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");
                var username = credentials[0];
                var password = credentials[1];

                if (_repository.ValidAdmin(username, password))
                {
                    var claims = new[] { new Claim("admin", username) };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                    //AuthenticationTicket ticket = new AuthenticationTicket(null, "");
                    //Console.WriteLine("auth scheme : {0}", Scheme.Name);
                    Console.WriteLine("b1");
                    return AuthenticateResult.Success(ticket);
                }
                else if (_repository.ValidLogin(username, password))
                {
                    var claims = new[] { new Claim("userName", username) };

                    ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return AuthenticateResult.Success(ticket);
                }
                Response.Headers.Add("WWW-Authenticate", "Basic");
                return AuthenticateResult.Fail("userName and password do not match or not belong to admin");
            }
        }
    }
}
