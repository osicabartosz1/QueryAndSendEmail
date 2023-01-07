namespace QueryAndSendEmail
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args[0] == "--sendMail" && args.Length == 6)
                {
                    Mail.FromMail = args[1];
                    Mail.Password = args[2];
                    Mail.ToMail = args[3];
                    Query.ConnectionString = args[4];
                    Query.DataBaseName = args[5];
                    Mail.SendProgressMail2Step();
                    return;
                }
                if (args[0] == "--watchMail" && args.Length == 6)
                {
                    Mail.FromMail = args[1];
                    Mail.Password = args[2];
                    Mail.ToMail = args[3];
                    Query.ConnectionString = args[4];
                    Query.DataBaseName = args[5];
                    Mail.watchMail2Step();
                }
                else
                {
                    Console.WriteLine("wrong input");
                    Log.printHelp();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}