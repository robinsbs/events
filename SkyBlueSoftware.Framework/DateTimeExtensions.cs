// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace SkyBlueSoftware.Framework
{
    public static class DateTimeExtensions
    {
        public static string MDYHH(this DateTime d) => $"{d:M/d/yyyy HH:mm:ss}";
    }
}