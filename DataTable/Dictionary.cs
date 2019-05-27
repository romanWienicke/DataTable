using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Connexion.DataExtensions
{
    public static class Dictionary
    {
        public static async Task<IDictionary<string, object>> FromSqlCommand(SqlCommand command)
        {


            try
            {
                if (command.Connection.State != System.Data.ConnectionState.Open)
                {
                    command.Connection.Open();
                }

                Dictionary<string, object> result = new Dictionary<string, object>();


                SqlDataReader reader = await command.ExecuteReaderAsync();
                bool dataIsPresent = reader.Read();

                if (dataIsPresent)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        result.Add(reader.GetName(i), reader[i]);
                    }
                }
                reader.Close();

                if (dataIsPresent) return result;
                return null;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (command.Connection.State != System.Data.ConnectionState.Closed)
                {
                    command.Connection.Close();
                }
            }
        }
    }
}
