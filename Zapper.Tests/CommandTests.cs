using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Xunit;
using Zapper.Common;
using Zapper.Models;

namespace Zapper.Tests
{
    public class CommandTests
    {
        private readonly SqlExecutor.SqlExecutor _sqlExecutor;
        private string _connectionString = "Server=.;Database=ZapperDB;Trusted_Connection=True;";

        public CommandTests()
        {
            _sqlExecutor = new SqlExecutor.SqlExecutor();
        }

        // method to set up test data
        private void SeedData()
        {
            // clear everything out
            Teardown();

            // create airlines
            string sql = @"
                            SET IDENTITY_INSERT Airline ON
                            INSERT INTO Airline (Id, Name, DateFounded, TotalEmployees) VALUES (1, 'American Airlines', '4/15/1926', 130000);
                            INSERT INTO Airline (Id, Name, DateFounded, TotalEmployees) VALUES (2, 'Delta Airlines', '3/2/1925', 91000);
                            INSERT INTO Airline (Id, Name, DateFounded, TotalEmployees) VALUES (3, 'United Airlines', '4/6/1926', 96000);
                            SET IDENTITY_INSERT Airline OFF;
                          ";
            _sqlExecutor.Execute(sql);

            // create employees for American
            for (int i = 0; i < 10; i++)
            {
                sql = $"INSERT INTO Employee (Name, EmployerId) VALUES ('{Guid.NewGuid().ToString().Substring(1, 8)}', 1);";
                _sqlExecutor.Execute(sql);
            }

            // create employees for Delta
            for (int i = 0; i < 7; i++)
            {
                sql = $"INSERT INTO Employee (Name, EmployerId) VALUES ('{Guid.NewGuid().ToString().Substring(1, 8)}', 2);";
                _sqlExecutor.Execute(sql);
            }
        }

        // teardown method
        private void Teardown()
        {
            string sql = "DELETE FROM Employee; DELETE FROM Airline;";
            _sqlExecutor.Execute(sql);
        }

        [Fact]
        public void DataManager_Execute_InsertOneRecord()
        {
            // arrange
            SeedData();
            var dataManager = new DataManagerBase();

            string sql = "INSERT INTO Airline (Name, DateFounded, TotalEmployees) VALUES ('JetBlue', '01/01/2000', 30000)";

            // act
            int rowsAffected = dataManager.Execute(sql, _connectionString, CommandType.Text);
            var result = dataManager.QuerySingle<Airline>("SELECT * FROM Airline WHERE Name = 'JetBlue'", _connectionString);

            // assert
            Assert.True(rowsAffected > 0);
            Assert.True(result.Name == "JetBlue");

            // cleanup
            Teardown();
        }

        [Fact]
        public void DataManager_Execute_InsertOneRecord_WithParams()
        {
            // arrange
            SeedData();
            var dataManager = new DataManagerBase();

            string sql = $"INSERT INTO Airline (Name, DateFounded, TotalEmployees) VALUES (@Name, '01/01/2000', 30000)";

            string name = Guid.NewGuid().ToString().Substring(0, 5);

            var parameters = new Dictionary<string, string>
            {
                { "Name", name }
            };

            // act
            int rowsAffected = dataManager.Execute(sql, _connectionString, CommandType.Text, parameters);
            var result = dataManager.QuerySingle<Airline>($"SELECT * FROM Airline WHERE Name = '{name}'", _connectionString);

            // assert
            Assert.True(rowsAffected > 0);
            Assert.True(result.Name == name);

            // cleanup
            Teardown();
        }
    }
}
