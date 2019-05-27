using Connexion.DataExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableTests
{
    [TestFixture]
    public class DataTableBasicTests
    {
        private TestDataContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new TestDataContext();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }

        [Test]
        public async Task GetFromDataBaseTest()
        {
            using (var conn = new SqlConnection(_context.ConnectionString))
            {
                var command = new SqlCommand("select * from editor.Infos");
                command.Connection = conn;
                DataTable dataTable = await DataTable.FromSqlCommand(command); 
               
            }
        }
    }
}
