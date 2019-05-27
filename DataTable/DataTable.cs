using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Connexion.DataExtensions
{
    public class DataTable
    {
        const string NotInitializedExceptionMessage = "Table was not initialized";

        private object[][] _data;
        public object[][] Data
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

        public DataTable()
        {
            _data = new object[100][];
        }

        public static async Task<DataTable> FromSqlCommand(SqlCommand command)
        {
            TryOpenConnection(command);

            try
            {
                DataTable dataTable = new DataTable();
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


            int _rowCount = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    _data[_rowCount] = new object[columnCount];
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        
                        _data[_rowCount][columnIndex] = reader.GetValue(columnIndex);
                        
                    }
                    _rowCount++;
                    if (_rowCount % 100 == 0)
                    {
                        Array.Resize(ref _data, _data.Length + 100);
                    }

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
