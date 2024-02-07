using GSqlQuery.Extensions;
using GSqlQuery.Runner.Queries;
using GSqlQuery.Runner.Test.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Xunit;

namespace GSqlQuery.Runner.Test.Queries
{
    public class UpdateWhereTest
    {
        private readonly Data.SearchCriteria _equal;
        private readonly UpdateQueryBuilder<Test1, IDbConnection> _updateQueryBuilder;

        public UpdateWhereTest()
        {
            _equal = new Data.SearchCriteria(new DefaultFormats(), new TableAttribute("name"), new ColumnAttribute("column"));
            var columnsValue = new List<string> { nameof(Test1.Id), nameof(Test1.Name), nameof(Test1.Create) };
            _updateQueryBuilder =
                new UpdateQueryBuilder<Test1, IDbConnection>(new ConnectionOptions<IDbConnection>(new TestFormats(), LoadGSqlQueryOptions.GetDatabaseManagmentMock()), columnsValue, string.Empty);
        }

        [Fact]
        public void Should_add_criteria_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> query = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            Assert.NotNull(query);
            query.Add(_equal);
            Assert.True(true);
        }

        [Fact]
        public void Throw_exception_if_null_ISearchCriteria_is_added_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> query = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            Assert.NotNull(query);
            Assert.Throws<ArgumentNullException>(() => query.Add(null));
        }

        [Fact]
        public void Should_build_the_criteria_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> query = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            Assert.NotNull(query);
            query.Add(_equal);

            var criteria = query.BuildCriteria();
            Assert.NotNull(criteria);
            Assert.NotEmpty(criteria);
        }

        [Fact]
        public void Should_get_the_IAndOr_interface_with_expression_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> where = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            var andOr = GSqlQueryExtension.GetAndOr(where, x => x.Id);
            Assert.NotNull(andOr);
        }

        [Fact]
        public void Throw_exception_if_expression_is_null_with_expression_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> where = null;
            Assert.Throws<ArgumentNullException>(() => GSqlQueryExtension.GetAndOr(where, x => x.Id));
        }

        [Fact]
        public void Should_validate_of_IAndOr_UpdateQuery()
        {
            var andOr = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            try
            {
                GSqlQueryExtension.Validate(andOr, x => x.IsTest);
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }

        [Fact]
        public void Throw_exception_if_expression_is_null_in_IAndOr_UpdateQuery()
        {
            IAndOr<Test1, UpdateQuery<Test1, IDbConnection>> where = null;
            Assert.Throws<ArgumentNullException>(() => GSqlQueryExtension.Validate(where, x => x.Id));
        }

        [Fact]
        public void Should_get_the_IAndOr_interface_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> where = new AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>>(_updateQueryBuilder, new DefaultFormats());
            var andOr = GSqlQueryExtension.GetAndOr(where);
            Assert.NotNull(andOr);
        }

        [Fact]
        public void Throw_exception_if_expression_is_null_UpdateQuery()
        {
            AndOrBase<Test1, UpdateQuery<Test1, IDbConnection>, ConnectionOptions<IDbConnection>> where = null;
            Assert.Throws<ArgumentNullException>(() => GSqlQueryExtension.GetAndOr(where));
        }
    }
}