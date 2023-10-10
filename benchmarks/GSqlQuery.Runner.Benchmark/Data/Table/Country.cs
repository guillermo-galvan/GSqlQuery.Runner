using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "country")]
    public class Country : EntityExecute<Country>
    {
        [Column("country_id", Size = 5, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public long Country_id { get; set; }

        [Column("country", Size = 50)]
        public string Name { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public Country()
        { }

        public Country(long country_id, string name, DateTime last_update)
        {
            Country_id = country_id;
            Name = name;
            Last_update = last_update;
        }
    }
}
