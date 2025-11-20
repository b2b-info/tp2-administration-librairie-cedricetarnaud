namespace BookStore;

public class Login
{
    private static string userName, passWord;

    private static readonly List<UserCredential> _users = new()
    {
        new UserCredential("admin", "admin"),
        new UserCredential("user", "1234")
    };

    public static void GetLoginInfo()
    {
        Console.Write("Username: ");
        userName = Console.ReadLine();

        Console.Write("Password: ");
        passWord = Console.ReadLine();
    }

    public static bool IsLoggedIn()
    {
        return _users.Any(user =>
            string.Equals(
                user.UserName, userName, StringComparison.OrdinalIgnoreCase)
                && user.Password == passWord
            );
    }
}

public class UserCredential
{
    public string UserName { get; }
    public string Password { get; }

    public UserCredential(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}