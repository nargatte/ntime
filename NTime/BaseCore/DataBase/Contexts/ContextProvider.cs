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
            PersistanceNameOrConnectionString = nameOrConnectionString;
        }

        protected static string PersistanceNameOrConnectionString { get; private set; } = "NTime";

        protected string UnitNameOrConnectionString { get; set; }

        public async Task DoAsync(Func<NTimeDBContext, Task> action)
        {
            using (NTimeDBContext context = new NTimeDBContext(UnitNameOrConnectionString))
            {
                await action(context);
            }
        }

        public static List<DatabaseSelectionModel> AvailableDatabases { get; } = new List<DatabaseSelectionModel>
        {
            //new DatabaseSelectionModel("NTimeTestServer", "Serwer testowy"),
            //new DatabaseSelectionModel("NTimeProductionServer", "Serwer produkcyjny"),
            new DatabaseSelectionModel("NTimeLocalDb", "Baza lokalna1"),
            new DatabaseSelectionModel("NTimeLocalDb", "Baza lokalna2"),
            new DatabaseSelectionModel("NTimeLocalDb", "Baza lokalna3"),
        };
    }
}