using GSqlQuery.Extensions;
using GSqlQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GSqlQuery.Runner.Queries
{
    internal class SelectQueryBuilder<T, TDbConnection> : GSqlQuery.Queries.SelectQueryBuilder<T, SelectQuery<T, TDbConnection>>,
        IQueryBuilder<SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>,
        IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>,
        IQueryBuilderWithWhere<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
        where T : class
    {
        public SelectQueryBuilder(IEnumerable<string> selectMember, ConnectionOptions<TDbConnection> options) : base(selectMember, options?.Formats)
        {
            Options = options;
        }

        public SelectQueryBuilder(IEnumerable<PropertyOptions> propertyOptions, ConnectionOptions<TDbConnection> options) : base(options?.Formats)
        {
            Columns = propertyOptions;
            Options = options;
        }

        new public ConnectionOptions<TDbConnection> Options { get; }

        public override SelectQuery<T, TDbConnection> Build()
        {
            string text = CreateQuery();
            return new SelectQuery<T, TDbConnection>(text, Columns, _criteria, Options);
        }

        private IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> Join<TJoin, TProperties>
            (JoinType joinEnum, Expression<Func<TJoin, TProperties>> expression)
            where TJoin : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }

            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            IEnumerable<string> selectMember = options.MemberInfo.Select(x => x.Name);
            return new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, joinEnum, Options, GeneralExtension.GetPropertyQuery(options.ClassOptions, selectMember));
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> InnerJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Inner, Options);
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> LeftJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Left, Options);
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> RightJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Right, Options);
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> InnerJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Inner, expression);
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> LeftJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Left, expression);
        }

        public IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> RightJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Right, expression);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.InnerJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Inner, Options);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.LeftJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Left, Options);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.RightJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWheree<T, TJoin, TDbConnection>(Columns, JoinType.Right, Options);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.InnerJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Inner, expression);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.LeftJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Left, expression);
        }

        IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.RightJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T, TJoin>, JoinQuery<Join<T, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Right, expression);
        }

        IWhere<SelectQuery<T, TDbConnection>> IQueryBuilderWithWhere<SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>.Where()
        {
            return base.Where();
        }
    }
}