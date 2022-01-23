﻿using System;
using System.Runtime.InteropServices;

namespace ConsoleRPG
{
    class Program
    {


        //For full RGB colors----------
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        //------------------------------
        static void Main(string[] args)
        {

            ActivateColors();
            Console.CursorVisible = false;

            Console.Write("\x1b[48;2;" + 0 + ";" + 0 + ";" + 0 + "m");

            //MapGenerator mapGen = new MapGenerator();
            //Console.Write(mapGen.CellsToString(mapGen.RecursiveBacktracker()));


            ConsoleRPG.MiniDC.PlayGame();
        }

        static void ActivateColors()
        {
            var handle = GetStdHandle(-11);

            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);
        }
    }

}
