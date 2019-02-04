using BaseCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ContextProvider : IContextProvider
    {
        public ContextProvider()
        {
            UnitNameOrConnectionString = PersistanceNameOrConnectionString;
        }

        public ContextProvider(string unitNameOrConnectionString)
        {
            UnitNameOrConnectionString = unitNameOrConnectionString;
        }

        public static void SetPersistanceNameOrConnectionString(string nameOrConnectionString)
        {
            UnitNameOrConnectionString = PersistanceNameOrConnectionString = nameOrConnectionString;
        }

        protected static string PersistanceNameOrConnectionString { get; private set; } = "NTime";

        protected static string UnitNameOrConnectionString { get; set; }

        public async Task DoAsync(Func<NTimeDBContext, Task> action)
        {
            using (NTimeDBContext context = new NTimeDBContext(UnitNameOrConnectionString))
            {
                await action(context);
            }
            UnitNameOrConnectionString = PersistanceNameOrConnectionString;
        }

        public static List<DatabaseSelectionModel> AvailableDatabases { get; } = new List<DatabaseSelectionModel>
        {
            //new DatabaseSelectionModel("NTimeLocalDb", "Baza lokalna"),
            new DatabaseSelectionModel("NTimeTestServer", "Serwer testowy"),
            new DatabaseSelectionModel("NTimeProductionServer", "Serwer produkcyjny"),
        };
    }
}