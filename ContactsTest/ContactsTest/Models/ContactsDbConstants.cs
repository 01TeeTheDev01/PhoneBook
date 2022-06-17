using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SQLite;

namespace ContactsTest.Models
{
    internal class ContactsDbConstants
    {
        private static readonly string dbPath = $"{Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Contacts.db3")}";
        internal static SQLiteConnectionString ConnectionString => new SQLiteConnectionString(dbPath);
    }
}
