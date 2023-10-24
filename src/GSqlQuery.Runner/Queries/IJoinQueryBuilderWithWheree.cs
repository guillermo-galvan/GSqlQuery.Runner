﻿using System;
using System.Linq.Expressions;

namespace GSqlQuery
{
    public interface IJoinQueryBuilderWithWheree<T1, T2, TReturn, TOptions, TDbConnection> : IJoinQueryBuilderWithWhere<T1, T2, TReturn, TOptions>
        where T1 : class
        where T2 : class
        where TReturn : IQuery<Join<T1, T2>>
    {
        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> InnerJoin<TJoin>() where TJoin : class;

        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> LeftJoin<TJoin>() where TJoin : class;

        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> RightJoin<TJoin>() where TJoin : class;

        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> InnerJoin<TJoin>(Expression<Func<TJoin, object>> expression)
            where TJoin : class;

        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> LeftJoin<TJoin>(Expression<Func<TJoin, object>> expression)
            where TJoin : class;

        new IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, TOptions> RightJoin<TJoin>(Expression<Func<TJoin, object>> expression)
            where TJoin : class;
    }

    public interface IJoinQueryBuilderWithWheree<T1, T2, T3, TReturn, TOptions, TDbConnection> : IJoinQueryBuilderWithWhere<T1, T2, T3, TReturn, TOptions>
        where T1 : class
        where T2 : class
        where T3 : class
        where TReturn : IQuery<Join<T1, T2, T3>>
    {

    }
}