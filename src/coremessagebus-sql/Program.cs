using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace coremessagebus_sql
{
    public class Program
    {
        private string _connectionString = null;
        private string _schemaName = null;
        private string _queuesTableName = null;
        private string _queueItemsTableName = null;
        private ILogger _logger;

        public Program()
        {
            var loggerFatory = new LoggerFactory();
            loggerFatory.AddConsole();
            _logger = loggerFatory.CreateLogger<Program>();
        }

        public static int Main(string[] args)
        {
            return new Program().Run(args);
        }

        private int Run(string[] args)
        {
            try
            {
                var description =
                    "Creates tables and indexes in Microsoft SQL Server database to be used for CoreServiceBus Queue";
                var app = new CommandLineApplication();
                app.Name = "coremessagebus-sql";
                app.Description = description;

                app.HelpOption("-?|-h|--help");
                app.Command("create", command =>
                {
                    command.Description = description;
                    var connectionStringArg = command.Argument("[connectionString]",
                        "The connection string to connect to the database.");
                    var schemaNameArg = command.Argument("[schemaName]", "Name of the table schema.");
                    var queuesTableNameArg = command.Argument("[queuesTableName]", "Name of the table storing Queues.");
                    var queueItemsTableNameArg = command.Argument("[queueItemsTableName]",
                        "Name of the table storing Queue Items");
                    command.HelpOption("-?|-h|--help");
                    command.OnExecute(() =>
                    {
                        if (new[]
                        {
                            connectionStringArg.Value, schemaNameArg.Value, queueItemsTableNameArg.Value,
                            queuesTableNameArg.Value
                        }.Any(string.IsNullOrEmpty))
                        {
                            _logger.LogWarning("Invalid input");
                            app.ShowHelp();
                            return 2;
                        }

                        _connectionString = connectionStringArg.Value;
                        _schemaName = schemaNameArg.Value;
                        _queueItemsTableName = queueItemsTableNameArg.Value;
                        _queuesTableName = queuesTableNameArg.Value;

                        CreateTableAndIndexes();

                        return 0;
                    });
                });

                app.OnExecute(() =>
                {
                    app.ShowHelp();
                    return 2;
                });

                return app.Execute(args);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("An error occurred. {0}", ex.Message);
                return 1;
            }
        }

        private void CreateTableAndIndexes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sqlQueries = new SqlQueries(_schemaName, _queuesTableName, _queueItemsTableName);
                var command = new SqlCommand(sqlQueries.TableInfo, connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _logger.LogWarning(
                            $"Tables with schema '{_schemaName}' and names '{_queuesTableName}' & '{_queueItemsTableName}' already exist. Please provide different names and try again.");
                        return;
                    }
                }

                using (var tx = connection.BeginTransaction())
                {
                    try
                    {

                        command = new SqlCommand(sqlQueries.CreateQueuesTable, connection, tx);
                        command.ExecuteNonQuery();

                        command = new SqlCommand(sqlQueries.CreateQueueItemsTable, connection, tx);
                        command.ExecuteNonQuery();

                        command = new SqlCommand(sqlQueries.CreateIndexes, connection, tx);
                        command.ExecuteNonQuery();
                        tx.Commit();
                        _logger.LogInformation("Table and indexes were created successfully.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("An error occurred while trying to create the tables and indexes.", ex);
                        tx.Rollback();
                    }
                }
            }
        }
    }
}
