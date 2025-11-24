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
        Console.Write("Username: ");
        userName = Console.ReadLine();

        Console.Write("Password: ");
        passWord = Console.ReadLine();
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
