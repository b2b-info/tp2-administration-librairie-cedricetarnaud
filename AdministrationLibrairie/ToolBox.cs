using BookStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    public class ToolBox
    {
    public static uint ReadUInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (uint.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    public static int ReadInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (int.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    public static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (double.TryParse(Console.ReadLine(), out var value)) return value;

            Console.WriteLine("Invalid number, try again");
        }
    }

    public static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) return input;

            Console.WriteLine("Value cannot be empty");
        }
    }

    public static string ReadOptional(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? "";
    }
}
