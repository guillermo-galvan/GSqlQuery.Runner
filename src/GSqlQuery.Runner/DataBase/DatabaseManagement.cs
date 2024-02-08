using GSqlQuery.Runner.Transforms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GSqlQuery.Runner
{
    public abstract class DatabaseManagement : IDatabaseManagement<IConnection>
    {
        protected readonly string _connectionString;

        public DatabaseManagementEvents Events { get; set; }

        public string ConnectionString => _connectionString;

        public DatabaseManagement(string connectionString, DatabaseManagementEvents events)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            Events = events ?? throw new ArgumentNullException(nameof(events));
        }

        internal DbCommand CreateCommand(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            DbCommand command = connection.GetDbCommand();
            command.CommandText = query.Text;

            if (parameters != null)
                command.Parameters.AddRange(parameters.ToArray());

            return command;
        }

        public int ExecuteNonQuery(IQuery query, IEnumerable<IDataParameter> parameters)
        {
            using (IConnection connection = GetConnection())
            {
                try
                {
                    return ExecuteNonQuery(connection, query, parameters); ;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public int ExecuteNonQuery(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            Events.WriteTrace("ExecuteNonQuery Query: {@Text} Parameters: {@parameters}",
             new object[] { query.Text, parameters });
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken))
            {
                try
                {
                    return await ExecuteNonQueryAsync(connection, query, parameters, cancellationToken);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken);
                }
            }
        }

        public Task<int> ExecuteNonQueryAsync(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Events.WriteTrace("ExecuteNonQueryAsync Query: {@Text} Parameters: {@parameters}",
             new object[] { query.Text, parameters });
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                return command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        public IEnumerable<T> ExecuteReader<T>(IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters) 
            where T : class
        {
            using (IConnection connection = GetConnection())
            {
                try
                {
                    return ExecuteReader<T>(connection, query, propertyOptions, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(IConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters) 
            where T : class
        {
            Events.WriteTrace("ExecuteReader Type: {@FullName} Query: {@Text} Parameters: {@parameters}",
              new object[] { typeof(T).FullName, query.Text, parameters });
            ITransformTo<T> transformToEntity = Events.GetTransformTo<T>(ClassOptionsFactory.GetClassOptions(typeof(T)));
            Queue<T> result = new Queue<T>();

            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                using (DbDataReader reader = command.ExecuteReader())
                {
                    IEnumerable<PropertyOptionsInEntity> columns = transformToEntity.GetOrdinalPropertiesInEntity(propertyOptions, query, reader);

                    while (reader.Read())
                    {
                        result.Enqueue(transformToEntity.Generate(columns, reader));
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) where T : class
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken))
            {
                try
                {
                    return await ExecuteReaderAsync(connection, query, propertyOptions, parameters, cancellationToken);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken);
                }
            }
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(IConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            Events.WriteTrace("ExecuteReaderAsync Type: {@FullName} Query: {@Text} Parameters: {@parameters}",
               new object[] { typeof(T).FullName, query.Text, parameters });
            ITransformTo<T> transformToEntity = Events.GetTransformTo<T>(ClassOptionsFactory.GetClassOptions(typeof(T)));
            Queue<T> result = new Queue<T>();

            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    IEnumerable<PropertyOptionsInEntity> columns = transformToEntity.GetOrdinalPropertiesInEntity(propertyOptions, query, reader);

                    while (await reader.ReadAsync(cancellationToken))
                    {
                        result.Enqueue(transformToEntity.Generate(columns, reader));
                    }
                }
            }

            return result;
        }

        public T ExecuteScalar<T>(IQuery query, IEnumerable<IDataParameter> parameters)
        {
            using (IConnection connection = GetConnection())
            {
                try
                {
                    return ExecuteScalar<T>(connection, query, parameters); ;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public T ExecuteScalar<T>(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            Events.WriteTrace("ExecuteScalar Type: {@FullName} Query: {@Text} Parameters: {@parameters}",
                new object[] { typeof(T).FullName, query.Text, parameters });
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                object resultCommand = command.ExecuteScalar();
                return (T)TransformTo.SwitchTypeValue(typeof(T), resultCommand);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken))
            {
                try
                {
                    return await ExecuteScalarAsync<T>(connection, query, parameters, cancellationToken);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken);
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Events.WriteTrace("ExecuteScalarAsync Type: {@FullName} Query: {@Text} Parameters: {@parameters}",
                new object[] { typeof(T).FullName, query.Text, parameters });
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                object resultCommand = await command.ExecuteScalarAsync(cancellationToken);
                return (T)TransformTo.SwitchTypeValue(typeof(T), resultCommand);
            }
        }

        public abstract IConnection GetConnection();

        public abstract Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}