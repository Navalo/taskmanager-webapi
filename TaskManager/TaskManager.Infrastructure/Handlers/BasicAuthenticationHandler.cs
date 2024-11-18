using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TaskManager.Infrastructure.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Infrastructure.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ApplicationDbContext _dbContext;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            Microsoft.AspNetCore.Authentication.ISystemClock clock,
            ApplicationDbContext dbContext) : base(options, logger, encoder, clock)
        {
            _dbContext = dbContext;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("No Authorization header found");

            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return AuthenticateResult.Fail("Invalid Authorization header format");

            var headerValue = authorizationHeader.Substring("Basic ".Length).Trim();

            try
            {
                var bytes = Convert.FromBase64String(headerValue);
                string credentials = Encoding.UTF8.GetString(bytes);

                if (string.IsNullOrEmpty(credentials))
                    return AuthenticateResult.Fail("Empty credentials provided");

                string[] parts = credentials.Split(":", 2);
                if (parts.Length != 2)
                    return AuthenticateResult.Fail("Invalid credentials format");

                string username = parts[0];
                string password = parts[1];

                byte[] byteArray = Encoding.UTF8.GetBytes(password);

                var encodedPassword = Convert.ToBase64String(byteArray);

                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (user == null || !VerifyPassword(encodedPassword, user.Password))  // Replace VerifyPassword with actual hash checking logic
                    return AuthenticateResult.Fail("Unauthorized");

                var claims = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (FormatException)
            {
                return AuthenticateResult.Fail("Invalid Base64 encoding");
            }
        }

        // Dummy method to simulate password verification. Replace with actual hashing logic.
        private bool VerifyPassword(string inputPassword, string storedPasswordHash)
        {
            // Implement a password hash comparison here
            return inputPassword == storedPasswordHash;  // Simplified for example
        }
    }
}
