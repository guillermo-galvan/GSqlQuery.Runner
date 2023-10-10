using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "payment")]
    public class Payment : EntityExecute<Payment>
    {
        [Column("payment_id", Size = 5, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public long Payment_id { get; set; }

        [Column("customer_id", Size = 5)]
        public long Customer_id { get; set; }

        [Column("staff_id", Size = 3)]
        public byte Staff_id { get; set; }

        [Column("rental_id", Size = 10)]
        public int? Rental_id { get; set; }

        [Column("amount", Size = 5)]
        public decimal Amount { get; set; }

        [Column("payment_date", Size = 0)]
        public DateTime Payment_date { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime? Last_update { get; set; }

        public Payment()
        { }

        public Payment(long payment_id, long customer_id, byte staff_id, int? rental_id, decimal amount, DateTime payment_date, DateTime? last_update)
        {
            Payment_id = payment_id;
            Customer_id = customer_id;
            Staff_id = staff_id;
            Rental_id = rental_id;
            Amount = amount;
            Payment_date = payment_date;
            Last_update = last_update;
        }
    }
}
