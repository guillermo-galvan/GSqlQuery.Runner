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
            IEnumerable<string> selectMember = options.MemberInfo.Select(x => x.Name);
            return new SelectQueryBuilder<T, TDbConnection>(selectMember, connectionOptions);
        }

        public static IJoinQueryBuilder<T, SelectQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>
            Select<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            IEnumerable<PropertyOptions> propertyOptions = ClassOptionsFactory.GetClassOptions(typeof(T)).PropertyOptions;
            return new SelectQueryBuilder<T, TDbConnection>(propertyOptions, connectionOptions);
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

        /// <summary>
        /// Delete query
        /// </summary>
        /// <param name="formats">Formats</param>
        /// <param name="entity">Entity</param>
        /// <returns>Bulder</returns>
        public static IQueryBuilderWithWhere<T, DeleteQuery<T, TDbConnection>, ConnectionOptions<TDbConnection>>
            Delete<TDbConnection>(ConnectionOptions<TDbConnection> connectionOptions, T entity)
        {
            if (connectionOptions == null)
            {
                throw new ArgumentNullException(nameof(connectionOptions), ErrorMessages.ParameterNotNull);
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), ErrorMessages.ParameterNotNull);
            }

            return new DeleteQueryBuilder<T, TDbConnection>(entity, connectionOptions);
        }
    }
}