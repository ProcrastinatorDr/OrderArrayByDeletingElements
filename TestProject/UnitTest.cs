/// <summary> 
/// Модуль для тестирования расчетного модуля приложения.
/// Автор программы: студент 415 группы Глеб Домрачев.
/// <summary>
using Lab_2_Domrachev;
using static Lab_2_Domrachev.Calculation;

namespace TestProject
{
    [TestClass]
    public class UnitTest
    {
        /// <summary> 
        /// Тест, в котором массив не упорядочен. Используются целые положительные числа.
        /// В результате должен получиться массив, в котором каждый элемент строго больше предыдущего
        /// <summary>
        [TestMethod]
        public void ArrayMustBeOrdered_NextElementIsGreaterOrEqual()
        {
            List<decimal> testArray = new() { 1, 2, 3, 4, 0, 5, 6 };
            List<int> expectedDeletedIndexes = new() { 4 };
            List<decimal> expectedArray = new() { 1, 2, 3, 4, 5, 6 };
            OrderRules expectedRule = OrderRules.NextElementIsGreaterOrEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// Тест, в котором массив не упорядочен. Используются целые, положительные и отрицательные числа.
        /// В результате должен получиться массив, в котором каждый элемент меньше либо равен предыдущему.
        /// <summary>
        [TestMethod]
        public void ArrayMustBeOrdered_NextElementIsLessOrEqual()
        {
            List<decimal> testArray = new() { -1, 10, 9, 9, 9, -1, 5 };
            List<int> expectedDeletedIndexes = new() { 0, 6 };
            List<decimal> expectedArray = new() { 10, 9, 9, 9, -1 };
            OrderRules expectedRule = OrderRules.NextElementIsLessOrEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// Тест, в котором массив не упорядочен. Используются целые, дробные, положительные и отрицательные числа.
        /// В результате должен получиться массив, в котором значения всех элементов совпадают.
        /// <summary>
        [TestMethod]
        public void ArrayMustBeOrdered_NextElementIsEqual()
        {
            List<decimal> testArray = new() { 0, 1, 0, 0, -23.5m, 0, 0 };
            List<int> expectedDeletedIndexes = new() { 1, 4 };
            List<decimal> expectedArray = new() { 0, 0, 0, 0, 0};
            OrderRules expectedRule = OrderRules.NextElementIsEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// Тест, в котором массив уже упорядочен по возрастанию, каждый элемент строго больше предыдущего. 
        /// Используются положительные целые и дробные числа.
        /// <summary>
        [TestMethod]
        public void ArrayIsOrdered_NextElementIsGreaterOrEqual()
        {
            List<decimal> testArray = new() { 1, 99, 100, 110, 112.4m, 654.1m };
            List<int> expectedDeletedIndexes = new() { };
            List<decimal> expectedArray = new() { 1, 99, 100, 110, 112.4m, 654.1m };
            OrderRules expectedRule = OrderRules.NextElementIsGreaterOrEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// Тест, в котором массив уже упорядочен по убыванию, первые элементы целые числа и они совпадают,
        /// последний элемент - дробное число, незначительно меньше предыдущих. 
        /// <summary>
        [TestMethod]
        public void ArrayIsOrdered_NextElementIsLessOrEqual()
        {
            List<decimal> testArray = new() { -2, -2, -2, -2, -2, -2.0001m };
            List<int> expectedDeletedIndexes = new() { };
            List<decimal> expectedArray = new() { -2, -2, -2, -2, -2, -2.0001m };
            OrderRules expectedRule = OrderRules.NextElementIsLessOrEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// Тест, в котором массив уже упорядочен, при этом каждый элемент равен предыдущему.
        /// <summary>
        [TestMethod]
        public void ArrayIsOrdered_NextElementIsEqual()
        {
            List<decimal> testArray = new() { 0, 0.0m, 0, 0, 0, 0.000m };
            List<int> expectedDeletedIndexes = new() { };
            List<decimal> expectedArray = new() { 0, 0, 0, 0, 0, 0 };
            OrderRules expectedRule = OrderRules.NextElementIsEqual;

            TestData(testArray, expectedDeletedIndexes, expectedArray, expectedRule);
        }
        /// <summary> 
        /// функция, в которой используются данные записанные в каждом из вышеуказанных тестов.
        /// Здесь вызывается расчетная функция и полученные данные сравниваются с ожидаемыми.
        /// <summary>
        public void TestData(List<decimal> testArray, List<int> expectedDeletedIndexes, List<decimal> expectedArray, OrderRules expectedRule)
        {
            Calculation testCalculation = new();
            testCalculation.OrderArrayByDeletionElements(testArray);

            // Проверка индексов удаленных элементов
            Assert.AreEqual(expectedDeletedIndexes.Count, testCalculation.result.DeletedIndexes.Count);
            for (int i = 0; i < expectedDeletedIndexes.Count; i++)
            {
                Assert.AreEqual(expectedDeletedIndexes[i], testCalculation.result.DeletedIndexes[i]);
            }

            // Проверка элементов полученного массива
            Assert.AreEqual(expectedArray.Count, testCalculation.result.OrderedElements.Count);
            for (int i = 0; i < expectedArray.Count; i++)
            {
                Assert.AreEqual(expectedArray[i], testCalculation.result.OrderedElements[i]);
            }

            Assert.AreEqual(expectedRule, testCalculation.result.BestRule);
        }
    }

}