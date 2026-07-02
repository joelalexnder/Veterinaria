namespace VetClinic.Web.Services;

public class AuthStateService
{
    public string? Token { get; private set; }
    public string? FullName { get; private set; }
    public string? Role { get; private set; }
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);

    public event Action? OnChange;

    public void SetSession(string token, string fullName, string role)
    {
        Token = token;
        FullName = fullName;
        Role = role;
        OnChange?.Invoke();
    }

    public void ClearSession()
    {
        Token = null;
        FullName = null;
        Role = null;
        OnChange?.Invoke();
    }
}