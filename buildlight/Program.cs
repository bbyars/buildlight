using System;
using System.Reflection;

namespace buildlight
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!Valid(args))
            {
                PrintUsage();
                return;
            }

            SetLed(args[0], args[1]);
        }

        private static bool Valid(string[] args)
        {
            return args.Length == 2
                   && GetColor(args[0]) != null
                   && GetAction(GetColor(args[0]), args[1]) != null;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: buildlight color action");
            Console.WriteLine("\tValid colors are Green, Red, Orange, Yellow, Gold, DarkOrange, AllColors");
            Console.WriteLine("\tValid actions are Off, On, Flash");
        }

        private static IColor GetColor(string colorName)
        {
            return typeof(Color).GetField(colorName, BindingFlags.Public | BindingFlags.Static).GetValue(null) as IColor;
        }

        private static MethodInfo GetAction(IColor color, string actionName)
        {
            return color.GetType().GetMethod(actionName, BindingFlags.Public | BindingFlags.Instance);
        }

        private static void SetLed(string colorName, string actionName)
        {
            var color = GetColor(colorName);
            var method = GetAction(color, actionName);
            method.Invoke(color, new object[0]);
        }
    }
}
