//Program.cs
using HuffmanEncoding;
using InfoCompression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task1_ConsoleApp
{
    public struct CountOfChar
    {
        public char Char;
        public int Count;
        public double Probability;
        public double RangeLow;
        public double RangeHigh;

    }

    public class Program
    {

        public static string ToBinary(double num)
        {
            StringBuilder sb = new StringBuilder();
            double x = num % 1;

            if (num > 0)
                sb.Append(Convert.ToString((int)num, 2));

            if (x == 0)
                return sb.ToString();

            sb.Append('.');

            for (int i = 0; i < 20; i++)
            {
                x *= 2;
                sb.Append(Math.Truncate(x));
                x %= 1;
            }

            return sb.ToString();
        }

        public static void Main(string[] args)
        {
            HuffmanCode huffmanCode = new HuffmanCode();

            for (int i = 1; i <= 4; i++)
                huffmanCode.PrintHuffmanTable(i);

            huffmanCode.PrintCalcTable();

            double low = 0;
            double high = 1;
            double range;

            Console.WriteLine("Arithmetic code");
            Console.WriteLine("Enter the text");

            Dictionary<char, int> dict = new Dictionary<char, int>();
            string str = Console.ReadLine();

            foreach (char s in str)
            {
                if (dict.ContainsKey(s))
                {
                    dict[s]++;
                }
                else
                {
                    dict.Add(s, 1);
                }
            }

            dict = dict.OrderByDescending(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            KeyValuePair<char, int>[] chars = dict.ToArray();

            CountOfChar[] CharsNew = new CountOfChar[chars.Length];

            for (int i = 0; i < chars.Length; i++)
            {
                CharsNew[i].Count = chars[i].Value;
                CharsNew[i].Char = chars[i].Key;
                CharsNew[i].Probability = (double)chars[i].Value / str.Length;
                CharsNew[i].RangeLow = (i == 0) ? 0 : CharsNew[i - 1].RangeHigh;
                CharsNew[i].RangeHigh = (i == 0) ? CharsNew[i].Probability : CharsNew[i].RangeLow + CharsNew[i].Probability;
            }


            CountOfChar temp;
            temp.RangeHigh = 0;
            temp.RangeLow = 0;



            foreach (char c in str)
            {
                range = high - low;
                for (int i = 0; i < CharsNew.Length; i++)
                {
                    if (CharsNew[i].Char == c)
                    {
                        temp = CharsNew[i];
                        break;

                    }
                }
                high = low + range * temp.RangeHigh;
                low = low + range * temp.RangeLow;
            }
            Console.WriteLine("Boundaries: ");
            Console.Write(low);
            Console.Write(" - ");
            Console.WriteLine(high);
            Console.WriteLine("Binary code: ");
            Console.Write(ToBinary(low));
            Console.Write(" - ");
            Console.WriteLine(ToBinary(high));
        }


    }
}
