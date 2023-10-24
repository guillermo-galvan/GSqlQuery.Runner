using GSqlQuery.Queries;
using System.Collections.Generic;
using System.Linq;

namespace GSqlQuery.Runner.Queries
{
    internal class InsertQueryBuilderExecute<T, TDbConnection> : InsertQueryBuilder<T, InsertQuery<T, TDbConnection>>,
        IQueryBuilder<InsertQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
    {
        public InsertQueryBuilderExecute(ConnectionOptions<TDbConnection> options, object entity) : base(options.Formats, entity)
        {
            Options = options;
        }

        new public ConnectionOptions<TDbConnection> Options { get; }

        public override InsertQuery<T, TDbConnection> Build()
        {
            var query = CreateQuery(out IEnumerable<CriteriaDetail> criteria);
            return new InsertQuery<T, TDbConnection>(query, Columns, criteria, Options, _entity, Columns.FirstOrDefault(x => x.ColumnAttribute.IsAutoIncrementing));
        }
    }
}