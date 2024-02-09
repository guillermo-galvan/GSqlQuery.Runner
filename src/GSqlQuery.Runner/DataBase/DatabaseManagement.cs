using GSqlQuery.Runner.Transforms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
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
            {
                foreach (IDataParameter item in parameters)
                {
                    command.Parameters.Add(item);
                }
            }

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
            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteNonQuery Query: {@Text} Parameters: {@parameters}", [query.Text, parameters]);
            }
                
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    return await ExecuteNonQueryAsync(connection, query, parameters, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public Task<int> ExecuteNonQueryAsync(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteNonQueryAsync Query: {@Text} Parameters: {@parameters}",[query.Text, parameters]);
            }
                
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
                    return ExecuteReader(connection, query, propertyOptions, parameters);
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
            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteReader Type: {@FullName} Query: {@Text} Parameters: {@parameters}", [typeof(T).FullName, query.Text, parameters]);
            }
            ClassOptions classOptions = ClassOptionsFactory.GetClassOptions(typeof(T));
            ITransformTo<T> transformToEntity = Events.GetTransformTo<T>(classOptions);
            Queue<T> result = new Queue<T>();

            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                using (DbDataReader reader = command.ExecuteReader())
                {
                    IEnumerable<PropertyOptionsInEntity> columns = transformToEntity.GetOrdinalPropertiesInEntity(propertyOptions, query, reader);

                    while (reader.Read())
                    {
                        T tmp = transformToEntity.Generate(columns, reader);
                        result.Enqueue(tmp);
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) where T : class
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    return await ExecuteReaderAsync(connection, query, propertyOptions, parameters, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public async Task<IEnumerable<T>> ExecuteReaderAsync<T>(IConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default) where T : class
        {
            cancellationToken.ThrowIfCancellationRequested();
              
            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteReaderAsync Type: {@FullName} Query: {@Text} Parameters: {@parameters}", [typeof(T).FullName, query.Text, parameters]);
            }

            ClassOptions classOptions = ClassOptionsFactory.GetClassOptions(typeof(T));
            ITransformTo<T> transformToEntity = Events.GetTransformTo<T>(classOptions);

#if NET5_0_OR_GREATER
            IAsyncEnumerable<T> entities = ExecuteReaderAsync(connection, query, propertyOptions, parameters, transformToEntity, cancellationToken);
            Queue<T> result = new Queue<T>();

            await foreach (T item in entities)
            {
                result.Enqueue(item);
            }
            return result;
#else
            return await ExecuteReaderAsync(connection, query, propertyOptions, parameters, transformToEntity, cancellationToken).ConfigureAwait(false);
#endif
        }

#if NET5_0_OR_GREATER
        private async IAsyncEnumerable<T> ExecuteReaderAsync<T>
            (IConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, ITransformTo<T> transformToEntity, [EnumeratorCancellation] CancellationToken cancellationToken = default) where T : class
        {
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    IEnumerable<PropertyOptionsInEntity> columns = transformToEntity.GetOrdinalPropertiesInEntity(propertyOptions, query, reader);

                    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {
                        yield return await transformToEntity.GenerateAsync(columns, reader);
                    }
                }
            }
        }
#else
        private async Task<IEnumerable<T>> ExecuteReaderAsync<T>
                    (IConnection connection, IQuery<T> query, IEnumerable<PropertyOptions> propertyOptions, IEnumerable<IDataParameter> parameters, ITransformTo<T> transformToEntity, CancellationToken cancellationToken = default) where T : class
        {
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                using (DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    Queue<T> result = new Queue<T>();

                    IEnumerable<PropertyOptionsInEntity> columns = transformToEntity.GetOrdinalPropertiesInEntity(propertyOptions, query, reader);

                    while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    {
                        T entity = await transformToEntity.GenerateAsync(columns, reader);
                        result.Enqueue(entity);
                    }

                    return result;
                }
            }
        }
#endif

        public T ExecuteScalar<T>(IQuery query, IEnumerable<IDataParameter> parameters)
        {
            using (IConnection connection = GetConnection())
            {
                try
                {
                    return ExecuteScalar<T>(connection, query, parameters);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public T ExecuteScalar<T>(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters)
        {
            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteScalar Type: {@FullName} Query: {@Text} Parameters: {@parameters}", [typeof(T).FullName, query.Text, parameters]);
            }
                
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                object resultCommand = command.ExecuteScalar();
                return (T)TransformTo.SwitchTypeValue(typeof(T), resultCommand);
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            using (IConnection connection = await GetConnectionAsync(cancellationToken).ConfigureAwait(false))
            {
                try
                {
                    return await ExecuteScalarAsync<T>(connection, query, parameters, cancellationToken).ConfigureAwait(false);
                }
                finally
                {
                    await connection.CloseAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        public async Task<T> ExecuteScalarAsync<T>(IConnection connection, IQuery query, IEnumerable<IDataParameter> parameters, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Events.IsTraceActive)
            {
                Events.WriteTrace("ExecuteScalarAsync Type: {@FullName} Query: {@Text} Parameters: {@parameters}", [typeof(T).FullName, query.Text, parameters]);
            }
                
            using (DbCommand command = CreateCommand(connection, query, parameters))
            {
                object resultCommand = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                return (T)TransformTo.SwitchTypeValue(typeof(T), resultCommand);
            }
        }

        public abstract IConnection GetConnection();

        public abstract Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}