/// <summary> 
/// Модуль для вывода данных.
/// Здесь происходит вывод данных на консоль или же сохранение данных.
/// Отдельно рассматриваются исходные и полученные в результате работы программы данные.
/// <summary>
using static Lab_2_Domrachev.Calculation;

namespace Lab_2_Domrachev
{
    internal class OutputProcessing
    {
        /// <summary> 
        /// Конструктор класса.  
        /// <summary>
        public OutputProcessing()
        {
            ResultMessage = new();
        }
        /// <summary> 
        /// Возможные состояния выбранных для записи файлов.  
        /// <summary>
        public enum FileState
        {
            ReadOnly,
            Occupied,
            NotExist,
            Empty,
            NotEmpty
        };
        /// <summary> 
        /// Вывод на экран исходных данных.  
        /// <summary>
        public static void ShowSourseData(List<decimal> array)
        {
            Console.WriteLine("Исходные данные");
            foreach (var item in array)
            {
                Console.Write(item +" ");
            }
            Console.WriteLine();
        }
        /// <summary> 
        /// Отображение результата.  
        /// <summary>
        public void ShowResultData(OrderResult result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            ResultMessage = GetResultText(result);
            foreach (string text in ResultMessage)
            {
                Console.WriteLine(text);
            }

            Console.WriteLine();
        }
        /// <summary> 
        /// Создание массива строк, который будет содержать информацию о результате работы программы.
        /// Полученный массив используется как для вывода на консоль, так и для сохранения результата в файл.
        /// <summary>
        private List<string> GetResultText(OrderResult result)
        {
            List<string> resultMessage = new();
            resultMessage.AddRange(GetOrderedArrayText(result.OrderedElements));
            resultMessage.AddRange(GetBestRuleText(result.BestRule));
            resultMessage.AddRange(GetDeletedIndexesText(result.DeletedIndexes));
            return resultMessage;
        }
        /// <summary> 
        /// Свойство для хранения сообщения о результате работы программы.
        /// <summary>
        private List<string> ResultMessage { get; set; }
        /// <summary> 
        /// Сохранение результата.  
        /// <summary>
        public void SaveResultData()
        {
            using (FileStream filestream = GetFileStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(filestream))
                {
                    foreach (string text in ResultMessage)
                    {
                        streamWriter.WriteLine(text);
                    }
                }
            }
            Console.WriteLine("Данные успешно сохранены.");
        }
        /// <summary> 
        /// Создание текста об индексах, которые были удалены в исходном массиве.
        /// <summary>
        private List<string> GetDeletedIndexesText(List<int> deletedIndexes)
        {
            List<string> deletedIndexesMessage = new();
            string newLine = Environment.NewLine;
            if (deletedIndexes.Count == 0)
            {
                deletedIndexesMessage.Add("Массив уже был упорядочен");
            }
            else
            {
                deletedIndexesMessage.Add("В массиве были удалены элементы со следующими индексами:");
                string deletedIndexesText = "";
                foreach (int index in deletedIndexes)
                {
                    deletedIndexesText += index + 1 + " ";
                }
                deletedIndexesMessage.Add(deletedIndexesText);
            }
            return deletedIndexesMessage;
        }

        /// <summary> 
        /// Создание текста о правиле, с помощью которого был упорядочен массив.
        /// <summary>
        private List<string> GetBestRuleText(OrderRules bestRule)
        {
            List<string> bestRuleMessage = new() {"Массив упорядочен "};
            switch (bestRule)
            {
                case OrderRules.NextElementIsLessOrEqual:
                    {
                        bestRuleMessage.Add("по убыванию или равенству элементов.");
                    }
                    break;
                case OrderRules.NextElementIsGreaterOrEqual:
                    {
                        bestRuleMessage.Add("по возрастанию или равенству элементов.");
                    }
                    break;
                case OrderRules.NextElementIsEqual:
                    {
                        bestRuleMessage.Add("как множество равных элементов.");
                    }
                    break;
            }
            return bestRuleMessage;
        }

        /// <summary> 
        /// Создание текста об элементах результирующего упорядоченного массива.
        /// <summary>
        private List<string> GetOrderedArrayText(List<decimal> orderedArray)
        {
            List<string> orderedArrayMessage = new() { "Упорядоченный массив:" };
            string resultArray = "";
            foreach (decimal element in orderedArray)
            {
                resultArray += element + " ";
            }
            orderedArrayMessage.Add(resultArray);
            return orderedArrayMessage;
        }
        /// <summary> 
        /// Сохранение исходных данных.  
        /// <summary>
        public void SaveSourseData(List<decimal> array)
        {
            using (FileStream filestream = GetFileStream())
            {
                using (StreamWriter streamWriter = new(filestream))
                {
                    streamWriter.Write(array.First());
                    for (int i = 1; i < array.Count; i++)
                    {
                        streamWriter.Write(" " + array[i]);
                    }
                }
            }
        }
        /// <summary> 
        /// Получение потока для работы с файлом.  
        /// <summary>
        public FileStream GetFileStream()
        {
            string filename = "";
            FileState fileState;
            while (true)
            {
                if (GetCorrectFilename(ref filename, out fileState))
                {
                    FileInfo fileInfo = new(filename);
                    if (fileState == FileState.Empty)
                    {
                        return File.OpenWrite(filename);
                    }
                    else if (ProcessNotEmptyFileChoice())
                    {
                        try
                        {
                            return ProcessNotEmptyFile(filename);
                        }
                        catch
                        {
                            Console.WriteLine("Не удалось записать данные, выберите другой файл.");
                        }
                    }
                }
            }
        }
        /// <summary> 
        /// Обработка вариантов изменения/пересоздания файла при работе с непустым файлом.
        /// <summary>
        private FileStream ProcessNotEmptyFile(string filename)
        {
            Console.WriteLine("Enter - сохранить в конец файла.");
            Console.WriteLine("Любая другая клавиша - перезаписать файл.");
            FileStream fstream;
            if (Console.ReadKey().Key.ToString() == "Enter")
            {
                fstream = File.OpenWrite(filename);
                fstream.Seek(0, SeekOrigin.End);
            }
            else
            {
                fstream = File.Create(filename);
            }
            return fstream;
        }
        /// <summary> 
        /// Получение корректного имени файла.  
        /// <summary>
        private bool GetCorrectFilename(ref string filename, out FileState fileState)
        {
            Console.WriteLine("Введите название файла.");
            filename = Console.ReadLine()!;

            fileState = CheckFileState(filename);
            switch ((FileState)fileState)
            {
                case FileState.ReadOnly:
                    {
                        Console.WriteLine("Ошибка! Файл находится в режиме только для чтения. Выберите другой файл!");
                        return false;
                    }
                case FileState.Occupied:
                    {
                        Console.WriteLine("Ошибка! Файл занят другим процессом. Выберите другой файл!");
                        return false;
                    }
                case FileState.NotExist:
                    {
                        try
                        {
                            using (FileStream fstream = File.Create(filename))
                            {
                                fileState = FileState.Empty;
                            }
                            return true;
                        }
                        catch
                        {
                            Console.WriteLine("Ошибка! Проверьте правильность имени файла!");
                            return false;
                        }
                    }
                default:
                    {
                        return true;
                    }
            }
        }
        /// <summary> 
        /// Выбор варианта записи в файл, в котором присутствуют данные.
        /// <summary>
        private bool ProcessNotEmptyFileChoice()
        {
            Console.WriteLine("Файл не пуст!");
            Console.WriteLine("Enter - продолжить работу с этим файлом.");
            Console.WriteLine("Любая другая клавиша - выбрать другой файл.");
            if (Console.ReadKey().Key.ToString() == "Enter")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary> 
        /// Проверка состояния файла.  
        /// <summary>
        private FileState CheckFileState(string filename)
        {
            if (File.Exists(filename))
            {
                FileInfo fileInfo = new(filename);
                if (fileInfo.IsReadOnly)
                {
                    return FileState.ReadOnly;
                }
                if (fileInfo.Length == 0)
                {
                    return FileState.Empty;
                }
                else
                {
                    //Для того, чтобы обработать исключения, когда файл оказывается занят другим процессом
                    try
                    {
                        using (var fstream = File.Open(filename, FileMode.Open, FileAccess.Write, FileShare.None))
                        {
                            return FileState.NotEmpty;
                        }
                    }
                    catch (Exception)
                    {
                        return FileState.Occupied;
                    }
                }
            }
            else
            {
                return FileState.NotExist;
            }
        }

    }
}
