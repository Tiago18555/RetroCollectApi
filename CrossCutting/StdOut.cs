using Microsoft.Extensions.Logging;
using System;

namespace CrossCutting;

public static partial class StdOut
{
    public static void Error<T>(this Logger<T> logger, string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        logger.LogError(message);     
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);  
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Warning<T>(this Logger<T> logger, string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);  
        logger.LogWarning(message);       
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Warning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);     
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Info<T>(this Logger<T> logger, string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        logger.LogInformation(message);      
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message); 
        Console.ForegroundColor = ConsoleColor.White;
    }
}
