using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Models
{
    public struct DatabaseSelectionModel
    {
        public DatabaseSelectionModel(string databaseNameOrConnectionString, string databaseDisplayName)
        {
            DatabaseNameOrConnectionString = databaseNameOrConnectionString;
            DatabaseDisplayName = databaseDisplayName;
        }

        public string DatabaseNameOrConnectionString { get; set; }

        public string DatabaseDisplayName { get; set; }
    }
}
