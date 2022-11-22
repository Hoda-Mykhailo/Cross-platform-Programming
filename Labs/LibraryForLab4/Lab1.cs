using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace LibraryForLab4
{
    public class Lab1 : ILab
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

            var inputData = File.ReadAllText(Input);

            if (inputData[^1] == '\n')
                inputData = inputData.Remove(inputData.Length - 1);

            string[] strNumbers = inputData.Split();
            
            Console.WriteLine();

            if (strNumbers.Length != 3)
                throw new ArgumentException("The number of arguments must be 3");

            string strA = strNumbers[0];
            string strB = strNumbers[1];

            int a = ConvertToInt32(strA);
            int b = ConvertToInt32(strB);
            int c = ConvertToInt32(strNumbers[2]);

            if (CheckInputData(a) || CheckInputData(b) || CheckInputData(c))
                throw new ArgumentException("Input values do not match criteria 0 <= a, b, c < 10^9");

            List<string> perA = new Permutations(strA).GetPermutationsSortList(false);
            List<string> perB = new Permutations(strB).GetPermutationsSortList(false);

            Dictionary<int, int> result = new();

            foreach (string a1 in perA)
            {
                foreach (string b1 in perB)
                {
                    int a2 = ConvertToInt32(a1);
                    int b2 = ConvertToInt32(b1);

                    if (a2 + b2 == c)
                        result.Add(a2, b2);
                }
            }

            if (result.Count > 0)
            {
                int minA = result.Keys.Min();

                string outputData = "YES\n" + $"{minA} {result[minA]}";
                File.WriteAllText(Output, outputData);
            }
            else
            {
                File.WriteAllText(Output, "NO");
            }

            PrintResult();
        }

        private static bool CheckInputData(int n) => !(0 <= n && n < Math.Pow(10, 9));
        private static int ConvertToInt32(string value)
        {
            bool parsed = Int32.TryParse(value, out int result);

            if (!parsed)
                throw new ArgumentException($"The value {value} was not parsed to int");

            return result;
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
