using GSqlQuery.Extensions;
using GSqlQuery.Queries;
using GSqlQuery.Runner.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GSqlQuery
{
    public static class SelectQueryBuilderExtension
    {
        public static IQueryBuilderWithWhere<T, CountQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
            Count<T, TDbConnection>(this IQueryBuilderWithWhere<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }
            return new CountQueryBuilder<T, TDbConnection>(queryBuilder);
        }

        public static IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> OrderBy<T, TProperties, TDbConnection>
           (this IQueryBuilderWithWhere<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, 
            Expression<Func<T, TProperties>> expression, OrderBy orderBy)
           where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Criteria, options);
            return new OrderByQueryBuilder<T, TDbConnection>(options.MemberInfo.Select(x => x.Name), orderBy, queryBuilder);
        }

        public static IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> OrderBy<T, TProperties, TDbConnection>
            (this IAndOr<T, SelectQuery<T, TDbConnection>> queryBuilder, Expression<Func<T, TProperties>> expression, OrderBy orderBy)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Criteria, options);
            SelectQuery<T, TDbConnection> tmp = queryBuilder.Build();
            return new OrderByQueryBuilder<T, TDbConnection>(options.MemberInfo.Select(x => x.Name), orderBy, queryBuilder, new ConnectionOptions<TDbConnection>(tmp.Formats, tmp.DatabaseManagement));
        }

        public static IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> OrderBy<T, TProperties, TDbConnection>
            (this IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, Expression<Func<T, TProperties>> expression, OrderBy orderBy)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Criteria, options);

            if (queryBuilder is IOrderByQueryBuilder order)
            {
                order.AddOrderBy(options, orderBy);
            }

            return queryBuilder;
        }

        public static IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> OrderBy<T, TProperties, TDbConnection>
            (this IQueryBuilderWithWhere<T, JoinQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> queryBuilder, Expression<Func<T, TProperties>> expression, OrderBy orderBy)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Criteria, options);
            return new JoinOrderByQueryBuilder<T, TDbConnection>(options, orderBy, queryBuilder);
        }

        public static IQueryBuilder<OrderByQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> OrderBy<T, TProperties, TDbConnection>
            (this IAndOr<T, JoinQuery<T, TDbConnection>> queryBuilder, Expression<Func<T, TProperties>> expression, OrderBy orderBy)
            where T : class
        {
            if (queryBuilder == null)
            {
                throw new ArgumentNullException(nameof(queryBuilder), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Criteria, options);
            JoinQuery<T, TDbConnection> tmp = queryBuilder.Build();
            return new JoinOrderByQueryBuilder<T, TDbConnection>(options, orderBy, queryBuilder, new ConnectionOptions<TDbConnection>(tmp.Formats, tmp.DatabaseManagement));
        }
    }
}