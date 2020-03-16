// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System.Collections.Generic;
using System.Linq;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class Fowler_CQRS
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void Fowler_CQRS_Example()
        {
            var data = new[] { new Person(1, "A", 7.0), new Person(2, "B", 7.5), new Person(3, "C", 9.1) };
            var storage = new Storage(data.ToDictionary(x => x.Id, x => x));
            var before = storage.Read();
            var id = before.Skip(1).First().Id;
            storage.Write(new NameWrite(id, "B2"));
            storage.Write(new AgeWrite(id, 8.0));
            var after = storage.Read();
            t.Verify(new { before, after });
        }

        class Person
        {
            public Person(int id, string name, double age)
            {
                Id = id;
                Name = name;
                Age = age;
            }

            public int Id { get; }
            public string Name { get; }
            public double Age { get; }
        }

        class NameWrite
        {
            public NameWrite(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }
            public string Name { get; }
        }

        class AgeWrite
        {
            public AgeWrite(int id, double rating)
            {
                Id = id;
                Age = rating;
            }

            public int Id { get; }
            public double Age { get; }
        }

        class Storage
        {
            private readonly IDictionary<int, Person> collection;

            public Storage(IDictionary<int, Person> collection)
            {
                this.collection = collection;
            }

            public Person[] Read() => collection.Values.ToArray();

            public void Write(NameWrite name)
            {
                var d = Fetch(name.Id);
                Save(new Person(d.Id, name.Name, d.Age));
            }

            public void Write(AgeWrite rating)
            {
                var d = Fetch(rating.Id);
                Save(new Person(d.Id, d.Name, rating.Age));
            }

            private Person Fetch(int id) => collection[id];
            private void Save(Person person) => collection[person.Id] = person;
        }
    }
}
