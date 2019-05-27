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
    public class DataTable2
    {
        const string NotInitializedExceptionMessage = "Table was not initialized";

        private HashSet<ColumnData[]> _data;
        public HashSet<ColumnData[]> Data
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
            
            _data = new HashSet<ColumnData[]>();
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
                    var row = new ColumnData[columnCount];
                    for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                    {
                        row[columnIndex] = new ColumnData {
                            Column = Columns[columnIndex],
                            Data = reader.GetValue(columnIndex)
                        };
                    }
                    Data.Add(row);
                }
            }
        }

        public Column[] Columns
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

        private Column[] _columns;
        private int GetColumns(SqlDataReader reader)
        {
            var fieldCount = reader.FieldCount;

            _columns = new Column[fieldCount];

            for (int i = 0; i < fieldCount; i++)
            {
                _columns[i] = new Column {
                    Index = i,
                    Name = reader.GetName(i),
                    Type = reader.GetFieldType(i)
                };
            }


            return fieldCount;
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

    public struct Column
    {
        public int Index;
        public string Name;
        public Type Type;
    }

    public struct ColumnData
    {
        public Column Column;
        public object Data;   
    }
}
