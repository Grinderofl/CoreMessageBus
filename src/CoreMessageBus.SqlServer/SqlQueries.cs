namespace CoreMessageBus.SqlServer
{
    public class SqlQueries
    {
        public SqlQueries(string schemaName, string tableName)
        {
            var tableNameWithSchema = string.Format(
                "{0}.{1}", DelimitIdentifier(schemaName), DelimitIdentifier(tableName));

            PeekQueue = string.Format(_peekFormat, tableNameWithSchema);
            DeQueue = string.Format(_deQueue, tableNameWithSchema);
            Error = string.Format(_error, tableNameWithSchema);

        }

        private string DelimitIdentifier(string identifier)
        {
            return "[" + identifier.Replace("]", "]]") + "]";
        }

        private string _peekFormat = "SELECT TOP (1) Id, MessageId, ContentType, Encoding, Type, Data, Created, Deferred, Status FROM {0} WHERE (Deferred IS NULL OR Deferred < getdate()) AND Status = 'Queued'  ORDER BY Deferred ASC, Created DESC";
        private string _deQueue = "UPDATE {0} SET Status = 'Dequeued' WHERE Id = @Id";
        private string _error = "UPDATE {0} SET Status = 'Error' WHERE Id = @Id";
        public string PeekQueue { get; }
        public string DeQueue { get; }
        public string Error { get; }
    }
}