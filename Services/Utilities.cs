namespace Sales.Services
{
    internal class Utilities
    {
        public static int DisplayBasicOptions()
        {
            List<string> BasicOptions = ["Create", "View", "Update", "Delete"];
            for(int i = 0; i < 4; i++)
            {
                Console.WriteLine($"{i + 1}. {BasicOptions[i]}");
            }
            Console.WriteLine("0. Go back to main menu");
            return BasicOptions.Count;
        }
        public static void Pause(string? info = "Press any key to continue ...")
        {
            Console.WriteLine(info);
            Console.ReadKey();
        }

        public static void DisplayHeader(List<string> strs, ConsoleColor color = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = color;
            foreach (var item in strs)
            {
                Console.WriteLine(item);
            }
            Console.ResetColor();
        }
    }
}
