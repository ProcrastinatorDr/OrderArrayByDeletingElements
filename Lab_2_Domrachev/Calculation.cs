/// <summary> 
/// Расчетный модуль программы. 
/// Сделан для упорядочивания поступившего массива, за счет удаления минимального числа элементов.
/// Упорядочивание происходит по одному из трех вариантов:
/// 1. Следующий элемент равен предыдущему;
/// 2. Следующий элемент больше или равен предыдущему;
/// 3. Следующий элемент меньше или равен предыдущему.
/// Если результат упорядочивания массива по очередному правилу не оказывается лучше предыдущего, он не учитывается,
/// а за результат программы определяют первый лучший.
/// Используемый алгоритм: 
/// В качестве упорядоченности заданы три вышеуказанных условия.
/// Решение задачи идет путем поочередного создания упорядоченных массивов,
/// начиная от каждого из элементов первичного массива.
/// Подходящие под условие следующие элементы первичного массива добавляются в новый.
/// После создания очередного массива происходит его сравнение с предыдущим.
/// Если новый массив оказывается большим по размеру, то он берется за образец,
/// и последующие массивы сравниваются с ним.
/// Для каждого из условий упорядоченности создается свой массив.
/// После чего происходит их сравнение и выбор лучшего среди упорядоченных.
/// Если находится несколько лучших, в качетстве результата определяется тот, что был получен раньше.
/// <summary>
namespace Lab_2_Domrachev
{
    public class Calculation
    {
        /// <summary> 
        /// Конструктор класса расчетов.
        /// <summary>
        public Calculation() 
        {
            result = new();
        }

        /// <summary> 
        /// Структура для хранения информации о полученном массиве.
        /// Сохраняется два списка:
        /// 1. Индексы удаленных элементов для упорядочивания массива;
        /// 2. Элементы полученного массива.
        /// <summary>
        public struct OrderResult
        {
            /// <summary> 
            /// Свойство для хранения информации об индексах удаленных элементов.
            /// <summary>
            public List<int> DeletedIndexes { get; set; }

            /// <summary> 
            /// Свойство для хранения информации об элементах нового упорядоченного массива.
            /// <summary>
            public List<decimal> OrderedElements { get; set; }

            /// <summary> 
            /// Свойство для хранения информации о правиле, по которому было произведено упорядочивание.
            /// <summary>
            public OrderRules BestRule { get; set; }

            /// <summary> 
            /// Конструктор для инициализации нового экземпляра структуры для дальнейшего заполнения полей.
            /// <summary>
            public OrderResult()
            {
                DeletedIndexes = new();
                OrderedElements = new();
                BestRule = 0;
            }

            /// <summary> 
            /// Конструктор для инициализации нового экземпляра структуры с уже известными полями.
            /// <summary>
            public OrderResult(List<int> deletedIndexes, List<decimal> orderedElements)
            {
                DeletedIndexes = deletedIndexes;
                OrderedElements = orderedElements;
                BestRule = 0;
            }
        }

        /// <summary> 
        /// Поле для хранения полученной в результате информации об упорядоченном массиве.
        /// <summary>
        public OrderResult result;

        /// <summary> 
        /// Варианты, по которым будет производиться сортировка.
        /// <summary>
        public enum OrderRules
        {
            NextElementIsEqual,
            NextElementIsGreaterOrEqual,
            NextElementIsLessOrEqual
        }
        /// <summary> 
        /// Делегат, в который сохраняется очередная функция, определяющая способ сравенения элементов в ходе упорядочивания массива.
        /// <summary>
        delegate bool OrderRule(decimal currentElement, decimal nextElement);

        /// <summary> 
        /// Координирующая функция. 
        /// Она поочередно вызывает функции для упорядочивания массива по определенному правилу и определяет лучший.
        /// <summary>
        public void OrderArrayByDeletionElements(List<decimal> array)
        {
            OrderRule orderRuleFunction;
            orderRuleFunction = NextElementIsEqual;
            const int NUMBER_OF_OPERATIONS = 3;
            OrderRules orderRuleIndex = 0;
            OrderResult newOrderResult;
            List<decimal> currentDeletedIndexes = new();
            for (int i = 0; i < NUMBER_OF_OPERATIONS; i++)
            {
                orderRuleIndex = (OrderRules)i;
                orderRuleFunction = GetNextOrderRule((OrderRules)i);
                newOrderResult = OrderArray(array, orderRuleFunction);
                if (newOrderResult.OrderedElements.Count > result.OrderedElements.Count)
                {
                    result = newOrderResult;
                    result.BestRule = orderRuleIndex;
                }
            }
            return;
        }

        /// <summary> 
        /// Функция, которая определяет лучший массив среди упорядоченных по заданному правилу.
        /// В ней происходит поочередный вызов функции GetNewOrderedArray,
        /// для которой в качестве нового первого массива опеределяется поочередно каждый из элементов текущего.
        /// <summary>
        private OrderResult OrderArray(List<decimal> array, OrderRule orderRule)
        {
            List<int> deletedIndexes = new List<int>();
            decimal firstElement = array[0];
            OrderResult greatestOrderResult = new();

            int lastElementToGetArray = array.Count - 2;
            // Начиная с каждого элемента создается упорядоченный массив
            for (int i = 0; i < lastElementToGetArray; i++)
            {
                OrderResult currentOrderResult = GetNewOrderedArray(array, i, orderRule);
                // Проверка, что массив лучше предыдущего
                if (currentOrderResult.OrderedElements.Count > greatestOrderResult.OrderedElements.Count)
                {
                    greatestOrderResult = currentOrderResult;
                }
            }
            return greatestOrderResult;
        }

        /// <summary> 
        /// Функция, которая упорядочивает массив за счет удаления элементов по заданному правилу.
        /// В ходе ее работы происходит полный проход по элементам массива, начиная от заданного.
        /// Заданный элемент определяется как первый элемент нового массива.
        /// Каждый следующий элемент сравнивается с последним элементом нового массива по заданному правилу.
        /// Подходящие элементы добавляются в новый массив.
        /// Индексы не подходящих элементов добавляются в список удаленных.
        /// Вывод:
        /// возвращается структура из списков индексов удаленных элементов и упорядоченного массива.
        /// Если массив уже был упорядочен, то в качестве списка удаленных индексов выступает пустой массив.
        /// <summary>
        private OrderResult GetNewOrderedArray(List<decimal> array, int firstNewArrayIndex, OrderRule orderRule)
        {
            List<decimal> orderedArray = new()
            {
                array[firstNewArrayIndex]
            };
            List<int> deletedIndexes = new List<int>();
            for (int i = 0; i < firstNewArrayIndex; i++)
            {
                deletedIndexes.Add(i);
            }
            for (int i = firstNewArrayIndex + 1; i < array.Count; i++)
            {
                if (orderRule(orderedArray.Last(), array[i]))
                {
                    orderedArray.Add(array[i]);
                }
                else
                {
                    deletedIndexes.Add(i);
                }
            }

            return new OrderResult(deletedIndexes, orderedArray);
        }

        /// <summary> 
        /// Назначение функции, по которой будет осуществляться сравнение элементов при их упорядочивании.
        /// <summary>
        private OrderRule GetNextOrderRule(OrderRules orderRule)
        {
            OrderRule rule;
            switch (orderRule)
            {
                case OrderRules.NextElementIsEqual:
                    {
                        rule = NextElementIsEqual;
                    }
                    break;
                case OrderRules.NextElementIsGreaterOrEqual:
                    {
                        rule = NextElementIsGreaterOrEqual;
                    }
                    break;
                case OrderRules.NextElementIsLessOrEqual:
                    {
                        rule = NextElementIsLessOrEqual;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
            return rule;
        }

        /// <summary> 
        /// Функция для проверки того, что следующий элемент массива совпадает с предыдущим
        /// <summary>
        private bool NextElementIsEqual(decimal currentElement, decimal nextElement)
        {
            if (nextElement == currentElement) return true;
            else return false;
        }

        /// <summary> 
        /// Функция для проверки того, что следующий элемент массива больше либо равен предыдущему
        /// <summary>
        private bool NextElementIsGreaterOrEqual(decimal currentElement, decimal nextElement)
        {
            if (nextElement >= currentElement) return true;
            else return false;
        }

        /// <summary> 
        /// Функция для проверки того, что следующий элемент массива меньше либо равен предыдущему
        /// <summary>
        private bool NextElementIsLessOrEqual(decimal currentElement, decimal nextElement)
        {
            if (nextElement <= currentElement) return true;
            else return false;
        }
    }
}
