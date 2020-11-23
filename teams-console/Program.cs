using System;

namespace teams_console
{
    public class Program
    {
        static void Main()
        {
            try
            {
                var app = new Application();
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error.");
                Console.WriteLine(e.Message);
            }
        }
    }
}
