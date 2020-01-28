using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Native;
using DevExpress.DataAccess.Web;

namespace BMS.Reports
{
    public class MyDataSourceWizardConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider
    {
        public Dictionary<string, string> GetConnectionDescriptions()
        {
            Dictionary<string, string> connections = AppConfigHelper.GetConnections().Keys.ToDictionary(x => x, x => x);

            // Customize the loaded connections list. 
            connections.Add("CustomMdbConnection", "Custom DB Connection");
            connections.Add("CustomSqlConnection", "Custom SQL Connection");
            return connections;
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name)
        {
            // Return custom connection parameters for the custom connection(s). 
            if (name == "CustomMdbConnection")
            {
                return new Access97ConnectionParameters("|DataDirectory|nwind.mdb", "", "");
            }
            else if (name == "CustomSqlConnection")
            {
                return new MsSqlConnectionParameters("localhost", "BMS", "sa", "Sldd140124", MsSqlAuthorizationType.SqlServer);
            }
            return AppConfigHelper.LoadConnectionParameters(name);
        }
    }
}