using SQLite;

namespace CHSBackOffice.Database.Models
{
    public class SQLiteParent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

    }
}
