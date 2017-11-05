using System.Transactions;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class RepositoryTestsBase
    {
        protected TransactionScope Scope;

        //[SetUp]
        public void TransactionSetUp()
        {
            Scope = new TransactionScope();
        }

        //[TearDown]
        public void TransactionTearDown()
        {
            Scope.Dispose();
        }

    }
}