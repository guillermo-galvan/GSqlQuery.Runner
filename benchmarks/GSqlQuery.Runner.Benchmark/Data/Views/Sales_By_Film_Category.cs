using GSqlQuery;

namespace Example.Entities.GSqlQuery.WithScheme.Views
{
    [Table("sakila", "sales_by_film_category")]
    public class Sales_By_Film_Category : Entity<Sales_By_Film_Category>
    {
        [Column("category", Size = 25)]
        public string Category { get; set; }

        [Column("total_sales", Size = 27)]
        public decimal? Total_sales { get; set; }

        public Sales_By_Film_Category()
        { }

        public Sales_By_Film_Category(string category, decimal? total_sales)
        {
            Category = category;
            Total_sales = total_sales;
        }
    }
}
