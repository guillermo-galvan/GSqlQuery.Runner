using GSqlQuery.Extensions;
using GSqlQuery.Runner.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GSqlQuery
{
    public abstract class EntityExecute<T> : Entity<T>, Runner.IRead<T>, Runner.ICreate<T>, Runner.IUpdate<T>, Runner.IDelete<T>
        where T : class
    {
        public static IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>
           Select<TProperties, TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions, Expression<Func<T, TProperties>> expression)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }

            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Read, options);
            return new SelectQueryBuilder<T, TDbConnection>(options.MemberInfo.Select(x => x.Name), connectionOptions);
        }

        public static IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>
            Select<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            return new SelectQueryBuilder<T, TDbConnection>(ClassOptionsFactory.GetClassOptions(typeof(T)).PropertyOptions.Select(x => x.PropertyInfo.Name), connectionOptions);
        }

        public IQueryBuilder<InsertQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Insert<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            return new InsertQueryBuilderExecute<T, TDbConnection>(connectionOptions, this);
        }

        public static IQueryBuilder<InsertQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>> Insert<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions, T entity)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), ErrorMessages.ParameterNotNull);
            }
            return new InsertQueryBuilderExecute<T, TDbConnection>(connectionOptions, entity);
        }

        public static ISet<T, UpdateQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
            Update<TProperties, TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions,
            Expression<Func<T, TProperties>> expression, TProperties value)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            ClassOptionsTupla<MemberInfo> options = GeneralExtension.GetOptionsAndMember(expression);
            GeneralExtension.ValidateMemberInfo(options.MemberInfo, options.ClassOptions);
            return new UpdateQueryBuilder<T, TDbConnection>(connectionOptions, new string[] { options.MemberInfo.Name }, value);
        }

        public ISet<T, UpdateQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
            Update<TProperties, TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions, Expression<Func<T, TProperties>> expression)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression), ErrorMessages.ParameterNotNull);
            }

            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Update, options);
            return new UpdateQueryBuilder<T, TDbConnection>(connectionOptions, this, options.MemberInfo.Select(x => x.Name));
        }

        public static IQueryBuilderWithWhere<T, DeleteQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
            Delete<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            return new DeleteQueryBuilder<T, TDbConnection>(connectionOptions);
        }
    }
}