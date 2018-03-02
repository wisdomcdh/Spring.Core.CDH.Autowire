using Spring.Data.Generic;

namespace Test.Dao.MyTable
{
    public class MyTableDao : AdoDaoSupport, IMyTableDao
    {
        public string GetConnectionString()
        {
            return AdoTemplate.DbProvider.ConnectionString;
        }
    }
}