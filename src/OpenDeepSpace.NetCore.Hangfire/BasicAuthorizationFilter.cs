using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire
{
    /// <summary>
    /// 基础认证
    /// </summary>
    public class BasicAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string UserName;
        private readonly string Password;

        /// <summary>
        /// BasicAuthorizationFilter
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Password"></param>
        public BasicAuthorizationFilter(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
        /// <summary>
        /// Authorize
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var header = httpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(header))
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            var authValues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);

            if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            if (authValues.Parameter == null)
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
            var parts = parameter.Split(':');

            if (parts.Length < 2)
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
            {
                SetChallengeResponse(httpContext);
                return false;
            }

            if (parts[0] == UserName && parts[1] == Password)
            {
                return true;
            }

            SetChallengeResponse(httpContext);
            return false;
        }

        private void SetChallengeResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Authentication is required.");
        }
    }
}
