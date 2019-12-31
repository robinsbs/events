using Microsoft.VisualStudio.TestTools.UnitTesting;
using SkyBlueSoftware.TestFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SkyBlueSoftware.Patterns.Test
{
    [TestClass]
    public class Fowler_Repository
    {
        private TestHarness t = TestHarness.Create();

        [TestInitialize]
        public void Initialize()
        {
            t = TestHarness.Create();
        }

        [TestMethod]
        public void Fowler_Repository_Example()
        {
            var log = new List<string>();

            var storage = new Storage(log);
            var repositoryA = new RepositoryA(storage);
            var repositoryB = new RepositoryB(storage);
            var aRead = repositoryA.Read(1);
            var aWrite = repositoryA.Write(new A(10, "Ten"));
            var bRead = repositoryB.Read(Guid.Empty);
            var bWrite = repositoryB.Write(new B(Guid.Empty, 1.23));

            t.Verify(new { aRead, aWrite, bRead, bWrite, log });
        }


        [TestMethod]
        public void Fowler_Repository_UnitOfWork_Example()
        {
            var log = new List<string>();

            var storage = new Storage(log);
            A aRead;
            WriteResult aWrite;
            B bRead;
            WriteResult bWrite;
            using (var unitOfWork = new UnitOfWork(storage, new List<UnitOfWorkWrite>()))
            {
                var repositoryA = new RepositoryA(unitOfWork);
                var repositoryB = new RepositoryB(unitOfWork);
                aRead = repositoryA.Read(1);
                aWrite = repositoryA.Write(new A(10, "Ten"));
                bRead = repositoryB.Read(Guid.Empty);
                bWrite = repositoryB.Write(new B(Guid.Empty, 1.23));
            }

            t.Verify(new { aRead, aWrite, bRead, bWrite, log });
        }

        class A
        {
            public A(int id, string name)
            {
                Id = id;
                Name = name;
            }

            public int Id { get; }
            public string Name { get; }
        }

        class B
        {
            public B(Guid id, double value)
            {
                Id = id;
                Value = value;
            }

            public Guid Id { get; }
            public double Value { get; }
        }

        interface IStorage
        {
            IStorageValues[] Query(string command, params object[] args);
            IStorageValues Read(string command, params object[] args);
            int Write(string command, params object[] args);
        }

        interface IStorageValues : IEnumerable<KeyValuePair<string, object>> 
        {
            object this[string key] { get; }
        }

        class Storage : IStorage
        {
            private readonly IList<string> log;

            public Storage(IList<string> log)
            {
                this.log = log;
            }

            public IStorageValues[] Query(string command, params object[] args)
            {
                return new IStorageValues[] { new ReadResult(new Dictionary<string, object>()) };
            }

            public IStorageValues Read(string command, params object[] args)
            {
                var values = new Dictionary<string, object>();
                if (command == "ALoad") 
                { 
                    values.Add("Id", 1); 
                    values.Add("Name", "a"); 
                }
                if (command == "BLoad") 
                { 
                    values.Add("Id", Guid.Empty); 
                    values.Add("Value", 7.89); 
                }
                var result = new ReadResult(values);
                var arguments = string.Join(", ", values.Select(x => $"{x.Key}={x.Value}"));
                log.Add($"{command} => {arguments}");
                return result;
            }

            public int Write(string command, params object[] args) 
            {
                var arguments = string.Join(", ", args.Select(x => x.ToString()));
                log.Add($"{command}({arguments})");
                return 1;
            }
        }

        class ReadResult : IStorageValues
        {
            private readonly IDictionary<string, object> values;

            public object this[string key] => values[key];
            public ReadResult(IDictionary<string, object> values) => this.values = values;
            public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => values.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        class UnitOfWork : IStorage, IDisposable
        {
            private readonly IStorage storage;
            private readonly IList<UnitOfWorkWrite> writes;

            public UnitOfWork(IStorage storage, IList<UnitOfWorkWrite> writes)
            {
                this.storage = storage;
                this.writes = writes;
            }

            public IStorageValues[] Query(string command, params object[] args) => storage.Query(command, args);
            public IStorageValues Read(string command, params object[] args) => storage.Read(command, args);
            public int Write(string command, params object[] args)
            {
                writes.Add(new UnitOfWorkWrite(command, args));
                return 1;
            }

            public void Dispose() 
            {
                foreach (var write in writes)
                {
                    storage.Write(write.Command, write.Args);
                }
            }
        }

        class UnitOfWorkWrite
        {
            public UnitOfWorkWrite(string command, object[] args)
            {
                Command = command;
                Args = args;
            }

            public string Command { get; }
            public object[] Args { get; }
        }

        interface IRepository<TEntity, TKey>
        {
            TEntity Read(TKey key);
            WriteResult Write(TEntity entity);
        }

        class WriteResult
        {
            public WriteResult(bool isSuccess, string message)
            {
                IsSuccess = isSuccess;
                Message = message;
            }

            public bool IsSuccess { get; }
            public string Message { get; }
        }
        
        class RepositoryA : IRepository<A, int>
        {
            private readonly IStorage storage;

            public RepositoryA(IStorage storage) => this.storage = storage;

            public A Read(int id)
            {
                var values = storage.Read("ALoad", id);
                if (values == null) return new A(0, string.Empty);
                var name = values["Name"]?.ToString() ?? string.Empty;
                return new A(id, name);
            }

            public WriteResult Write(A a)
            {
                var result = storage.Write("ASave", a.Id, a.Name);
                return result == 1 ? new WriteResult(true, "Success") : new WriteResult(false, "Error");
            }
        }

        class RepositoryB : IRepository<B, Guid>
        {
            private readonly IStorage storage;
            public RepositoryB(IStorage storage) => this.storage = storage;
            public B Read(Guid id)
            {
                var values = storage.Read("BLoad", id);
                if (values == null) return new B(Guid.Empty, 0);
                double.TryParse(values["Value"]?.ToString(), out var value);
                return new B(id, value);
            }

            public WriteResult Write(B b)
            {
                var result = storage.Write("BSave", b.Id, b.Value);
                return result == 1 ? new WriteResult(true, "Success") : new WriteResult(false, "Error");
            }
        }
    }
}
