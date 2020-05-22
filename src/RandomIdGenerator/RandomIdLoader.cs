using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;

namespace NetExtensions
{
    public class RandomIdLoader
    {
        private readonly RandomIdGenerator _randomIdGenerator;
        public RandomIdLoader(RandomIdGenerator randomIdGenerator)
        {
            _randomIdGenerator = randomIdGenerator;
        }
        public async Task Start(string dbName, int ids)
        {
            var dataSource = dbName;
            using var connection = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = dataSource }.ConnectionString);
            connection.Open();
            CreateTable(connection);
            for (int i = 0; i < 10000; i++)
            {
                await Load(connection, CreateBatch());
            }
        }

        private string CreateBatch()
        {
            var generate = _randomIdGenerator.Generate(10000);
            var insertText = new StringBuilder();
            insertText.Append("INSERT INTO RandomIds ( BusinessId) VALUES ");
            for (int i = 0; i < generate.Count; i++)
            {
                long l = generate[i];
                if (i == generate.Count - 1)
                {
                    insertText.Append($"({l}); ");
                    continue;
                }
                insertText.Append($"({l}), ");

            }
            return insertText.ToString();
        }

        private static void CreateTable(SqliteConnection connection)
        {
            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = "CREATE TABLE RandomIds (BusinessId bigint NOT NULL)";
            createTableCmd.ExecuteNonQuery();
        }

        private static async Task Load(SqliteConnection connection, string commandText)
        {
            using var transaction = await connection.BeginTransactionAsync();
            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText = commandText;
            await insertCmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();

        }
    }
}
