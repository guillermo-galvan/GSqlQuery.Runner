using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "film_category")]
    public class Film_Category : EntityExecute<Film_Category>
    {
        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public long Film_id { get; set; }

        [Column("category_id", Size = 3, IsPrimaryKey = true)]
        public byte Category_id { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public Film_Category()
        { }

        public Film_Category(long film_id, byte category_id, DateTime last_update)
        {
            Film_id = film_id;
            Category_id = category_id;
            Last_update = last_update;
        }
    }
}
