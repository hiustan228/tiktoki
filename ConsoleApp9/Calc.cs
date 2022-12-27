//Calc.cs
using System;
using System.Text;
using System.Collections.Generic;
using HuffmanEncoding;

namespace InfoCompression
{
    public static class Calc
    {
        public static double Aggregate(double x, double y)
        {
            return x * y;
        }

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

        public static string GetHuffmanString(double[] chars)
        {
            int n = -1;
            StringBuilder sb = new StringBuilder();

            n = (chars.Length == 2) ? 10 :
                (chars.Length == 4) ? 100 :
                (chars.Length == 8) ? 1000 : 10000;

            if (n == -1) throw new Exception();

            for (int i = 0; i < chars.Length; i++)
                sb.Append(new String(Convert.ToChar(i + 97), (int)(chars[i] * n)));

            return sb.ToString();
        }

        public static List<string> GetHuffmanBits(string str)
        {
            string bits;
            List<string> bitlist = new List<string>();

            Huffman<char> huffman = new Huffman<char>(str);
            List<int> encoding = huffman.Encode(str);
            List<char> decoding = huffman.Decode(encoding);
            string result = new string(decoding.ToArray());

            if (result != str)
                throw new Exception("Encoding/Decoding failed!");

            HashSet<char> chars = new HashSet<char>(str);

            foreach (char c in chars)
            {
                bits = "";

                encoding = huffman.Encode(c);

                foreach (int bit in encoding)
                    bits += bit.ToString();

                bitlist.Add(bits);
            }

            return bitlist;
        }

        public static double[] AvgCodewordLength(double[] chars, int[] charnum)
        {
            double[] n = new double[chars.Length];

            for (int i = 0; i < chars.Length; i++)
                n[i] = chars[i] * charnum[i];

            return n;
        }

        public static double AvgLengthPerChar(double avgcwlen, int n)
        {
            return avgcwlen / n;
        }

        public static double Entropy(double[] chars)
        {
            double sum = 0;

            foreach (double p in chars)
                sum += p * Math.Log2(p);

            return -sum;
        }

        public static double EntropyMax(double m)
        {
            return Math.Log2(m);
        }

        public static double SrcRedundancy(double h, double hmax)
        {
            return 1.0 - h / hmax;
        }

        public static double CodeRedundancy(double pmax)
        {
            double result;

            if (pmax < 0.5)
            {
                result = 1.0 - Math.Log2(Math.E) + Math.Log2(Math.Log2(Math.E));
                result += pmax;
            }
            else
            {
                result = 2.0;
                result -= -pmax * Math.Log2(pmax) - (1.0 - pmax) * Math.Log2(1.0 - pmax);
                result -= pmax;
            }

            return result;
        }
    }
}

