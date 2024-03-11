using System;
using System.Text.RegularExpressions;

public class StringModifier
{
    private static Random random = new Random();

    public static string RandomlyDeleteSlashOrAsterisk(string input)
    {
        return Regex.Replace(input, @"\*/|\*/", m =>
        {
            // Randomly choose between 0 or 1
            int choice = random.Next(2);
            
            // If the choice is 0, remove '/'
            // If the choice is 1, remove '*'
            // The decision affects only the first character found, appropriate for both "*/" and "/*" cases.
            return choice == 0 ? m.Value.Replace("/", "") : m.Value.Replace("*", "");
        });
    }
    public static string RandomlyDeletePlusOrMinus(string input)
    {
        return Regex.Replace(input, @"(\+\-|\-\+)", m =>
        {
            // Randomly choose between 0 or 1
            int choice = random.Next(2);
            
            // If the choice is 0, remove '+'
            // If the choice is 1, remove '-'
            // Note: This will replace the first occurrence of the chosen character in the matched group
            return choice == 0 ? m.Value.Replace("+", "" ) : m.Value.Replace("-", "" );
            
        });
    }
    
}