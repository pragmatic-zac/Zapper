using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Zapper.Common
{
    // https://exceptionnotfound.net/mapping-datatables-and-datarows-to-objects-in-csharp-and-net-using-reflection/
    public class DataNamesMapper<T> where T : class, new()
    {
        public T Map(DataRow row)
        {
            var columnNames = row.Table.Columns
                .Cast<DataColumn>()
                .Select(x => x.ColumnName)
                .ToList();

            var properties = typeof(T).GetProperties();

            T entity = new T();
            foreach (var property in properties)
            {
                PropertyMapHelper.Map(typeof(T), row, property, entity);
            }

            return entity;
        }

        public IEnumerable<T> Map(DataTable table)
        {
            //Step 1 - Get the Column Names
            var columnNames = table.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList();

            //Step 2 - Get the Property Data Names
            var properties = typeof(T).GetProperties();

            //Step 3 - Map the data
            List<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var prop in properties)
                {
                    PropertyMapHelper.Map(typeof(T), row, prop, entity);
                }
                entities.Add(entity);
            }

            return entities;
        }
    }
}
