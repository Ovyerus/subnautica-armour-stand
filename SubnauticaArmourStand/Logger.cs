using System;

namespace SubnauticaArmourStand
{
    internal static class Logger
    {
        internal static string Prefix = "SubnauticaArmourStand";

        internal static void Log(string str)
        {
            Console.WriteLine($"[{Prefix}] {str}");
        }

        internal static void Log(string str, params object[] args)
        {
            if (args != null && args.Length > 0) str = string.Format(str, args);
            Console.WriteLine($"[{Prefix}] {str}");
        }
    }
}
