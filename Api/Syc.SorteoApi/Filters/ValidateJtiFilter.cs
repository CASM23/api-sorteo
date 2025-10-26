using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SyC.Sorteo.Infrastructure.Persistence;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace SyC.Sorteo.Api.Filters
{
    public class ValidateJtiFilter : IAsyncActionFilter
    {
        private readonly SorteoDbContext _dbContext;

        public ValidateJtiFilter(SorteoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
         
            if (!context.HttpContext.User.Identity?.IsAuthenticated ?? true)
            {
                await next();
                return;
            }

            var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentJti = context.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(currentJti))
            {
            
                context.Result = new UnauthorizedObjectResult(new { 
                    status = 401,
                    message = "Información de usuario incompleta en el token." 
                });
                return;
            }

            int userId = int.Parse(userIdClaim);

            
            var lastToken = await _dbContext.LastTokenIds
                .AsNoTracking() 
                .FirstOrDefaultAsync(t => t.UsuarioId == userId);

            if (lastToken == null || lastToken.Jti != currentJti)
            {
            
                context.Result = new UnauthorizedObjectResult(new { 
                    status = 401,
                    message = "Su sesión ha sido invalidada. Por favor, inicie sesión nuevamente." 
                });
                return;
            }

            await next(); 
        }
    }
}
