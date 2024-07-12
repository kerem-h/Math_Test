using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Helper
{
    private const string EmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    public static bool ValidateEmail (string email)
    {
        if (email != null)
            return Regex.IsMatch (email, EmailPattern);
        return false;
    }
    public static float TruncateTo(this float value, int decimalPlaces)
    {
        float factor = (float)Math.Pow(10, decimalPlaces);
        return (float)Math.Truncate(value * factor) / factor;
    }
    public static string ConvertToNormalNotation(string scientificNotation)
    {
        var split = scientificNotation.Split(new char[] { 'E', 'e' });
        if (split.Length > 1)
        {
            var numberPart = split[0];
            var exponentPart = split[1];

            // Handle the number part (removing decimal point)
            var originalDecimalPosition = numberPart.IndexOf('.');
            if (originalDecimalPosition != -1)
            {
                numberPart = numberPart.Replace(".", "");
            }
            else
            {
                originalDecimalPosition = numberPart.Length;
            }

            // Handle the exponent part
            int.TryParse(exponentPart, out var exponent);

            // Calculate the new decimal position
            var newDecimalPosition = originalDecimalPosition + exponent;

            // Add necessary zeros if the new decimal position is outside the current length of the number
            if (newDecimalPosition > numberPart.Length)
            {
                numberPart = numberPart.PadRight(newDecimalPosition, '0');
            }
            else if (newDecimalPosition < 0)
            {
                numberPart = numberPart.PadLeft(numberPart.Length - newDecimalPosition, '0');
                newDecimalPosition = 1; // Decimal point goes right after the leading zero(s)
            }

            // Insert the decimal point at the new position if necessary
            if (newDecimalPosition < numberPart.Length)
            {
                numberPart = numberPart.Insert(newDecimalPosition, ".");
            }

            return numberPart;
        }
        else
        {
            // No scientific notation, return the input
            return scientificNotation;
        }
    }

    public static void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}
