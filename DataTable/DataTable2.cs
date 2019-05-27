using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Connexion.DataExtensions
{
    public class DataTable2 : MarshalByValueComponent
    {
        const string NotInitializedExceptionMessage = "Table was not initialized";

        private HashSet<object[]> _data;
        public HashSet<object[]> Data
        {
            get
            {
                if (_data == null)
                {
                    throw new Exception("NotInitializedExceptionMessage");
                }
                return _data;
            }
        }

        public DataTable2()
        {
            
            _data = new HashSet<object[]>();
        }

        public static async Task<DataTable2> FromSqlCommand(SqlCommand command)
        {
            TryOpenConnection(command);

            try
            {
                var dataTable = new DataTable2();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    dataTable.GetData(reader);
                }
                return dataTable;
               
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed)
                    command.Connection.Close();
            }

        }

        private void GetData(SqlDataReader reader)
        {
            var columnCount = GetColumns(reader);


            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var row = new object[columnCount];
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        row[columnIndex] = reader.GetValue(columnIndex);
                        
                    }
                    Data.Add(row);
                }
            }
        }

        public string[] Columns
        {
            get
            {
                if (_columns == null)
                {
                    throw new Exception(NotInitializedExceptionMessage);
                }
                return _columns;
            }
        }

        private string[] _columns;
        private int GetColumns(SqlDataReader reader)
        {
            _columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray();

            return _columns.Length;
        }

        private static void TryOpenConnection(SqlCommand command)
        {
            if (command.Connection == null)
                throw new ArgumentException($"Command Connection was not set");

            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
        }
    }


}
