using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_Lab1
{
	internal static class ConsoleInput
	{
		private static bool goodLastRead = false;
		public static bool LastReadWasGood
		{
			get
			{
				return goodLastRead;
			}
		}

		public static string ReadToWhiteSpace(bool skipLeadingWhiteSpace)
		{
			string input = "";

			char nextChar;
			while (char.IsWhiteSpace(nextChar = (char)System.Console.Read()))
			{
				if (!skipLeadingWhiteSpace)
					input += nextChar;
			}
			input += nextChar;
			while (!char.IsWhiteSpace(nextChar = (char)System.Console.Read()))
			{
				input += nextChar;
			}

			goodLastRead = input.Length > 0;
			return input;
		}

		public static string ScanfRead(string unwantedSequence = null, int maxFieldLength = -1)
		{
			string input = "";

			char nextChar;
			if (unwantedSequence != null)
			{
				nextChar = '\0';
				for (int charIndex = 0; charIndex < unwantedSequence.Length; charIndex++)
				{
					if (char.IsWhiteSpace(unwantedSequence[charIndex]))
					{
						while (char.IsWhiteSpace(nextChar = (char)System.Console.Read()))
						{
						}
					}
					else
					{
						nextChar = (char)System.Console.Read();
						if (nextChar != unwantedSequence[charIndex])
							return null;
					}
				}

				input = nextChar.ToString();
				if (maxFieldLength == 1)
					return input;
			}

			while (!char.IsWhiteSpace(nextChar = (char)System.Console.Read()))
			{
				input += nextChar;
				if (maxFieldLength == input.Length)
					return input;
			}

			return input;
		}
	}

	class Program
    {
        static void Main(string[] args)
        {
			if (!File.Exists("INPUT.txt"))
			{
				File.Create("INPUT.txt");
				Console.WriteLine("Файл створено, заповнiть його даними!!");
			}
			StreamReader Read = new StreamReader("INPUT.txt");
			string n = Read.ReadLine();
			if (n.ToCharArray().Count() > 15)
			{
				Console.WriteLine("Error: You enter more 15 symbols");
				Environment.Exit(0);
			}
			int c = (int)factorial(n.Length);
            int sum = 0;

            for (int i = 0; i < n.Length - 1; ++i)
            {
                for (int j = 1; j < n.Length; ++j)
                {
                    if (n[i] == n[j])
                    {
                        sum++;
                        c /= (int)factorial(sum);
                    }
                }
                sum = 0;
            }
			StreamWriter Output = new StreamWriter("OUTPUT.txt", false);
			Output.WriteLine(c);
			Output.Close();
        }

        public static long factorial(int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else
            {
                return n * factorial(n - 1);
            }
        }

    }
}
