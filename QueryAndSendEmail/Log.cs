namespace QueryAndSendEmail
{
    internal static class Log
    {
        public static void printHelp() 
        {
            Console.WriteLine("To send email I need: \r\n--sendMail FromMail Password ToMail ConnectionString DataBaseName");
            Console.WriteLine("To send email I need: \r\n--watchMail FromMail Password ToMail ConnectionString DataBaseName");
        }
    }
}