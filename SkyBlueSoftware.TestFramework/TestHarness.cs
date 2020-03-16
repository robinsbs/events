// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;


namespace SkyBlueSoftware.TestFramework
{
    public class TestHarness
    {
        private readonly string projectPath;

        public static TestHarness Create() => new TestHarness();

        public TestHarness()
        {
            projectPath = @"..\..\..\";
        }

        public void Verify(object o, string? fileNameOverride = null)
        {
            var fileName = $"{fileNameOverride ?? TestMethodName()}.json";
            var projectFileName = $"{projectPath}{fileName}";
            var actual = CreateActual(o);
            var expected = ReadExpected(projectFileName);
            if (actual == expected.Contents)
            {
                Console.WriteLine($"Verified against {fileName}");
                return;
            }
            WriteExpected(actual, projectFileName);
            var message = $"{(expected.Exists ? "Replaced" : "Created")} expected file: {fileName}";
            if (expected.Exists) Assert.Fail(message); else Assert.Inconclusive(message);
        }

        private static string CreateActual(object o) => JsonConvert.SerializeObject(o, new JsonSerializerSettings { Formatting = Formatting.Indented });
        private static void WriteExpected(string actual, string fileName) => File.WriteAllText(fileName, actual);
        private static (bool Exists, string Contents) ReadExpected(string fileName) => File.Exists(fileName) ? (true, File.ReadAllText(fileName)) : (false, string.Empty);

        private static string TestMethodName() => CallerInfo(x => x.Name);

        private static string CallerInfo(Func<MethodBase, string> selector)
        {
            var result = string.Empty;
            var frames = new StackTrace().GetFrames();
            foreach (var frame in frames)
            {
                if (frame == null) continue;
                var methodBase = frame.GetMethod();
                if (methodBase == null) continue;
                var methodName = methodBase.Name;
                foreach (var customAttributeData in methodBase.CustomAttributes)
                {
                    if (customAttributeData.AttributeType == typeof(TestMethodAttribute) || customAttributeData.AttributeType == typeof(TestInitializeAttribute))
                    {
                        result = selector(methodBase);
                        break;
                    }
                }
            }
            return result;
        }
    }
}
