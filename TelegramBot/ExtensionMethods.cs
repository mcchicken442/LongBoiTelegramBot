
using System.Text.RegularExpressions;

public static class ExtensionMethods
    {
        public static string SplitByUppercase(this string str) => 
            Regex.Replace(str, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1");
    }