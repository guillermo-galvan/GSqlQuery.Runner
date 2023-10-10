using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "category")]
    public class Category : EntityExecute<Category>
    {
        [Column("category_id", Size = 3, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public byte Category_id { get; set; }

        [Column("name", Size = 25)]
        public string Name { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public Category()
        { }

        public Category(byte category_id, string name, DateTime last_update)
        {
            Category_id = category_id;
            Name = name;
            Last_update = last_update;
        }
    }
}
