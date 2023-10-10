using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "customer")]
    public class Customer : EntityExecute<Customer>
    {
        [Column("customer_id", Size = 5, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public long Customer_id { get; set; }

        [Column("store_id", Size = 3)]
        public byte Store_id { get; set; }

        [Column("first_name", Size = 45)]
        public string First_name { get; set; }

        [Column("last_name", Size = 45)]
        public string Last_name { get; set; }

        [Column("email", Size = 50)]
        public string Email { get; set; }

        [Column("address_id", Size = 5)]
        public long Address_id { get; set; }

        [Column("active", Size = 3)]
        public byte Active { get; set; }

        [Column("create_date", Size = 19)]
        public DateTime Create_date { get; set; }

        [Column("last_update", Size = 19)]
        public DateTime? Last_update { get; set; }

        public Customer()
        { }

        public Customer(long customer_id, byte store_id, string first_name, string last_name, string email, long address_id, byte active, DateTime create_date, DateTime? last_update)
        {
            Customer_id = customer_id;
            Store_id = store_id;
            First_name = first_name;
            Last_name = last_name;
            Email = email;
            Address_id = address_id;
            Active = active;
            Create_date = create_date;
            Last_update = last_update;
        }
    }
}
