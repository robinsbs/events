using System.Collections.Generic;

namespace SkyBlueSoftware.Storage
{
    public interface IDataProvider
    {
        IEnumerable<IDataRow> Execute(string command);
    }
}