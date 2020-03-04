using System;

namespace SkyBlueSoftware.Framework
{
    public static class DateTimeExtensions
    {
        public static string MDYHH(this DateTime d) => $"{d:M/d/yyyy HH:mm:ss}";
    }
}