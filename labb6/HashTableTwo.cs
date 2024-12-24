using System.Windows;

namespace labb6;

public class HashTableTwo
{
    private static int TableSize = 10000;
    private int[] table;
    private bool[] isOccupied;

    public HashTableTwo()
    {
        table = new int[TableSize];
        isOccupied = new bool[TableSize];
    }

    // Линейное исследование
    private int LinearProbing(int key, int i)
    {
        return (key + i) % TableSize;
    }

    // Квадратичное исследование
    private int QuadraticProbing(int key, int i)
    {
        return (key + i * i) % TableSize;
    }

    // Двойное хеширование
    private int DoubleHashing(int key, int i)
    {
        int hash1 = key % TableSize;
        int hash2 = 1 + (key % (TableSize - 1)); // вторичный хеш
        return (hash1 + i * hash2) % TableSize;
    }

    private void ResizeTable()
    {
        // Удваиваем размер таблицы
        int newTableSize = TableSize * 2;
        int[] newTable = new int[newTableSize];
        bool[] newIsOccupied = new bool[newTableSize];

        // Переносим все элементы из старой таблицы в новую
        for (int i = 0; i < TableSize; i++)
        {
            if (isOccupied[i])
            {
                int key = table[i];
                for (int j = 0; j < newTableSize; j++)
                {
                    int index = LinearProbing(key, j); // Можно использовать другой метод хеширования
                    if (!newIsOccupied[index])
                    {
                        newTable[index] = key;
                        newIsOccupied[index] = true;
                        break;
                    }
                }
            }
        }

        table = newTable;
        isOccupied = newIsOccupied;
        TableSize = newTableSize;
        MessageBox.Show($"Таблица расширена до нового размера: {newTableSize}");
    }
    
    // Вставка элемента
    public void Insert(int key, int method = 0)
    {
        for (int i = 0; i < TableSize; i++)
        {
            int index = method switch
            {
                0 => LinearProbing(key, i),
                1 => QuadraticProbing(key, i),
                2 => DoubleHashing(key, i),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!isOccupied[index])
            {
                table[index] = key;
                isOccupied[index] = true;
                return;
            }
        }

        // Переполнение таблицы — расширяем таблицу
        ResizeTable();
        Insert(key, method);  // После расширения повторно вставляем элемент
    }

    // Поиск элемента
    public bool Search(int key, int method = 0)
    {
        for (int i = 0; i < TableSize; i++)
        {
            int index = method switch
            {
                0 => LinearProbing(key, i),
                1 => QuadraticProbing(key, i),
                2 => DoubleHashing(key, i),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!isOccupied[index]) return false;
            if (table[index] == key) return true;
        }

        return false;
    }

    // Удаление элемента
    public bool Delete(int key, int method = 0)
    {
        for (int i = 0; i < TableSize; i++)
        {
            int index = method switch
            {
                0 => LinearProbing(key, i),
                1 => QuadraticProbing(key, i),
                2 => DoubleHashing(key, i),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!isOccupied[index]) return false;
            if (table[index] == key)
            {
                isOccupied[index] = false;
                return true;
            }
        }

        return false;
    }

    // Подсчет длины самого длинного кластера
    public int LongestCluster()
    {
        int maxCluster = 0;
        int currentCluster = 0;

        for (int i = 0; i < TableSize; i++)
        {
            if (isOccupied[i])
            {
                currentCluster++;
                maxCluster = Math.Max(maxCluster, currentCluster);
            }
            else
            {
                currentCluster = 0;
            }
        }

        return maxCluster;
    }

    // Метод для тестирования производительности
    public void TestHashFunctions()
    {
        Random rand = new Random();
        int[] keys = new int[10000];
        for (int i = 0; i < 10000; i++)
        {
            keys[i] = rand.Next();
        }

        // Вставка и поиск для каждой хэш-функции
        for (int method = 0; method < 3; method++)
        {
            Console.WriteLine($"Тестирование метода {method + 1}");
            var start = DateTime.Now;
            foreach (var key in keys)
            {
                Insert(key, method);
            }
            var end = DateTime.Now;
            Console.WriteLine($"Вставка времени: {end - start}");

            start = DateTime.Now;
            foreach (var key in keys)
            {
                Search(key, method);
            }
            end = DateTime.Now;
            Console.WriteLine($"Поиск времени: {end - start}");
        }
    }
}