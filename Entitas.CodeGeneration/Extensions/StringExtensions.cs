using Microsoft.CodeAnalysis.CSharp;

namespace Entitas.CodeGeneration.Extensions;

public static class StringExtensions
{
    const string KeywordPrefix = "@"; // ex: 'class' variable name is illegal, @class is ok
    
    public static string ToUpperFirst(this string str)
    {
        return !string.IsNullOrEmpty(str) ? char.ToUpper(str[0]).ToString() + str.Substring(1) : str;
    }

    public static string ToLowerFirst(this string str)
    {
        return !string.IsNullOrEmpty(str) ? char.ToLower(str[0]).ToString() + str.Substring(1) : str;
    }
    
    public static string AddPrefixIfIsKeyword(this string name)
    {
        if (!SyntaxFacts.IsValidIdentifier(name))
            name = KeywordPrefix + name;

        return name;
    }

    public static string RemoveLast(this string str, string pattern)
    {
        if (str.EndsWith(pattern))
            return str.Substring(0, str.Length - pattern.Length);

        return str;
    }
}