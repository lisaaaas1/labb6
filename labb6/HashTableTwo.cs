using System;
using System.Text;

public class HashTableTwo<TKey, TValue>
{
    private const int Size = 10000; // Размер хеш-таблицы
    private KeyValuePair<string, TValue>?[] table = new KeyValuePair<string, TValue>?[Size];
    private Func<string, int> hashFunction;
    private Func<int, string, int> collisionResolution;

    public HashTableTwo()
    {
        // Установим по умолчанию хеш-функцию и метод разрешения коллизий
        hashFunction = HashByDivision;
        collisionResolution = LinearProbing;
    }

    public void SetHashFunction(string method)
    {
        switch (method)
        {
            case "Метод деления":
                hashFunction = HashByDivision;
                break;
            case "Метод умножения":
                hashFunction = HashByMultiplication;
                break;
            case "ComputeHash":
                hashFunction = ComputeHash;
                break;
            case "Метод Полинома":
                hashFunction = HashByPolynomial;
                break;
            case "Метод FNV (Fowler–Noll–Vo)":
                hashFunction = HashByFNV;
                break;
            default:
                throw new ArgumentException("Неизвестный метод хеширования");
        }
    }

    public void SetCollisionResolution(string method)
    {
        switch (method)
        {
            case "Линейное исследование":
                collisionResolution = LinearProbing;
                break;
            case "Квадратичное исследование":
                collisionResolution = QuadraticProbing;
                break;
            case "Двойное хеширование":
                collisionResolution = DoubleHashing;
                break;
            case "Собственный метод 1":
                collisionResolution = CustomProbingMethod1;
                break;
            case "Собственный метод 2":
                collisionResolution = CustomProbingMethod2;
                break;
            default:
                throw new ArgumentException("Неизвестный метод разрешения коллизий");
        }
    }

     public void Insert(string key, TValue value)
    {
        int index = hashFunction(key);
        if (table[index].HasValue)
        {
            index = collisionResolution(index, key);
        }
    
        // Проверка на наличие свободного места
        if (table[index] == null)
        {
            table[index] = new KeyValuePair<string, TValue>(key, value);
        }
        else
        {
            throw new InvalidOperationException("Хеш-таблица заполнена.");
        }
    }

    public TValue Search(string key)
    {
        try
        {
            int index = hashFunction(key);

            // Сохраняем начальный индекс для проверки завершения поиска
            int startIndex = index;

            while (table[index].HasValue)
            {
                if (table[index].Value.Key == key)
                    return table[index].Value.Value;

                // Переход к следующему индексу для линейного пробирования
                index = (index + 1) % Size;

                // Проверяем, не вернулись ли мы к начальному индексу
                if (index == startIndex)
                    break; // Мы обошли всю таблицу
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new KeyNotFoundException($"Key '{key}' not found."); // Сообщение об ошибке
        }

        throw new KeyNotFoundException($"Key '{key}' not found."); // Сообщение об ошибке

    }

    public void Delete(string key)
    {
        int index = hashFunction(key);
        while (table[index].HasValue)
        {
            if (table[index].Value.Key == key)
            {
                table[index] = null; // Удаляем элемент
                return;
            }
            index = (index + 1) % Size; // Для линейного пробирования
        }
    }

    public int LongestClusterLength()
    {
        int longest = 0;
        int currentLength = 0;

        foreach (var item in table)
        {
            if (item.HasValue)
            {
                currentLength++;
            }
            else
            {
                longest = Math.Max(longest, currentLength);
                currentLength = 0;
            }
        }
        return Math.Max(longest, currentLength); // Проверка в конце массива
    }

    // Хеш-функции
    private int HashByDivision(string key)
    {
        if (Size <= 0) throw new ArgumentException("Size must be greater than zero.");
        int hash = key.GetHashCode();
        return Math.Abs(hash % Size);  // Убедитесь, что Size > 0
    }
    private int HashByMultiplication(string key)
    {
        if (Size <= 0) throw new ArgumentException("Size must be greater than zero.");
        double A = 0.6180339887; // Золотое сечение
        return (int)(Size * (key.GetHashCode() * A % 1)) % Size; // Убедитесь, что Size > 0
    }

    private int ComputeHash(string key)
    {
        if (Size <= 0) throw new ArgumentException("Size must be greater than zero.");
        int hash = 0;

        foreach (char c in key)
        {
            hash += c; // Суммируем ASCII-коды символов
            hash ^= (hash << 5); // Побитовый сдвиг и XOR
            hash ^= (hash >> 2); // Побитовый сдвиг и XOR
        }

        return Math.Abs(hash) % Size; // Убедитесь, что Size > 0
    }

    private int HashByPolynomial(string key)
    {
        if (Size <= 0) throw new ArgumentException("Size must be greater than zero.");
        int hash = 0;
        for (int i = 0; i < key.Length; i++)
        {
            hash += (int)(key[i] * Math.Pow(31, key.Length - 1 - i));
        }
        return Math.Abs(hash % Size); // Убедитесь, что Size > 0
    }

    private int HashByFNV(string key)
{
    if (Size <= 0) throw new ArgumentException("Size must be greater than zero.");
    const uint FNVOffsetBasis = 2166136261;
    const uint FNVPrime = 16777619;
    uint hash = FNVOffsetBasis;

    foreach (char c in key)
    {
        hash ^= (uint)c;
        hash *= FNVPrime;
    }
    return (int)(hash % Size); // Убедитесь, что Size > 0
}

    // Методы разрешения коллизий
    private int LinearProbing(int index, string key)
    {
        while (table[index].HasValue)
        {
            index = (index + 1) % Size; // Линейное пробирование
            // Убедитесь, что index не выходит за пределы
        }
        return index; // Возвращаем свободный индекс
    }

    private int QuadraticProbing(int index, string key)
    {
        int i = 1;
        // Проверяем, что индекс не выходит за пределы
        while (i < Size && table[(index + i * i) % Size].HasValue)
        {
            i++;
        }

        // Возвращаем свободный индекс, если i не превысил размер
        return (index + i * i) % Size;
    }

    private int DoubleHashing(int index, string key)
    {
        // Вторая хеш-функция для шага
        int stepSize = 7 - (key.GetHashCode() % 7); 
        stepSize = stepSize < 0 ? -stepSize : stepSize; // Обеспечиваем положительный шаг
    
        int originalIndex = index;  // Сохраняем исходный индекс, чтобы избежать зацикливания

        while (table[index].HasValue)  // Пока ячейка не пустая
        {
            index = (index + stepSize) % Size;

            // Если мы вернулись в исходную позицию, значит, таблица переполнена
            if (index == originalIndex)
            {
                throw new InvalidOperationException("Таблица переполнена");
            }
        }
        return index; // Возвращаем свободный индекс
    }
    // Собственные методы разрешения коллизий

    private int CustomProbingMethod1(int index, string key)
    {
        // Увеличиваем шаг на 2
        int i = 1;
        while (table[(index + i * 2) % Size].HasValue)
        {
            i++;
        }
        return (index + i * 2) % Size; // Возвращаем свободный индекс
    }

    private int CustomProbingMethod2(int index, string key)
    {
        // Если не нашли место, пробуем с начала
        int startIndex = index;
        while (table[index].HasValue)
        {
            index = (index + 1) % Size;
            if (index == startIndex)
                throw new InvalidOperationException("Таблица переполнена");
        }
        return index; // Возвращаем свободный индекс
    }
}

public static class KeyGenerator
{
    private static Random _random = new Random();
    
    public static string GenerateRandomKey(int length = 10)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append(chars[_random.Next(chars.Length)]);
        }
        return stringBuilder.ToString();
    }
    
    public static string[] GenerateKeys(int count, int length = 10)
    {
        var keys = new string[count];
        for (int i = 0; i < count; i++)
        {
            keys[i] = GenerateRandomKey(length);
        }
        return keys;
    }
}