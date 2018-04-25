using System.Collections.Generic;
using Test.Dao.MyTable.Model;

namespace Test.Dao.MyTable
{
    public interface IMyTableDao
    {
        IList<MyTableItem> GetAll();

        string GetConnectionString();

        int Insert(MyTableItem item);

        MyTableItem FindItem(int id);
    }
}