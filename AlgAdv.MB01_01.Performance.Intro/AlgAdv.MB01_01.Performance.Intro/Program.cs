﻿using System.Diagnostics;

namespace AlgAdv.MB01_01.Performance.Intro {
    public class Program {
        static void Main(string[] args) {
            var stopWatch = new Stopwatch();

            stopWatch.Start();
            Console.WriteLine("\nStart ReadAllLogs");
            var lineCount = ReadAllLogs();
            Console.WriteLine("Number of lines: " + lineCount);
            Console.WriteLine("Time elapsed: {0:0.0000}", stopWatch.ElapsedMilliseconds / 1000.0);


            stopWatch.Restart();
            Console.WriteLine("\nStart CountUniqueIPs");
            var ipCount = CountUniqueIPs();
            Console.WriteLine("Number of unique IPs: " + ipCount);
            Console.WriteLine("Time elapsed: {0:0.0000}", stopWatch.ElapsedMilliseconds / 1000.0);


            Console.ReadLine();
        }

        private static int ReadAllLogs() {
            var logReader = new LogReader();
            var linesSeen = 0;
            foreach (var line in logReader) {
                var ip = line.GetIP();
                linesSeen++;
            }
            return linesSeen;
        }

        private static int CountUniqueIPs() {
            var logReader = new LogReader();
            var ipsSeen = new List<string>();

            foreach (var logLine in logReader) {
                var ip = logLine.GetIP();
                if (!ipsSeen.Contains(ip))
                    ipsSeen.Add(ip);
            }
            return ipsSeen.Count;
        }
    }
}