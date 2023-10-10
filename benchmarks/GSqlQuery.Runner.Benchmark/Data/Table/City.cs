using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "city")]
    public class City : EntityExecute<City>
    {
        [Column("city_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long City_id { get; set; }

        [Column("city", Size = 50)]
        public string Name { get; set; }

        [Column("country_id", Size = 5)]
        public long Country_id { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public City()
        { }

        public City(long city_id, string name, long country_id, DateTime last_update)
        {
            City_id = city_id;
            Name = name;
            Country_id = country_id;
            Last_update = last_update;
        }
    }
}
