using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "rental")]
    public class Rental : EntityExecute<Rental>
    {
        [Column("rental_id", Size = 10, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public int Rental_id { get; set; }

        [Column("rental_date", Size = 19)]
        public DateTime Rental_date { get; set; }

        [Column("inventory_id", Size = 7)]
        public int Inventory_id { get; set; }

        [Column("customer_id", Size = 5)]
        public long Customer_id { get; set; }

        [Column("return_date", Size = 19)]
        public DateTime? Return_date { get; set; }

        [Column("staff_id", Size = 3)]
        public byte Staff_id { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime Last_update { get; set; }

        public Rental()
        { }

        public Rental(int rental_id, DateTime rental_date, int inventory_id, long customer_id, DateTime? return_date, byte staff_id, DateTime last_update)
        {
            Rental_id = rental_id;
            Rental_date = rental_date;
            Inventory_id = inventory_id;
            Customer_id = customer_id;
            Return_date = return_date;
            Staff_id = staff_id;
            Last_update = last_update;
        }
    }
}
