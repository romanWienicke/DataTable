using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYSDATA = System.Data;
using System.Data.SqlClient;
using DataTableTests;

namespace Connexion.DataExtensions.Performance
{
    public class DataTablePerformance
    {
        [Benchmark(Baseline = true)]
        public SYSDATA.DataTable SystemDataTableTest()
        {
            var context = new TestDataContext();

            using (var conn = new SqlConnection(context.ConnectionString))
            {
                using (var command = new SqlCommand("select * from editor.Infos"))
                {
                    command.Connection = conn;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
                    {
                        var dataTable = new SYSDATA.DataTable();
                        sqlDataAdapter.Fill(dataTable);
                        return dataTable;
                    }
                }

            }
        }

        [Benchmark()]
        public Connexion.DataExtensions.DataTable ConnexionDataTableTest()
        {
            var context = new TestDataContext();

            using (var conn = new SqlConnection(context.ConnectionString))
            {
                using (var command = new SqlCommand("select * from editor.Infos"))
                {
                    command.Connection = conn;
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
                    {
                        return Connexion.DataExtensions.DataTable.FromSqlCommand(command)
                            .GetAwaiter().GetResult();
                    }
                }

            }
        }
    }
}
