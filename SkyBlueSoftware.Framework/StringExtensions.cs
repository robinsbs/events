namespace SkyBlueSoftware.Framework
{
    public static class StringExtensions
    {
        public static bool HasSpaces(this string value) => value.Contains(" ");
        public static bool HasNoSpaces(this string value) => !HasSpaces(value);
    }
}