using McMaster.Extensions.CommandLineUtils;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

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

        private static int ConvertToInt32(string value)
        {
            bool parsed = Int32.TryParse(value, out int result);

            if (!parsed)
                throw new ArgumentException($"The value {value} was not parsed to int");

            return result;
        }
        public static void ParseStrings(string[] lines, out int N, out int K, out int[,] matrix)
        {
            string[] NK = lines[0].Split();

            N = ConvertToInt32(NK[0]);
            K = ConvertToInt32(NK[1]);

            if (!(1 < N && N <= 15))
                throw new ArgumentException("Input values do not match criteria 1 < N <= 15");

            if (!(0 < K && K <= 30))
                throw new ArgumentException("Input values do not match criteria 0 < K <= 30");

            matrix = new int[N + 2, N + 2];

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    matrix[i, j] = 1;
            }

            for (int i = 1; i < lines.Length; i++)
            {
                for (int j = 0; j < N; j++)
                    matrix[i, j + 1] = ConvertToInt32(Convert.ToString(lines[i][j]));
            }
        }

        private void OnExecute()
        {
            string[] lines = File.ReadAllLines(InputFile);
            int sizeIJ, len;
            int[,] a;

            ParseStrings(lines, out sizeIJ, out len, out a);

            int[,,] count = new int[1 + len, sizeIJ + 2, sizeIJ + 2];

            count[0, 1, 1] = 1;

            for (int step = 1; step <= len; step++)
            {
                for (int i = 1; i <= sizeIJ; i++)
                {
                    for (int j = 1; j <= sizeIJ; j++)
                    {
                        if (a[i, j] == 0)
                            count[step, i, j] = count[step - 1, i - 1, j] + count[step - 1, i + 1, j] + count[step - 1, i, j + 1] + count[step - 1, i, j - 1];
                    }
                }
            }

            File.WriteAllText(OutputFile, count[len, sizeIJ, sizeIJ].ToString());
        }
    }
}
