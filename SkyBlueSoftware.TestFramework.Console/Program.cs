// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using SkyBlueSoftware.Storage.Test;

namespace SkyBlueSoftware.TestFramework.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var test = new Storage_Tests();
            test.Initialize();
            test.Storage_Tests_SqlServer();
        }
    }
}
