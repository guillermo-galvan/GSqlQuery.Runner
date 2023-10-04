using System;

namespace GSqlQuery
{
    public class ConnectionOptions<TDbConnection>
    {
        public IFormats Formats { get; }

        public IDatabaseManagement<TDbConnection> DatabaseManagement { get; }

        public ConnectionOptions(IFormats formats, IDatabaseManagement<TDbConnection> databaseManagement)
        {
            Formats = formats ?? throw new ArgumentNullException(nameof(formats));
            DatabaseManagement = databaseManagement ?? throw new ArgumentNullException(nameof(databaseManagement));
        }
    }
}