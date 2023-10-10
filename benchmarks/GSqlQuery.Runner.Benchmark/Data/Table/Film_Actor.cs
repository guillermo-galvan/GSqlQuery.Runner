using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "film_actor")]
    public class Film_Actor : EntityExecute<Film_Actor>
    {
        [Column("actor_id", Size = 5, IsPrimaryKey = true)]
        public long Actor_id { get; set; }

        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public long Film_id { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime Last_update { get; set; }

        public Film_Actor()
        { }

        public Film_Actor(long actor_id, long film_id, DateTime last_update)
        {
            Actor_id = actor_id;
            Film_id = film_id;
            Last_update = last_update;
        }
    }
}
