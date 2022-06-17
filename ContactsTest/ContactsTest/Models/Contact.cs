using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace ContactsTest.Models
{
    public class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }
    }
}
