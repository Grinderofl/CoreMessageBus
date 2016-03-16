namespace coremessagebus_sql
{
    internal class SqlQueries
    {
        private const string TableInfoFormat =
            "SELECT TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE " +
            "FROM INFORMATION_SCHEMA.TABLES " +
            "WHERE TABLE_SCHEMA = '{0}' " +
            "AND (TABLE_NAME = '{1}' OR TABLE_NAME = '{2}')";

        private const string CreateQueuesTableFormat =
            "CREATE TABLE {0}( " + 
            "Id uniqueidentifier NOT NULL, " + 
            "MessageId uniqueidentifier NOT NULL, " +
            "Encoding nvarchar(255) NOT NULL, " + 
            "Data nvarchar(MAX) NULL, " +
            "Created datetime NULL, " +
            "Deferred datetime NULL, " +
            "[Status] nvarchar(50) NOT NULL, " +
            "[Type] nvarchar(255) NULL, " +
            "QueueId int NOT NULL, " +
            "CONSTRAINT pk_Id PRIMARY KEY CLUSTERED(Id ASC))";

        private const string CreateQueueItemsTableFormat =
            "CREATE TABLE {0}( " +
            "Id int IDENTITY(1,1) NOT NULL, " +
            "Name nvarchar(255) NOT NULL, " +
            "CONSTRAINT pk_Id PRIMARY KEY CLUSTERED(Id ASC))";

        private const string CreateIndexesFormat = "";

        public SqlQueries(string schemaName, string queuesTableName, string queueItemsTableName)
        {
            var queuesTableNameWithSchema = $"{DelimitIdentifier(schemaName)}.{DelimitIdentifier(queuesTableName)}";
            var queueItemsTableNameWithSchema = $"{DelimitIdentifier(schemaName)}.{DelimitIdentifier(queueItemsTableName)}";

            TableInfo = string.Format(TableInfoFormat, EscapeLiteral(schemaName), EscapeLiteral(queuesTableName),
                EscapeLiteral(queueItemsTableName));

            CreateQueuesTable = string.Format(CreateQueuesTableFormat, queuesTableNameWithSchema);
            CreateQueueItemsTable = string.Format(CreateQueueItemsTableFormat, queueItemsTableNameWithSchema);
            CreateIndexes = string.Format(CreateIndexesFormat, queuesTableNameWithSchema, queueItemsTableNameWithSchema);
        }


        public string TableInfo { get; }
        public string CreateQueuesTable { get; }
        public string CreateQueueItemsTable { get; }
        public string CreateIndexes { get; }

        // From EF's SqlServerQuerySqlGenerator
        private string DelimitIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

        private string EscapeLiteral(string literal)
        {
            return literal.Replace("'", "''");
        }
    }
}