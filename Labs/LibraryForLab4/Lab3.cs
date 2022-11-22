using System;
using System.IO;

namespace LibraryForLab4
{
    public class Lab3 : ILab
    {
        public string Input { get; set; }
        public string Output { get; set; }

        public void Run()
        {
            if (!File.Exists(Input))
            {
                Console.WriteLine($"Input file {Input} Not Found!");
                return;
            }

            string[] lines = File.ReadAllLines(Input);

            int N, M;
            int[,] matrix;


            ParseStrings(lines, out N, out M, out matrix);

            int result = ShortestWay(matrix, N, M);

            File.WriteAllText(Output, result.ToString());

            PrintResult();
        }

        private static bool CheckNM(int x) => !(1 <= x && x <= 30);
        private static int ConvertToInt32(string value)
        {
            bool parsed = Int32.TryParse(value, out int result);

            if (!parsed)
                throw new ArgumentException($"The value {value} was not parsed to int");

            return result;
        }
        private static void ParseStrings(string[] lines, out int N, out int M, out int[,] matrix)
        {
            string[] NM = lines[0].Split();

            if (NM.Length != 2)
                throw new ArgumentException("First line must have two numbers!");

            N = ConvertToInt32(NM[0]);
            M = ConvertToInt32(NM[1]);

            if (CheckNM(N) || CheckNM(M))
                throw new ArgumentException("Input values do not match criteria 1 <= M, N <= 30");

            if (lines.Length - 1 != N)
                throw new ArgumentException("Must be N lines");

            matrix = new int[N, M];

            for (int i = 1; i < lines.Length; i++)
            {
                string[] numbers = lines[i].Split();

                if (numbers.Length != M)
                    throw new ArgumentException("Must be M numbers in all lines");

                for (int j = 0; j < M; j++)
                {
                    int node = ConvertToInt32(numbers[j]);
                    if (node < 0 || node > 100)
                        throw new ArgumentException("Node do not match criteria 0 <= node <= 100");
                    matrix[i - 1, j] = ConvertToInt32(numbers[j]);
                }
            }
        }
        private static void InitWay(int[,] way)
        {
            for (int i = 0; i < way.GetUpperBound(0) + 1; i++)
            {
                for (int j = 0; j < way.GetUpperBound(1) + 1; j++)
                {
                    way[i, j] = 1000000;
                }
            }
        }
        private static int ShortestWay(int[,] a, int n, int m)
        {
            int[,] way = new int[n, m];
            InitWay(way);

            way[0, 0] = a[0, 0];

            void func(int y, int x)
            {
                if (y == n - 1 && x == m - 1)
                    return;

                if (y != n - 1 && way[y, x] + a[y + 1, x] < way[y + 1, x])
                {
                    way[y + 1, x] = way[y, x] + a[y + 1, x];
                    func(y + 1, x);
                }
                if (y != 0 && way[y, x] + a[y - 1, x] < way[y - 1, x])
                {
                    way[y - 1, x] = way[y, x] + a[y - 1, x];
                    func(y - 1, x);
                }
                if (x != m - 1 && way[y, x] + a[y, x + 1] < way[y, x + 1])
                {
                    way[y, x + 1] = way[y, x] + a[y, x + 1];
                    func(y, x + 1);
                }
                if (x != 0 && way[y, x] + a[y, x - 1] < way[y, x - 1])
                {
                    way[y, x - 1] = way[y, x] + a[y, x - 1];
                    func(y, x - 1);
                }
            }

            func(0, 0);


            return way[n - 1, m - 1];
        }
        private void PrintResult()
        {
            Console.WriteLine(new String('-', 20));
            Console.WriteLine(File.ReadAllText(Output));
            Console.WriteLine("Input: " + Input);
            Console.WriteLine("Output: " + Output);
            Console.WriteLine(new String('-', 20));
        }

    }
}
