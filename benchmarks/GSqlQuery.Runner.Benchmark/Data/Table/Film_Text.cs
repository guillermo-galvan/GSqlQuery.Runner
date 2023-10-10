namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "film_text")]
    public class Film_Text : EntityExecute<Film_Text>
    {
        [Column("film_id", Size = 5, IsPrimaryKey = true)]
        public long Film_id { get; set; }

        [Column("title", Size = 255)]
        public string Title { get; set; }

        [Column("description", Size = 65535)]
        public string Description { get; set; }

        public Film_Text()
        { }

        public Film_Text(long film_id, string title, string description)
        {
            Film_id = film_id;
            Title = title;
            Description = description;
        }
    }
}
