using GSqlQuery;

namespace Example.Entities.GSqlQuery.WithScheme.Views
{
    [Table("sakila", "sales_by_store")]
    public class Sales_By_Store : Entity<Sales_By_Store>
    {
        [Column("store", Size = 101)]
        public string Store { get; set; }

        [Column("manager", Size = 91)]
        public string Manager { get; set; }

        [Column("total_sales", Size = 27)]
        public decimal? Total_sales { get; set; }

        public Sales_By_Store()
        { }

        public Sales_By_Store(string store, string manager, decimal? total_sales)
        {
            Store = store;
            Manager = manager;
            Total_sales = total_sales;
        }
    }
}
