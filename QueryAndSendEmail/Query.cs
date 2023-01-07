using ConsoleTables;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;

namespace QueryAndSendEmail
{
    internal static class Query
    {

        public static string ConnectionString = "";
        public static string DataBaseName = "";

        public static string GetProgressTable() 
        {
            ConsoleTable table = Query.CustomQuery("Use " + Query.DataBaseName + "; Select count(*) as Count, Status from StaticBox group by Status");
            return table.ToString();
        }

        public static ConsoleTable CustomQuery(string Query)
        {
            MySqlConnection MyConnection = new MySqlConnection(ConnectionString);
            MySqlCommand MyCommand = new MySqlCommand(Query, MyConnection);
            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    List<List<string>> output = new List<List<string>>();
                    MyConnection.Open();
                    MySqlDataReader reader = MyCommand.ExecuteReader();
                    ConsoleTable table = new ConsoleTable(Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToArray());
                    while (reader.Read())
                    {
                        table.AddRow(Enumerable.Range(0, reader.FieldCount).Select(reader.GetString).ToArray());
                    }
                    MyConnection.Close();
                    return table;
                }
                catch (Exception ex)
                {
                    if (MyConnection.State == ConnectionState.Open) MyConnection.Close();
                    Console.WriteLine("MyConnection.State = " + MyConnection.State);
                    Console.WriteLine(ex.ToString());
                    Thread.Sleep(5000 * i);
                }
            }
            throw new Exception("Unable to reach Database. Final Exception.");
        }
    }
}