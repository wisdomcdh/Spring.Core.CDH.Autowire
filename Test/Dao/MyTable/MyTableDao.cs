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
    }
}