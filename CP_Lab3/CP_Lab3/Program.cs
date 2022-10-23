using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;

namespace lab3
{
    public class Program
    {

        [Option(ShortName = "i")]
        public string InputFile { get; }

        [Option(ShortName = "o")]
        public string OutputFile { get; }

        static void Main(string[] args)
            => CommandLineApplication.Execute<Program>(args);

        static int c = 0;

        static int[,] m;
        static int[] used;

        static void dfs(int v)
        {
            used[v] = 1; c++;
            for (int i = 0; i < m.GetLength(0); i++)
                if (m[v, i] == 1 && used[i] == 0) dfs(i);
        }

        private static bool CheckN(int n) => !(1 <= n && n <= 100);
        private static int ConvertToInt32(string value)
        {
            bool parsed = Int32.TryParse(value, out int result);

            if (!parsed)
                throw new ArgumentException($"The value {value} was not parsed to int");

            return result;
        }
        public static void ParseStrings(string[] lines, out int N, out int[,] matrix, out int edges)
        {
            N = ConvertToInt32(lines[0]);

            if (CheckN(N))
                throw new ArgumentException("Input values do not match criteria 1 <= N <= 100");

            if (lines.Length - 1 != N)
                throw new ArgumentException("Must be N lines");

            matrix = new int[N, N];
            edges = 0;

            for (int i = 1; i < lines.Length; i++)
            {
                string[] numbers = lines[i].Split();

                for (int j = 0; j < N; j++)
                {
                    matrix[i - 1, j] = ConvertToInt32(numbers[j]);
                    edges += matrix[i - 1, j];
                }
            }
        }

        private void OnExecute()
        {
            string[] lines = File.ReadAllLines(InputFile);
            int N;
            int Edges;


            ParseStrings(lines, out N, out m, out Edges);

            used = new int[N];

            dfs(0);
            Edges /= 2;

            if ((Edges == N - 1) && (c == N))
                File.WriteAllText(OutputFile, "YES");
            else
                File.WriteAllText(OutputFile, "NO");

        }
    }
}
