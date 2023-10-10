namespace GSqlQuery.Runner.Benchmark.Data.Views
{
    [Table("sakila", "actor_info")]
    public class Actor_Info : Entity<Actor_Info>
    {
        [Column("actor_id", Size = 5)]
        public long Actor_id { get; set; }

        [Column("first_name", Size = 45)]
        public string First_name { get; set; }

        [Column("last_name", Size = 45)]
        public string Last_name { get; set; }

        [Column("film_info", Size = 65535)]
        public string Film_info { get; set; }

        public Actor_Info()
        { }

        public Actor_Info(long actor_id, string first_name, string last_name, string film_info)
        {
            Actor_id = actor_id;
            First_name = first_name;
            Last_name = last_name;
            Film_info = film_info;
        }
    }
}
