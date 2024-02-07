﻿using GSqlQuery.Extensions;
using GSqlQuery.Runner.Extensions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery
{
    public class CountQuery<T, TDbConnection> : CountQuery<T>, IExecute<int, TDbConnection>
        where T : class
    {
        public IDatabaseManagement<TDbConnection> DatabaseManagement { get; }

        internal CountQuery(string text, IEnumerable<PropertyOptions> columns, IEnumerable<CriteriaDetail> criteria, ConnectionOptions<TDbConnection> connectionOptions)
            : base(text, columns, criteria, connectionOptions.Formats)
        {
            DatabaseManagement = connectionOptions.DatabaseManagement;
        }

        public int Execute()
        {
            return DatabaseManagement.ExecuteScalar<int>(this, this.GetParameters<T, TDbConnection>(DatabaseManagement));
        }

        public int Execute(TDbConnection dbConnection)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection), ErrorMessages.ParameterNotNull);
            }

            return DatabaseManagement.ExecuteScalar<int>(dbConnection, this, this.GetParameters<T, TDbConnection>(DatabaseManagement));
        }

        public Task<int> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            return DatabaseManagement.ExecuteScalarAsync<int>(this, this.GetParameters<T, TDbConnection>(DatabaseManagement), cancellationToken);
        }

        public Task<int> ExecuteAsync(TDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (dbConnection == null)
            {
                throw new ArgumentNullException(nameof(dbConnection), ErrorMessages.ParameterNotNull);
            }
            return DatabaseManagement.ExecuteScalarAsync<int>(dbConnection, this, this.GetParameters<T, TDbConnection>(DatabaseManagement), cancellationToken);
        }
    }
}