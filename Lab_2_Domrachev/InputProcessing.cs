/// <summary> 
/// Модуль для ввода данных.
/// Здесь происходит ввод данных с консоли, случайный ввод или же получение данных из файла.
/// <summary>
using System.Globalization;

namespace Lab_2_Domrachev
{
    static public class InputProcessing
    {
        /// <summary> 
        /// Варианты осуществления ввода.
        /// <summary>
        enum InputType
        {
            Keyboard,
            Random,
            File
        }
        /// <summary> 
        /// Получение ввода пользователя.
        /// <summary>
        static public void GetData(out List<decimal> array)
        {
            string newLine = Environment.NewLine;
            array = new List<decimal>();
            Console.WriteLine("Выберите способ ввода исходных данных." + newLine
                + "Ввод через клавиатуру - 0;" + newLine
                + "Рандомный ввод - 1;" + newLine
                 + "Ввод из файла - 2.");
            int inputWay;
            bool correctInput = false;
            while (!correctInput)
            {
                while (!int.TryParse(Console.ReadLine(), out inputWay))
                {
                    Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                }
                correctInput = true;
                switch ((InputType)inputWay)
                {
                    case InputType.Keyboard:
                        {
                            array = GetDataFromKeyboard();
                            break;
                        }
                    case InputType.Random:
                        {
                            array = GetRandomData();
                            break;
                        }
                    case InputType.File:
                        {
                            array = GetDataFromFile();
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                            correctInput = false;
                            break;
                        }
                }
            }
        }
         /// <summary> 
         /// Получение данных с клавиатуры.
         /// <summary>
        private static List<decimal> GetDataFromKeyboard()
        {
            List<decimal> array = new();
            
            bool corrrectInput = false;
            List<string> input = new();
            Console.WriteLine("Введите список элементов массива через пробел.");
            Console.WriteLine("В массиве должно быть не менее трех элементов, иначе решение задачи будет бессмысленно.");
            do
            {
                input!.AddRange(Console.ReadLine()!.Replace(',', '.').Split(' '));
                if(InputIsCorrect(input, array))
                {
                    corrrectInput = true;
                }
                else
                {
                    Console.WriteLine("Ввод не верен");
                    input = new();
                }
            } while (!corrrectInput);
            return array;
        }
        /// <summary> 
        /// Проверка полученных от пользователя данных на соответствие требованиям задачи.
        /// <summary>
        private static bool InputIsCorrect(List<string> input, List<decimal> array)
        {
            NumberFormatInfo numberFormatInfo = new()
            {
                NumberDecimalSeparator = ".",
            };
            const int MINIMAL_ARRAY_SIZE = 3;
            if (input.Count < MINIMAL_ARRAY_SIZE)
            {
                return false;
            }

            foreach (var item in input)
            {
                if (decimal.TryParse(item, NumberStyles.Float, numberFormatInfo, out decimal value))
                {
                    array.Add(value);
                }
                else
                {
                    array = new();
                    return false;
                }
            }
            return true;
        }

        /// <summary> 
        /// Получение случайных данных.
        /// <summary>
        private static List<decimal> GetRandomData()
        {
            int arrayLength = GetArrayLength();
            List<decimal> array = new();
            Random rand = new();
            for (int i = 0; i < arrayLength; i++)
            {
                array.Add(rand.Next(-100, 100));
            }
            return array;
        }

        /// <summary> 
        /// Получение длины массива, который требуется создать.
        /// Необходимо для ввода с помощью случайных чисел.
        /// <summary>
        private static int GetArrayLength()
        {
            bool correctInput = false;
            int arrayLength = 0;
            Console.WriteLine("Введите длину массива.");
            Console.WriteLine("Минимальное разрешенная длина не менее 3-х элементов.");
            Console.WriteLine("Для меньших значений решение задачи бессмыленно.");
            const int MINIMAL_ARRAY_SIZE = 3;
            while (!correctInput)
            {
                if(int.TryParse(Console.ReadLine(), out arrayLength))
                {
                    if(arrayLength >= MINIMAL_ARRAY_SIZE)
                    {
                        correctInput = true;
                    }
                    else
                    {
                        Console.WriteLine("Неверная длина массива. Попробуйте еще раз.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Попробуйте еще раз.");
                }
            }
            return arrayLength;
        }
        /// <summary> 
        /// Получение данных из файла.
        /// <summary>
        private static List<decimal> GetDataFromFile()
        {
            List<decimal> array = new();
            Console.WriteLine("Значения должны лежать в первой строке файла и представлять набор чисел с пробелами.");

            bool correctFile = false;
            string filename;
            List<string> input = new();
            do
            {
                Console.WriteLine("Введите путь к файлу.");
                filename = Console.ReadLine()!;
                if (FileCorrect(filename))
                {
                    using (FileStream fstream = File.OpenRead(filename))
                    {
                        using (StreamReader streamReader = new StreamReader(fstream))
                        {
                            string textLine = streamReader.ReadLine()!;

                            input.AddRange(textLine.Replace(',', '.').Split(' '));
                        }
                    }
                    if(InputIsCorrect(input, array))
                    {
                        correctFile = true;
                    }
                }
                if(!correctFile)
                {
                    Console.WriteLine("Ввод не верен");
                    input = new();
                }
            } while (!correctFile);
            return array;
        }
        /// <summary> 
        /// Проверка, что данный файл содержит какие-либо данные.
        /// <summary>
        private static bool FileCorrect(string filename)
        {
            // Проверка существования файла
            if (File.Exists(filename))
            {
                FileInfo fileInfo = new FileInfo(filename);
                if (fileInfo.Length == 0)
                {
                    Console.WriteLine("Ошибка! Файл пуст! Выберите другой файл.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Ошибка! Проверьте название файла.");
                return false;
            }
            return true;
        }
    }
}
