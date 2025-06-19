using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UserContextService : IUserContextService {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor) {
        _httpContextAccessor = httpContextAccessor;
    }

    public string UserId => _httpContextAccessor.HttpContext?
                                                .User?
                                                .FindFirst(ClaimTypes.NameIdentifier)?
                                                .Value;

    public string UserEmail => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
}
