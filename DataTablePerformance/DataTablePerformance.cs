using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SYSDATA = System.Data;
using System.Data.SqlClient;
using DataTableTests;
using System.Collections;

namespace Connexion.DataExtensions.Performance
{
    public class DataTablePerformance
    {
        const string sqlSelect = @"SELECT        c.id, c.contractId, c.name, c.groupName, c.description, c.TypeId, c.wageProgressID, c.digits, 
			c.province, c.contractversion, c._validFor, c._weekLength, c._showInfo, c._coverage, c.version, c.isHidden, c.longName, 
                         c.hasAnniversaryBonus, p.id AS Expr1, p.contractId AS Expr2, p.headline, p.text, p.areaID, p.version AS Expr3, p.isHidden AS Expr4
FROM            kv.Contracts AS c INNER JOIN
                         kv.Paragraphs AS p ON c.contractId = p.contractId";

        [Benchmark(Baseline = true)]
        public SYSDATA.DataTable SystemDataTableTest()
        {
            var context = new TestDataContext();

            using (var conn = new SqlConnection(context.ConnectionString))
            {
                using (var command = new SqlCommand(sqlSelect))
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
                using (var command = new SqlCommand(sqlSelect))
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

        //[Benchmark()]
        //public Connexion.DataExtensions.DataTable2 ConnexionDataTable2Test()
        //{
        //    var context = new TestDataContext();

        //    using (var conn = new SqlConnection(context.ConnectionString))
        //    {
        //        using (var command = new SqlCommand("select * from editor.Infos"))
        //        {
        //            command.Connection = conn;
        //            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command))
        //            {
        //                return Connexion.DataExtensions.DataTable2.FromSqlCommand(command)
        //                    .GetAwaiter().GetResult();
        //            }
        //        }

        //    }

        //}

        [Benchmark()]
        public IDictionary<string, object> TotallyDifferentTest()
        {
            var context = new TestDataContext();

            using (var conn = new SqlConnection(context.ConnectionString))
            {
                using (var command = new SqlCommand(sqlSelect))
                {
                    command.Connection = conn;

                    return Dictionary.FromSqlCommand(command).GetAwaiter().GetResult();

                }

            }
        }
    }
}
