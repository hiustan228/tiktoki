//HuffmanCode.cs
using System;
using System.Collections.Generic;
using System.Linq;
using HuffmanEncoding;

namespace InfoCompression
{
    public class HuffmanCode
    {
        private double[] arrX;
        private double[] arrY;
        private double[] arrZ;
        private double[] arrQ;
        private int[][] arrayContainer;

        public HuffmanCode()
        {
            int counter;

            arrX = new double[2];
            arrY = new double[4];
            arrZ = new double[8];
            arrQ = new double[16];

            arrayContainer = new int[4][]
            {
                new int[2],
                new int[4],
                new int[8],
                new int[16]
            };

            arrX[0] = 0.9;
            arrX[1] = 0.1;

            arrY[0] = Math.Round(Calc.Aggregate(arrX[0], arrX[0]), 2);
            arrY[1] = Math.Round(Calc.Aggregate(arrX[0], arrX[1]), 2);
            arrY[2] = Math.Round(Calc.Aggregate(arrX[1], arrX[0]), 2);
            arrY[3] = Math.Round(Calc.Aggregate(arrX[1], arrX[1]), 2);



            counter = 0;
            for (int i = 0; i < arrX.Length; i++)
            {
                for (int j = 0; j < arrY.Length; j++)
                {
                    arrZ[counter] = Math.Round(Calc.Aggregate(arrX[i], arrY[j]), 3);
                    counter++;
                }
            }

            counter = 0;
            for (int i = 0; i < arrX.Length; i++)
            {
                for (int j = 0; j < arrZ.Length; j++)
                {
                    arrQ[counter] = Math.Round(Calc.Aggregate(arrX[i], arrZ[j]), 4);
                    counter++;
                }
            }
        }

        public void PrintHuffmanTable(int n)
        {
            string InString, bits;
            string[] coll;
            double[] arr;



            if (n == 1)
            {
                arr = arrX;
                coll = new string[2] { "x1", "x2" };
            }
            else if (n == 2)
            {
                arr = arrY;
                coll = new string[4]
                {
                    "x1x1", "x1x2",
                    "x2x1", "x2x2"
                };
            }
            else if (n == 3)
            {
                arr = arrZ;
                coll = new string[8]
                {
                    "x1x1x1", "x1x1x2", "x1x2x1", "x1x2x2",
                    "x2x1x1", "x2x1x2", "x2x2x1", "x2x2x2"
                };
            }
            else if (n == 4)
            {
                arr = arrQ;
                coll = new string[16]
                {
                    "x1x1x1x1", "x1x1x1x2", "x1x1x2x1", "x1x1x2x2",
                    "x1x2x1x1", "x1x2x1x2", "x1x2x2x1", "x1x2x2x2",
                    "x2x1x1x1", "x2x1x1x2", "x2x1x2x1", "x2x1x2x2",
                    "x2x2x1x1", "x2x2x1x2", "x2x2x2x1", "x2x2x2x2"
                };
            }
            else
                throw new Exception();

            InString = Calc.GetHuffmanString(arr);

            var huffman = new Huffman<char>(InString);
            List<int> encoding = huffman.Encode(InString);
            List<char> decoding = huffman.Decode(encoding);
            string outStr = new string(decoding.ToArray());

            if (outStr != InString)
                throw new Exception("Encoding/Decoding failed!");

            var charhash = new HashSet<char>(InString);
            char[] chars = new char[charhash.Count];
            charhash.CopyTo(chars);



            Console.WriteLine("==================================================================\n" +
                              " Source     Codeword     Probability  Huffman code  Character     \n" +
                              " alphabet   Designations p(Si)                      Count         \n" +
                              "==================================================================");

            for (int i = 0; i < arr.Length; i++)
            {
                encoding = huffman.Encode(chars[i]);

                bits = "";


                foreach (int bit in encoding)
                    bits += bit.ToString();


                Console.WriteLine("{0}\t{1}{2}{3}\t {4}\t\t{5}\t{6}", coll[i],
                    (n != 4) ? "\t" : "",
                    (n == 1) ? "X" : (n == 2) ? "Y" : (n == 3) ? "Z" : "Q",
                    i + 1,
                    (n == 1) ? arrX[i] : (n == 2) ? arrY[i] : (n == 3) ? arrZ[i] : arrQ[i],
                    bits + new string(' ', 12 - bits.Length),
                    bits.Length.ToString() + new string(' ', 14 - bits.Length.ToString().Length));
                arrayContainer[n - 1][i] = bits.Length;


                if (i == arr.Length - 1) Console.WriteLine();
            }
        }





        public void Print()
        {
            for (int i = 0; i < arrX.Length; i++)
                Console.WriteLine("X{0} = {1}", i + 1, arrX[i]);
            Console.WriteLine();
            for (int i = 0; i < arrY.Length; i++)
                Console.WriteLine("Y{0} = {1}", i + 1, arrY[i]);
            Console.WriteLine();
            for (int i = 0; i < arrZ.Length; i++)
                Console.WriteLine("Z{0} = {1}", i + 1, arrZ[i]);
            Console.WriteLine();
            for (int i = 0; i < arrQ.Length; i++)
                Console.WriteLine("Q{0} = {1}", i + 1, arrQ[i]);
        }

        public void PrintCalcTable()
        {

            Console.WriteLine("========================================================================================\n" +
                                 "l       p(Xк)   H(S)    H1(S)   nminc   Xи      n       nc      nminc / nc      Xк     \n" +
                                 "        \n" +
                                 "========================================================================================");
            double h, hmax, avgcwlen, avglenchar, avglenmin;



            h = Calc.Entropy(arrX);
            hmax = Calc.EntropyMax(2);
            avgcwlen = Calc.AvgCodewordLength(arrX, arrayContainer[0]).Sum();
            avglenmin = Calc.AvgCodewordLength(arrX, arrayContainer[0]).Min();
            avglenchar = Calc.AvgLengthPerChar(avgcwlen, 2);


            Console.WriteLine("1\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\t{8}",
    arrX.Max(),
    Math.Round(h, 4),
    Math.Round(hmax, 4),
    avglenmin,
    Math.Round(Calc.SrcRedundancy(h, hmax), 5),
    Math.Round(avgcwlen, 3),
    Math.Round(avglenchar, 3),
    Math.Round(avglenmin / avglenchar, 4),
    Math.Round(Calc.CodeRedundancy(arrX.Max()), 5));

            //
            h = Calc.Entropy(arrY);
            hmax = Calc.EntropyMax(4);
            avgcwlen = Calc.AvgCodewordLength(arrY, arrayContainer[1]).Sum();
            avglenmin = Calc.AvgCodewordLength(arrY, arrayContainer[1]).Min();
            avglenchar = Calc.AvgLengthPerChar(avgcwlen, 4);


            Console.WriteLine("2\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\t{8}",
arrY.Max(),
Math.Round(h, 4),
Math.Round(hmax, 4),
avglenmin,
Math.Round(Calc.SrcRedundancy(h, hmax), 5),
Math.Round(avgcwlen, 3),
Math.Round(avglenchar, 3),
Math.Round(avglenmin / avglenchar, 4),
Math.Round(Calc.CodeRedundancy(arrY.Max()), 5));

            h = Calc.Entropy(arrZ);
            hmax = Calc.EntropyMax(8);
            avgcwlen = Calc.AvgCodewordLength(arrZ, arrayContainer[2]).Sum();
            avglenmin = Calc.AvgCodewordLength(arrZ, arrayContainer[2]).Min();
            avglenchar = Calc.AvgLengthPerChar(avgcwlen, 8);

            Console.WriteLine("3\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\t{8}",
arrZ.Max(),
Math.Round(h, 4),
Math.Round(hmax, 4),
avglenmin,
Math.Round(Calc.SrcRedundancy(h, hmax), 5),
Math.Round(avgcwlen, 3),
Math.Round(avglenchar, 3),
Math.Round(avglenmin / avglenchar, 4),
Math.Round(Calc.CodeRedundancy(arrZ.Max()), 5));

            h = Calc.Entropy(arrQ);
            hmax = Calc.EntropyMax(16);
            avgcwlen = Calc.AvgCodewordLength(arrQ, arrayContainer[3]).Sum();
            avglenmin = Calc.AvgCodewordLength(arrQ, arrayContainer[3]).Min();
            avglenchar = Calc.AvgLengthPerChar(avgcwlen, 16);

            Console.WriteLine("4\t{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t\t{8}",
arrQ.Max(),
Math.Round(h, 4),
Math.Round(hmax, 4),
avglenmin,
Math.Round(Calc.SrcRedundancy(h, hmax), 5),
Math.Round(avgcwlen, 3),
Math.Round(avglenchar, 3),
Math.Round(avglenmin / avglenchar, 4),
Math.Round(Calc.CodeRedundancy(arrQ.Max()), 5));

        }
    }
}