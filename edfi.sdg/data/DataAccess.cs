using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EdFi.SampleDataGenerator.Data
{
    public class DataAccess
    {
        public string GetNextValue(string statTableName, IEnumerable<string> attributes)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;

            using (var connection = new SqlConnection(connectionString))
            {

                try
                {
                    var attrs = DataTableHelper.ToDataTable(attributes);

                    var cmd = new SqlCommand
                    {
                        Connection = connection,
                        CommandType = CommandType.StoredProcedure,
                        CommandText = "GetNextValue"
                    };
                    cmd.Parameters.AddWithValue("@StatTableName", statTableName);
                    cmd.Parameters.AddWithValue("@AttributeFilters", attrs).SqlDbType = SqlDbType.Structured;

                    connection.Open();
                    var dataReader = cmd.ExecuteReader();
                    if (dataReader.Read())
                    {
                        return (string) dataReader[0];
                    }
                }
                finally
                {
                    connection.Close();
                }

                return null;
            }
        }
    }

    public static class DataTableHelper
    {
        public static DataTable ToDataTable(IEnumerable<string> items)
        {
            var dataTable = new DataTable("Attributes");

            dataTable.Columns.Add("Attribute");

            foreach (var item in items)
            {
                dataTable.Rows.Add(item);
            }

            return dataTable;
        }
    }
}
