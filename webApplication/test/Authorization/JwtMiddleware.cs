using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using webApplication.Authorization;
using Domain.Interfaces;
using BusinessLogic.Authorization;
using System.Security.Claims;

namespace webApplication.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // ✅ Правильная сигнатура: только HttpContext
        public async Task Invoke(HttpContext context)
        {
            _logger.LogDebug("JwtMiddleware invoked for path: {Path}", context.Request.Path);

            var authHeader = context.Request.Headers ["Authorization"].FirstOrDefault();
            _logger.LogDebug("Authorization header: {AuthHeader}", authHeader ?? "null");

            string token = null;

            if (!string.IsNullOrEmpty(authHeader))
            {
                var parts = authHeader.Split(' ', 2);
                if (parts.Length == 2 && parts [0].Equals("Bearer", StringComparison.OrdinalIgnoreCase))
                {
                    token = parts [1];
                }
            }

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var jwtUtils = context.RequestServices.GetRequiredService<IJwtUtils>();
                    var accountId = jwtUtils.ValidateJwtToken(token);

                    if (accountId.HasValue)
                    {
                        var wrapper = context.RequestServices.GetRequiredService<IRepositoryWrapper>();
                        var user = await wrapper.user.GetByIdWithToken(accountId.Value);

                        if (user != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.userid.ToString()),
                                new Claim("userid", user.userid.ToString())
                            };

                            // Добавляем роль, если она используется в [Authorize(Roles = "...")]
                            if (user.RoleT != null)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, user.RoleT.ToString()));
                            }

                            var identity = new ClaimsIdentity(claims, "jwt"); // "jwt" — схема аутентификации
                            context.User = new ClaimsPrincipal(identity);

                            // Опционально: сохраняем объект для удобства в контроллерах
                            context.Items ["user"] = user;

                            _logger.LogInformation("User {UserId} authenticated via JWT", user.userid);
                        }
                        else
                        {
                            _logger.LogWarning("User with ID {UserId} not found in database", accountId.Value);
                        }
                    }
                    else
                    {
                        _logger.LogWarning("JWT validation returned null user ID");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in JwtMiddleware");
                }
            }

            await _next(context);
        }
    }
}