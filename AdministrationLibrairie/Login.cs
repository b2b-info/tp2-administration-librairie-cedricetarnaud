namespace BookStore;

using Microsoft.Extensions.Logging;

public class Login
{
    private static string userName,
        passWord;

    private static readonly List<UserCredential> _users = new()
    {
        new UserCredential("admin", "admin"),
        new UserCredential("user", "1234"),
    };

    private static readonly object _lockLogin = new();

    public static void GetLoginInfo()
    {
        Program.logger.LogInformation("User need to enter his Username and Password to login");
        
        Console.Write("Username: ");
        userName = Console.ReadLine();

        Console.Write("Password: ");
        passWord = Console.ReadLine();

        Program.logger.LogDebug("Username: " + userName + ", Password: " + passWord);
        Program.logger.LogInformation("Username and Password entered");
    }

    public static bool IsLoggedIn()
    {
        lock (_lockLogin)
        {
            return _users.Any(user =>
                string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase)
                && user.Password == passWord
            );
        }
    }
}
