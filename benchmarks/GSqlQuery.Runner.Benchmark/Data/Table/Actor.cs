using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "actor")]
    public class Actor : EntityExecute<Actor>
    {
        [Column("actor_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long Actor_id { get; set; }

        [Column("first_name", Size = 45)]
        public string First_name { get; set; }

        [Column("last_name", Size = 45)]
        public string Last_name { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public Actor()
        { }

        public Actor(long actor_id, string first_name, string last_name, DateTime last_update)
        {
            Actor_id = actor_id;
            First_name = first_name;
            Last_name = last_name;
            Last_update = last_update;
        }
    }
}
