using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Assertions;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToColorString(this float value, Color color)
        {
            return value.ToString(CultureInfo.InvariantCulture).ToColorString(color);
        }
		
        public static string ToColorString(this int value, Color color)
        {
            return value.ToString().ToColorString(color);
        }
		
        public static string ToColorString(this string text, Color color)
        {
            var colorHexCode = ColorUtility.ToHtmlStringRGB(color);
            return text.ToColorString(colorHexCode);
        }
		
        public static string ToColorString(this string text, string colorHexCode)
        {
            return $"<color=#{colorHexCode}>{text}</color>";
        }
		
        public static StringBuilder ToColorString(this StringBuilder text, string colorHexCode)
        {
            return text.Insert(0, $"<color=#{colorHexCode}>").Append("</color>");
        }
		
        public static StringBuilder ToColorString(this StringBuilder text, Color color)
        {
            var colorHexCode = ColorUtility.ToHtmlStringRGB(color);
            return text.ToColorString(colorHexCode);
        }
		
        public static string GetRegexMatch(this string text, string pattern)
        {
            var regex = new Regex(pattern);
            var match = regex.Match(text);
            Assert.IsTrue(match.Success);
            return match.Value;
        }
		
        public static string ToCamelCase(this string message)
        {
            message = message.Replace("-", " ").Replace("_", " ");
            message = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(message);
            message = message.Replace(" ", "");
            return message;
        }

        public static string SplitCamelCase(this string camelCaseString)
        {
            if (string.IsNullOrEmpty(camelCaseString)) return camelCaseString;

            string camelCase = Regex.Replace(Regex.Replace(camelCaseString, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
            string firstLetter = camelCase.Substring(0, 1).ToUpper();

            if (camelCaseString.Length > 1)
            {
                string rest = camelCase.Substring(1);

                return firstLetter + rest;
            }

            return firstLetter;
        }
    }
}