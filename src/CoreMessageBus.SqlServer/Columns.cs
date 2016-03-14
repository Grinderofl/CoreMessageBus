namespace CoreMessageBus.SqlServer
{
    internal static class Columns
    {
        public static class Names
        {
            public const string Id = "Id";
            public const string MessageId = "MessageId";
            public const string ContentType = "ContentType";
            public const string Encoding = "Encoding";
            public const string Type = "Type";
            public const string Data = "Data";
            public const string Created = "Created";
            public const string Deferred = "Deferred";
            public const string Status = "Status";
        }

        public static class Indexes
        {
            public const int IdIndex = 0;
            public const int MessageIdIndex = 1;
            public const int ContentTypeIndex = 2;
            public const int EncodingIndex = 3;
            public const int TypeIndex = 4;
            public const int DataIndex = 5;
            public const int CreatedIndex = 6;
            public const int DeferredIndex = 7;
            public const int StatusIndex = 8;
        }
    }
}