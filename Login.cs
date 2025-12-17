namespace BookStore;

using Microsoft.Extensions.Logging;

public class Login
{
    private static string _userNameLoginAttempt = "default",_passWordLoggingAttempt = "default";
    private static bool _isLoggedIn { get; set; }

    private static readonly List<UserCredential> _users = new()
    {
        new UserCredential("admin", "admin"),
        new UserCredential("user", "1234"),
    };
    public static void RunLoginLoop()
    {
        uint count = 0;
        Console.WriteLine("Please Login");

        while (count <= 2)
        {
            GetLoginInfo();

            if (_isLoggedIn)
            {
                Program.logger.LogInformation($"User successfully logged in.");
                Console.Clear();
                break;
            }
            else
            {

                if (count == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Too many failed attempts....");
                    Program.logger.LogError("Application exiting due to too many failed login attempts.");
                    Environment.Exit(0);
                }
                else
                {
                    count++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid credentials. Try again....");
                    Program.logger.LogWarning($"Failed login attempt: {count}");
                    Console.ResetColor();
                }
            }
        }
        Program.logger.LogInformation("Starting main menu loop.");
    }


    public static void GetLoginInfo()
    {
        Program.logger.LogInformation("Asking the user to enter login info");


        _userNameLoginAttempt = ToolBox.ReadNonEmpty("Username : ");

        _passWordLoggingAttempt = ToolBox.ReadNonEmpty("Password : ");

        Program.logger.LogDebug($"Username: {_userNameLoginAttempt}, Password: {_passWordLoggingAttempt}.");
        Program.logger.LogInformation("Username and Password entered.");
        CheckingLoginAttempt();
    }

    private static void CheckingLoginAttempt()
    {
        Program.logger.LogDebug("Checking login attempt.");

            bool success = _users.Any(user =>
                string.Equals(user.UserName, _userNameLoginAttempt, StringComparison.OrdinalIgnoreCase)
                && user.Password == _passWordLoggingAttempt
            );

            if (success)
            {
                Program.logger.LogInformation($"Login successful for user: {_userNameLoginAttempt}");
                _isLoggedIn = true;
            }
            else
            {
                Program.logger.LogWarning($"Login failed for user: {_userNameLoginAttempt}");
                _isLoggedIn = false;
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
