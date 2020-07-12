using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;
using Zapper.Common;
using Zapper.Models;
using Zapper.Tests.SqlExecutor;

namespace Zapper.Tests
{
    public class ReadTests
    {
        private readonly SqlExecutor.SqlExecutor _sqlExecutor;
        private string _connectionString = "Server=.;Database=ZapperDB;Trusted_Connection=True;";

        public ReadTests()
        {
            _sqlExecutor = new SqlExecutor.SqlExecutor();
        }

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
                sql = $"INSERT INTO Employee (Name, EmployerId) VALUES ('{Guid.NewGuid().ToString().Substring(1,8)}', 1);";
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
        public void DataManager_GetAirlines()
        {
            // arrange
            SeedData();

            var dataManager = new DataManagerBase();
            string sql = "SELECT * FROM Airline;";

            // act
            var airlines = dataManager.Query<Airline>(sql, _connectionString, CommandType.Text);

            // assert
            Assert.NotNull(airlines);

            // cleanup
            Teardown();
        }

        [Fact]
        public void DataManager_GetEmployees_NotAllColumnsSelected()
        {
            // arrange
            SeedData();

            var dataManager = new DataManagerBase();
            string sql = "SELECT Name, DateFounded FROM Airline;";

            // act
            var airlines = dataManager.Query<Airline>(sql, _connectionString, CommandType.Text);

            // assert
            Assert.NotNull(airlines);

            // cleanup
            Teardown();
        }

        [Fact]
        public void DataManager_MultipleResultSet()
        {
            // arrange
            SeedData();

            var dataManager = new DataManagerBase();
            string sql = "SELECT * FROM Airline; SELECT * FROM Employee;";

            // act
            var result = dataManager.QueryMultiple(sql, _connectionString, CommandType.Text);

            // still a WIP - slow, but this does work
            var airlines = result.Tables[0].ToList<Airline>();
            var employees = result.Tables[1].ToList<Employee>();

            // assert
            Assert.NotNull(result);

            // cleanup
            Teardown();
        }

        // tests to add
        // stored procedure
        // multiple params
        // speed
        // security
        // one to many
    }
}
