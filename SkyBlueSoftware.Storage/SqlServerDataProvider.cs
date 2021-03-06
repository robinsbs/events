﻿// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SkyBlueSoftware.Framework;

namespace SkyBlueSoftware.Storage
{
    public class SqlServerDataProvider : DataProvider
    {
        private readonly string connectionString;

        public SqlServerDataProvider(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override IEnumerable<IDataRow> Execute(string command, params (string Name, object Value)[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var dbCommand = connection.CreateCommand())
                {
                    dbCommand.CommandText = command;
                    if (command.HasNoSpaces())
                    {
                        dbCommand.CommandType = CommandType.StoredProcedure;
                        foreach (var parameter in parameters)
                        {
                            dbCommand.Parameters.Add(new SqlParameter(parameter.Name, parameter.Value));
                        }
                    }
                    using (var reader = dbCommand.ExecuteReader())
                    {
                        var dataReader = new DataReader(reader, CreateColumns(reader));
                        while (dataReader.Read())
                        {
                            yield return dataReader;
                        }
                    }
                }
            }
        }
    }
}