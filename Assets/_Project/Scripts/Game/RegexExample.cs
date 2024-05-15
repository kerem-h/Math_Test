using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class RegexExample : MonoBehaviour
{
    void Start()
    {
        string input = "Your input string with hms(5404) and hm(5404)";
        string pattern = @"hms\((.*?)\)|hm\((.*?)\)";

        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);

        foreach (Match match in matches)
        {
            // Extract the value inside the brackets
            string innerValue = match.Groups[1].Value;
            bool isHms = !string.IsNullOrEmpty(innerValue);

            if (!isHms)
            {
                innerValue = match.Groups[2].Value;
            }

            // Parse the value as a float
            if (float.TryParse(innerValue, out float value))
            {
                // Convert the value to clock format
                string clockFormat = GetClockFromSecond(value, isHms);
                Debug.Log("Match found: " + match.Value);
                Debug.Log("Clock format: " + clockFormat);
            }
            else
            {
                Debug.LogWarning("Unable to parse value inside brackets: " + innerValue);
            }
        }
    }

    private static string GetClockFromSecond(float value, bool showSeconds)
    {
        // Ensure value is within 24 hours (86400 seconds)
        if (value < 0) value = 86400 + value;

        value = value % 86400;

        // Convert seconds to clock format
        string clock = "";
        int hours = (int)(value / 3600);
        int minutes = (int)((value % 3600) / 60);
        int seconds = (int)(value % 60);

        if (hours < 10)
        {
            clock += "0" + hours + "h";
        }
        else
        {
            clock += hours + "h";
        }

        if (minutes < 10)
        {
            clock += "0" + minutes + "m";
        }
        else
        {
            clock += minutes + "m";
        }

        if (showSeconds)
        {
            if (seconds < 10)
            {
                clock += "0" + seconds + "s";
            }
            else
            {
                clock += seconds + "s";
            }
        }

        return clock;
    }
}
