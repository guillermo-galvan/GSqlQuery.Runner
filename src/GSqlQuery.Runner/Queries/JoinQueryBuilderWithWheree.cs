using GSqlQuery.Extensions;
using GSqlQuery.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GSqlQuery.Runner.Queries
{
    internal class JoinQueryBuilderWithWheree<T1, T2, TDbConnection> : 
        JoinQueryBuilderWithWhereBase<T1, T2, Join<T1, T2>, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>,
        IJoinQueryBuilderWithWheree<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>
        where T1 : class
        where T2 : class
    {
        public JoinQueryBuilderWithWheree(IEnumerable<PropertyOptions> columns, JoinType joinEnum, ConnectionOptions<TDbConnection> options,
            IEnumerable<PropertyOptions> columnsT2 = null) : base(null, options.Formats)
        {
            Options = options;
            ClassOptions classoptions = ClassOptionsFactory.GetClassOptions(typeof(T1));
            JoinInfo joinInfo = new JoinInfo(columns, classoptions, true);
            _joinInfos.Enqueue(joinInfo);

            ClassOptions classoptions2 = ClassOptionsFactory.GetClassOptions(typeof(T2));
            columnsT2 ??= classoptions2.PropertyOptions;

            _joinInfo = new JoinInfo(columnsT2, classoptions2, joinEnum);

            _joinInfos.Enqueue(_joinInfo);

            Columns = _joinInfos.SelectMany(x => x.Columns);
        }

        new public ConnectionOptions<TDbConnection> Options { get; }


        private IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>>
            Join<TJoin, TProperties>(JoinType joinEnum, Expression<Func<TJoin, TProperties>> expression)
            where TJoin : class
        {
            ClassOptionsTupla<IEnumerable<MemberInfo>> options = GeneralExtension.GetOptionsAndMembers(expression);
            GeneralExtension.ValidateMemberInfos(QueryType.Join, options);
            IEnumerable<string> selectMember = options.MemberInfo.Select(x => x.Name);
            return new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, joinEnum, Options, GeneralExtension.GetPropertyQuery(options.ClassOptions, selectMember));
        }

        public override JoinQuery<Join<T1, T2>, TDbConnection> Build()
        {
            string query = CreateQuery();
            return new JoinQuery<Join<T1, T2>, TDbConnection>(query, Columns, _criteria, Options);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> InnerJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Inner, Options);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> LeftJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Left, Options);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> RightJoin<TJoin>() where TJoin : class
        {
            return new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Right, Options);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> InnerJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Inner, expression);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> LeftJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Left, expression);
        }

        public IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>, TDbConnection>, ConnectionOptions<TDbConnection>> RightJoin<TJoin>(Expression<Func<TJoin, object>> expression) where TJoin : class
        {
            return Join(JoinType.Right, expression);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.InnerJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Inner, Options);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.LeftJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Left, Options);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.RightJoin<TJoin>()
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)new JoinQueryBuilderWithWhere<T1, T2, TJoin, TDbConnection>(_joinInfos, JoinType.Right, Options);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.InnerJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Inner, expression);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.LeftJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Left, expression);
        }

        IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>> IJoinQueryBuilderWithWhere<T1, T2, JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.RightJoin<TJoin>(Expression<Func<TJoin, object>> expression)
        {
            return (IComparisonOperators<Join<T1, T2, TJoin>, JoinQuery<Join<T1, T2, TJoin>>, ConnectionOptions<TDbConnection>>)Join(JoinType.Right, expression);
        }

        IWhere<JoinQuery<Join<T1, T2>, TDbConnection>> IQueryBuilderWithWhere<JoinQuery<Join<T1, T2>, TDbConnection>, ConnectionOptions<TDbConnection>>.Where()
        {
            return base.Where();
        }
    }

    internal class JoinQueryBuilderWithWhere<T1, T2, T3, TDbConnection> :
        JoinQueryBuilderWithWhereBase<T1, T2, T3, Join<T1, T2, T3>, JoinQuery<Join<T1, T2, T3>, TDbConnection>, ConnectionOptions<TDbConnection>>,
         IJoinQueryBuilderWithWheree<T1, T2, T3, JoinQuery<Join<T1, T2, T3>, TDbConnection>, ConnectionOptions<TDbConnection>, TDbConnection>
         where T1 : class
        where T2 : class
        where T3 : class
    {
        public JoinQueryBuilderWithWhere(Queue<JoinInfo> joinInfos, JoinType joinEnum, ConnectionOptions<TDbConnection> options, IEnumerable<PropertyOptions> columnsT3 = null) :
            base(joinInfos, joinEnum, options?.Formats, columnsT3)
        {
            Options = options;
        }

        new public ConnectionOptions<TDbConnection> Options { get; }

        public override JoinQuery<Join<T1, T2, T3>, TDbConnection> Build()
        {
            string query = CreateQuery();
            return new JoinQuery<Join<T1, T2, T3>, TDbConnection>(query, Columns, _criteria, Options);
        }

        IWhere<JoinQuery<Join<T1, T2, T3>, TDbConnection>> IQueryBuilderWithWhere<JoinQuery<Join<T1, T2, T3>, TDbConnection>, ConnectionOptions<TDbConnection>>.Where()
        {
            return base.Where();
        }
    }
}