using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.SqlServer.Dac;
using NUnit.Framework;

namespace IntegrationTests.Tests
{
    public class TestDatabase
    {
        private readonly string databaseName;

        private readonly string dacpacPath;

        private readonly string connectionString;

        private readonly string dbScriptsFolderPath;

        public TestDatabase(string connectionString)
        {
            this.connectionString = connectionString;

            databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;

            dacpacPath = Path.GetFullPath(
                Path.Combine(
                    TestContext.CurrentContext.TestDirectory, 
                    @"..\..\..\..\IntegrationTests.Database\bin\debug\IntegrationTests.Database.dacpac"));
            dbScriptsFolderPath = @"..\..\..\DbScripts\";
        }

        public void Initialise()
        {
            DeployDatabase();
        }

        public void RunDbScript(string scriptName)
        {
            var sql = File.ReadAllText(dbScriptsFolderPath + scriptName);
            RunCustomSql(sql);
        }

        public void CleanData()
        {
            RunDbScript("DataCleanup.sql");
        }

        public void RunCustomSql(string sql)
        {
            using var dbConnection = GetSqlConnection();
            using IDbCommand command = dbConnection.CreateCommand();

            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            command.CommandText = sql;
            command.ExecuteNonQuery();
        }

        private void DeployDatabase()
        {
            using(var dacPackage = DacPackage.Load(dacpacPath))
            {
                var dacDeployOptions = new DacDeployOptions
                {
                    // true so that we can deploy to LocalDB
                    AllowIncompatiblePlatform = true,
                    // true so that the deploy works even if we add a new non-nullable column
                    GenerateSmartDefaults = true,
                    // true so that the old objects are dropped
                    DropObjectsNotInSource = true,
                    // false so that the deploy succeeds even if some columns are dropped
                    BlockOnPossibleDataLoss = false
                };

                var instance = new DacServices(connectionString);
                // notice that we set value of upgradeExisting to true
                instance.Deploy(dacPackage, databaseName, upgradeExisting: true, dacDeployOptions);
            }
            CleanData();
        }

        private SqlConnection GetSqlConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
