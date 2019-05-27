using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Connexion.DataExtensions;
using System.Reflection;
using System.IO;

namespace DataTableTests
{
    [TestFixture]
    public class QueryCacheTests
    {
        private TestDataContext _context;

        [OneTimeSetUp]
        public void Setup()
        {
            _context = new TestDataContext();
            _context.ConnectionString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = EfCoreTests; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task EntityFrameworkQueryCacheTest()
        {
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "users1.json");

            var users = await _context.Users.FromCache(fileName, true);

            Assert.IsTrue(File.Exists(fileName));

            var users2 = await _context.Users.FromCache(fileName);

            Assert.IsTrue(users2.Count() == 2);

        }

        [Test]
        public async Task EntityFrameworkQueryCacheWithIncludeTest()
        {
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "users2.json");

            var users = await _context.Users
                .Include(u => u.Orders)
                .FromCache(fileName, true);

            Assert.IsTrue(File.Exists(fileName));

            var users2 = await _context.Users.FromCache(fileName);

            Assert.IsTrue(users2.Count() == 2);

        }

        [Test]
        public async Task EntityFrameworkQueryCacheWithFind()
        {
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "users3.json");

            var user = await _context.Users
                .Find(1)
                .FromCacheSingle(fileName, true);
                

            //Assert.IsTrue(File.Exists(fileName));

            var user2 = await _context.Users.Find(1).FromCacheSingle(fileName);

            Assert.IsNotNull(user2);

        }

        [Test]
        public async Task EntityFrameworkQueryCacheWithWhere()
        {
            var fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "users4.json");

            var users = await _context.Users
                .Where(u => u.Name == "Roman")
                .Include(u => u.Orders)
                .FromCache(fileName, true);


            Assert.IsTrue(File.Exists(fileName));

            var users2 = await _context.Users.FromCache(fileName);

            Assert.IsTrue(users2.FirstOrDefault().Name == "Roman");

        }
    }
}
