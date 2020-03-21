// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;

namespace SkyBlueSoftware.Storage
{
    public interface IDataProvider
    {
        IEnumerable<IDataRow> Execute(string command, params (string Name, object Value)[] parameters);
    }
}