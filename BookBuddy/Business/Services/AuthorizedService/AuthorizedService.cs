using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNetCore.Identity;

namespace BookBuddy.Business.Services.AuthorizedService;

public class AuthorizedService : IAuthorizedService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthorizedService> _logger;

    public AuthorizedService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<AuthorizedService> logger)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger;
    }

    public async Task<bool> IsUserAuthorizedAsync()
    {
        try
        {
            if (_httpContextAccessor.HttpContext?.User == null)
            {
                return false;
            }

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user == null)
            {
                return false;
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles == null || roles.Count == 0;
        }
        catch (Exception ex)
        {
            _logger.LogError($"ERROR : AuthorizedService.IsUserAuthorizedAsync() : {ex.Message}");
            return false;
        }
    
    }
}
