namespace BadgerClan.Web.Services;

public class CurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext;
        if (context is null)
            throw new InvalidOperationException("HttpContext is null");

        if (context.Request.Cookies.TryGetValue("BadgerClan.CurrentUser", out var userId))
        {
            CurrentUserId = Guid.Parse(userId);
        }
        else
        {
            CurrentUserId = Guid.NewGuid();
            context.Response.Cookies.Append("BadgerClan.CurrentUser", CurrentUserId.ToString());
        }
    }
    public Guid CurrentUserId { get; } = Guid.NewGuid();
}
