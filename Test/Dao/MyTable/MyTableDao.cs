using Spring.Data.Generic;
using System.Collections.Generic;
using Test.Dao.MyTable.Model;

namespace Test.Dao.MyTable
{
    public class MyTableDao : AdoDaoSupport, IMyTableDao
    {
        public string GetConnectionString()
        {
            return AdoTemplate.DbProvider.ConnectionString;
        }

        public IList<MyTableItem> GetAll()
        {
            return AdoTemplate.QueryWithRowMapperDelegate<MyTableItem>(System.Data.CommandType.Text, "SELECT * FROM MyTable", (reader, row) =>
            {
                return new MyTableItem
                {
                    Id = (int)reader.GetValue(reader.GetOrdinal("Id")),
                    Name = reader.GetValue(reader.GetOrdinal("Name"))?.ToString(),
                    Age = (int)reader.GetValue(reader.GetOrdinal("Age"))
                };
            });
        }

        public int Insert(MyTableItem item)
        {
            var param = AdoTemplate.CreateDbParameters();
            param.AddWithValue("name", item.Name).DbType = System.Data.DbType.String;
            param.AddWithValue("age", item.Age).DbType = System.Data.DbType.Int32;
            return (int)AdoTemplate.ExecuteScalar(System.Data.CommandType.Text, "INSERT INTO MyTable (Name, Age) VALUES (@name, @age);SELECT CAST(scope_identity() AS int)", param);
        }

        public MyTableItem FindItem(int id)
        {
            var list = AdoTemplate.QueryWithRowMapperDelegate<MyTableItem>(System.Data.CommandType.Text, "SELECT * FROM MyTable WHERE id = " + id, (reader, row) =>
           {
               return new MyTableItem
               {
                   Id = (int)reader.GetValue(reader.GetOrdinal("Id")),
                   Name = reader.GetValue(reader.GetOrdinal("Name"))?.ToString(),
                   Age = (int)reader.GetValue(reader.GetOrdinal("Age"))
               };
           });

            return list.Count > 0 ? list[0] : null;
        }
    }
}