using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.DataAccess.Sql;
using DevExpress.Xpo.DB;

namespace BMS.Reports
{
    public class MyDBSchemaProvider : IDBSchemaProviderEx
    {
        DBSchemaProviderEx provider;

        public MyDBSchemaProvider()
        {
            this.provider = new DBSchemaProviderEx();
        }

        public DBTable[] GetTables(SqlDataConnection connection, params string[] tableList)
        {
            return provider.GetTables(connection, tableList)
                .ToArray();
        }

        public DBTable[] GetViews(SqlDataConnection connection, params string[] viewList)
        {
            return provider.GetViews(connection, viewList)
               
                .ToArray();
        }

        public DBStoredProcedure[] GetProcedures(SqlDataConnection connection, params string[] procedureList)
        {
            return provider.GetProcedures(connection, procedureList)
                .Where(storedProcedure => storedProcedure.Arguments.Count == 0)
                .ToArray();
        }

        public void LoadColumns(SqlDataConnection connection, params DBTable[] tables)
        {
            provider.LoadColumns(connection, tables);
        }
    }
}