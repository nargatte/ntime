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
            TempNameOrConnectionString = NameOrConnectionString;
        }

        public ContextProvider(string nameOrConnectionString)
        {
            TempNameOrConnectionString = nameOrConnectionString;
        }

        public static void SetNameOrConnectionString(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString;
        }

        protected static string NameOrConnectionString { get; private set; } = "NTimeLocalDb";

        protected string TempNameOrConnectionString { get; set; }

        public async Task DoAsync(Func<NTimeDBContext, Task> action)
        {
            using (NTimeDBContext context = new NTimeDBContext(TempNameOrConnectionString))
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