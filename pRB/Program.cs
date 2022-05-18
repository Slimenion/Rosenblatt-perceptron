using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace perseptronCSharp
{
    internal class Program
    {

        static List<double[,]> RecordingDataFromFile(string filePath, int quanityOutputsData)
        {
            int countLines = File.ReadAllLines(filePath).Length;
            StreamReader reader = new StreamReader(filePath);
            int countColums = reader.ReadLine().Split().Length;
            reader.Close();
            using (StreamReader reader1 = new StreamReader(filePath))
            {
                string? line;
                double[,] inputs = new double[countLines, countColums - quanityOutputsData];
                double[,] outputs = new double[countLines, quanityOutputsData];
                int i = 0;
                while ((line = reader1.ReadLine()) != null)
                {
                    string[] subline = line.Split();
                    for (int j = 0; j < subline.Length - quanityOutputsData; j++)
                    {
                        inputs[i, j] = Convert.ToDouble(subline[j]);
                    }
                    for (int j = subline.Length - quanityOutputsData; j < subline.Length; j++)
                    {
                        outputs[i, j - subline.Length + quanityOutputsData] = Convert.ToDouble(subline[j]);
                    }
                    i++;

                }
                List<double[,]> ans = new List<double[,]>();
                ans.Add(inputs);
                ans.Add(outputs);
                return ans;
            }
        }

        static void PrintMass(double[,] elems)
        {
            for (int i = 0; i < elems.GetLength(0); i++)
            {
                for (int j = 0; j < elems.GetLength(1); j++)
                {
                    Console.Write(elems[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        static double sign(double x, bool proisv = false)
        {
            if (proisv)
            {
                return sign(x) * (1 - sign(x));
            }
            return 1 / (1 + Math.Exp(-x));
        }

        static double vectorMultiplication(double[,] x, double[] y, int k)
        {
            double sum = 0;
            for (int i = 0; i < x.GetLength(1); i++)
            {
                sum += x[k, i] * y[i];
            }
            return sum;
        }

        static double vectorMultiplication(double[] x, double[] y)
        {
            double sum = 0;
            for (int i = 0; i < x.GetLength(1); i++)
            {
                sum += x[i] * y[i];
            }
            return sum;
        }

        static void InputValues(ref string nameOfFile, ref int outPutColums, ref int quanityEpochs, ref double speedEducation)
        {
            Console.WriteLine("Введите путь к файлу с обучающей выборкой");
            nameOfFile = Console.ReadLine();
            Console.WriteLine("Введите количество столбцов выходных данных");
            outPutColums = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите количество эпох");
            quanityEpochs = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Введите скорость обучения");
            speedEducation = Convert.ToDouble(Console.ReadLine());

        }

        static void Main(string[] args)
        {
            //объявление переменных для ввода данных
            string nameOfFile = @"..\trainingSample.txt";
            int outPutColumns = 1;
            int quanityEpochs = 10000;
            double speedEducation = 0.5;


            //ввод данных
            //InputValues(ref nameOfFile, ref outPutColumns, ref quanityEpochs, ref speedEducation);
            List<double[,]> OutInputs = RecordingDataFromFile(nameOfFile, outPutColumns);
            double[,] inputs = OutInputs[0];
            double[,] outputs = OutInputs[1];

            //вывод данных полученных из текстового файла
            PrintMass(inputs);
            PrintMass(outputs);

            //объявление и заполнение весов случайными данными
            double[] weights = new double[inputs.GetLength(0)];
            Random rand = new Random();
            //Console.WriteLine("Изначальные веса:");
            for (int j = 0; j < weights.Length; j++)
            {
                weights[j] = rand.NextDouble();
            }
            Console.WriteLine();

            //объявление переменных для начала цикла обучения
            double[] errorSign1 = new double[weights.Length];
            double[] deltaSign1 = new double[weights.Length];
            double[] sign1 = new double[weights.Length];

            //начало цикла обучения
            for (int i = 0; i < quanityEpochs; i++)
            {
                for (int k = 0; k < weights.Length; k++)
                {
                    sign1[k] = sign(vectorMultiplication(inputs, weights, k));
                }

                for (int k = 0; k < weights.Length; k++)
                {
                    errorSign1[k] = outputs[k, 0] - sign1[k];
                }
                for (int k = 0; k < weights.Length; k++)
                {
                    deltaSign1[k] = errorSign1[k] * sign(sign1[k]);
                }

                for (int k = 0; k < weights.Length; k++)
                {
                    weights[k] = weights[k] + vectorMultiplication(inputs, deltaSign1, k);
                }

            }

            //проверяем как обучилась сеть 
            Console.WriteLine("\n\nВывод результатов после обучения:");
            for (int j = 0; j < inputs.GetLength(0); j++)
            {
                Console.WriteLine(sign1[j]);
            }
        }
    }
}
