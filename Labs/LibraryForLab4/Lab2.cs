using System;
using System.IO;
using System.Collections.Generic;

namespace LibraryForLab4
{
    public class Lab2: ILab
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

            var inputData = ConvertToInt32(File.ReadAllText(Input));

            if (CheckInputData(inputData))
                throw new ArgumentException("Input values do not match criteria 0 < N <= 70");

            decimal Count = DirectOrder(inputData);

            File.WriteAllText(Output, Count.ToString());

            PrintResult();
        }

        private static bool CheckInputData(int n) => !(0 < n && n <= 70);
        private static int ConvertToInt32(string value)
        {
            bool parsed = Int32.TryParse(value, out int result);

            if (!parsed)
                throw new ArgumentException($"The value {value} was not parsed to int");

            return result;
        }
        private static decimal DirectOrder(int N)
        {
            decimal[] initialValues = { 1, 2, 4 };
            var result = new List<decimal>(initialValues);

            for (int i = 3; i < N; i++)
                result.Add(result[i - 3] + result[i - 2] + result[i - 1]);

            return result[N - 1];
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
