using System;

namespace GSqlQuery.Runner.Benchmark.Data.Table
{
    [Table("sakila", "staff")]
    public class Staff : EntityExecute<Staff>
    {
        [Column("staff_id", Size = 3, IsPrimaryKey = true, IsAutoIncrementing = true)]
        public byte Staff_id { get; set; }

        [Column("first_name", Size = 45)]
        public string First_name { get; set; }

        [Column("last_name", Size = 45)]
        public string Last_name { get; set; }

        [Column("address_id", Size = 5)]
        public long Address_id { get; set; }

        [Column("picture", Size = 65535)]
        public byte[] Picture { get; set; }

        [Column("email", Size = 50)]
        public string Email { get; set; }

        [Column("store_id", Size = 3)]
        public byte Store_id { get; set; }

        [Column("active", Size = 3)]
        public byte Active { get; set; }

        [Column("username", Size = 16)]
        public string Username { get; set; }

        [Column("password", Size = 40)]
        public string Password { get; set; }

        [Column("last_update", Size = 0)]
        public DateTime Last_update { get; set; }

        public Staff()
        { }

        public Staff(byte staff_id, string first_name, string last_name, long address_id, byte[] picture, string email, byte store_id, byte active, string username, string password, DateTime last_update)
        {
            Staff_id = staff_id;
            First_name = first_name;
            Last_name = last_name;
            Address_id = address_id;
            Picture = picture;
            Email = email;
            Store_id = store_id;
            Active = active;
            Username = username;
            Password = password;
            Last_update = last_update;
        }
    }
}
