using System;

namespace Test.ThreadTimer
{
    class Program
    {
        static void Main(string[] args)
        {
            var tool = new ToolTime();
            tool.Start();

            while(true)
            {
                var key = Console.ReadKey().Key.ToString();
                switch(key)
                {
                    case "s":
                    case "S":
                        tool.Start();
                        break;
                    case "p":
                    case "P":
                        tool.Pause();
                        break;
                }
            }
        }
    }
}
