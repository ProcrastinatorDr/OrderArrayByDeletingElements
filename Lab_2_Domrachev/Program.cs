/// <summary> 
/// Главный модуль. Отсюда происходит вызов всех компонентов программы для решения задачи.
/// <summary>
using static Lab_2_Domrachev.Calculation;

namespace Lab_2_Domrachev
{
    internal class Program
    {
        /// <summary> 
        /// Главная функция программы.
        /// <summary>
        static void Main(string[] args)
        {
            bool end = false;
            do
            {
                ProgramInfo();
                InputProcessing.GetData(out List<decimal> array);
                OutputProcessing.ShowSourseData(array);
                Console.WriteLine("Хотите ли вы сохранить исходные данные в файл?");
                Console.WriteLine("Enter - да;");
                Console.WriteLine("Любая другая клавиша - продолжить без сохранения.");
                if (Console.ReadKey().Key.ToString() == "Enter")
                {
                    OutputProcessing sourseData = new();
                    sourseData.SaveSourseData(array);
                }
                Calculation calculation = new();
                calculation.OrderArrayByDeletionElements(array);

                OutputProcessing outputProcessing = new();
                OrderResult result = calculation.result;
                outputProcessing.ShowResultData(result);
                Console.WriteLine("Хотите ли вы сохранить результат работы программы в файл?");
                Console.WriteLine("Enter - да;");
                Console.WriteLine("Любая другая клавиша - продолжить без сохранения.");
                if (Console.ReadKey().Key.ToString() == "Enter")
                {
                    outputProcessing.SaveResultData();
                }

                Console.WriteLine("Хотите ли вы выйти из программы?");
                Console.WriteLine("Enter - да;");
                Console.WriteLine("Любая другая клавиша - перезапустить программу.");
                if (Console.ReadKey().Key.ToString() == "Enter")
                {
                    end = true;
                }
                else
                {
                    Console.Clear();
                }
            } while (!end);
            Console.WriteLine("Спасибо за использование!");
            Console.WriteLine("Hello, World!");
        }

        /// <summary> 
        /// Вывод информации о программе 
        /// <summary>
        static void ProgramInfo()
        {
            string newLine = Environment.NewLine;
            Console.WriteLine("Доброго времени суток!" + newLine
                    + "Эта программа создает упорядоченный массив из заданного" + newLine
                    + "путем удаления минимального числа элементов." + newLine
                    + "Считается она за счет взятия за первый элемент нового массива поочередно каждого элемента в заданном массиве." + newLine
                    + "С этим элементом происходит по порядку сравнение каждого слудующего элемента массива." + newLine
                    + "Если эти элементы попадают под условие упорядочивания они добавляются в новый массив," + newLine
                    + "иначе пропускаются." + newLine
                    + "В качестве правила упорядочивания поочередно берется" + newLine
                    + "1. Совпадение каждого элемента массива с предыдущим;" + newLine
                    + "2. Следующий элемент больше или равен предыдущему;" + newLine
                    + "3. Следующий элемент меньше или равен предыдущему." + newLine
                    + "Полученные массивы сравниваются, и в качестве лучшего отбирается массив с большим числом элементов." + newLine
                    + "Если в результате получается несколько массивов, лучшим определяется тот, что был получен раньше." + newLine
                    + "В результате получается новый упорядоченный массив, правило его упорядочивания и список удаленных элементов." + newLine
                    + "Полученные данные, как и исходные возможно записать в файл.");
        }
    }
}