using System.Security.Claims;
using GLMS.Web.Data;
using GLMS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Web.Services;

public interface IAuthService
{
    bool Validate(string username, string password, out AppUser? user);
    ClaimsPrincipal CreatePrincipal(AppUser user);
}

public class AuthService : IAuthService
{
    private readonly GlmsDbContext _db;

    public AuthService(GlmsDbContext db)
    {
        _db = db;
    }

    public bool Validate(string username, string password, out AppUser? user)
    {
        // Removes accidental spaces from the start or end of the username.
        var normalizedUsername = username.Trim().ToUpper();

        // EF Core can translate ToUpper() into SQL, so this query works in the database.
        user = _db.Users.AsNoTracking().FirstOrDefault(x =>
            x.Username.ToUpper() == normalizedUsername &&
            x.Password == password);

        return user is not null;
    }

    public ClaimsPrincipal CreatePrincipal(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "Cookies");
        return new ClaimsPrincipal(identity);
    }
}