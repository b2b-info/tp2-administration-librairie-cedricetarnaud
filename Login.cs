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
        Program.logger.LogInformation("User need to enter his Username and Password to login.");


        userName = ToolBox.ReadNonEmpty("Username : ");

        passWord = ToolBox.ReadNonEmpty("Password : ");

        Program.logger.LogDebug($"Username: {userName}, Password: {passWord}.");
        Program.logger.LogInformation("Username and Password entered.");
    }

    public static bool IsLoggedIn()
    {
        Program.logger.LogDebug("Checking login attempt.");
        lock (_lockLogin)
        {
            bool success = _users.Any(user =>
                string.Equals(user.UserName, userName, StringComparison.OrdinalIgnoreCase)
                && user.Password == passWord
            );

            if (success)
            {
                Program.logger.LogInformation($"Login successful for user: {userName}");
            }
            else
            {
                Program.logger.LogWarning($"Login failed for user: {userName}");
            }

            return success;
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
}
