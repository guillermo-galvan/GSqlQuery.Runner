using GSqlQuery;
using System;

namespace Example.Entities.GSqlQuery.WithScheme.Table
{
    [Table("sakila", "store")]
    public class Store : EntityExecute<Store>
    {
        [Column("store_id", Size = 3, IsAutoIncrementing = true, IsPrimaryKey = true)]
        public byte Store_id { get; set; }

        [Column("manager_staff_id", Size = 3)]
        public byte Manager_staff_id { get; set; }

        [Column("address_id", Size = 5)]
        public long Address_id { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime Last_update { get; set; }

        public Store()
        { }

        public Store(byte store_id, byte manager_staff_id, long address_id, DateTime last_update)
        {
            Store_id = store_id;
            Manager_staff_id = manager_staff_id;
            Address_id = address_id;
            Last_update = last_update;
        }
    }
}
