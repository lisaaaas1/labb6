using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace labb6;

public partial class Task1 : Window
{
    private HashTable hashTable;

    public Task1()
    {
        InitializeComponent();
        hashTable = new HashTable();  // Инициализация хеш-таблицы
        InitializeComboBox();
    }
    
    private void InitializeComboBox()
    {
        HashMethodComboBox.Items.Add("Division");
        HashMethodComboBox.Items.Add("Multiplication");
        HashMethodComboBox.Items.Add("Custom 1");
        HashMethodComboBox.Items.Add("Custom 2");
        HashMethodComboBox.Items.Add("Custom 3");

        HashMethodComboBox.SelectedIndex = 0; // Устанавливаем значение по умолчанию
    }

    // Обработчик для кнопки Insert (вставка элемента в хеш-таблицу)
    private void InsertButton_Click(object sender, RoutedEventArgs e)
    {
        string key = KeyTextBox.Text.Trim();
        string value = ValueTextBox.Text.Trim();

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
        {
            ResultTextBox.Text = "Key and Value cannot be empty!";
            return;
        }

        // Устанавливаем метод хеширования
        SetHashMethod();

        // Вставляем элемент в хеш-таблицу
        hashTable.Insert(key, value);
        ResultTextBox.Text = $"Inserted: {key} = {value}";
    }

    // Обработчик для кнопки Search (поиск элемента в хеш-таблице)
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        string key = KeyTextBox.Text.Trim();

        if (string.IsNullOrEmpty(key))
        {
            ResultTextBox.Text = "Key cannot be empty!";
            return;
        }

        // Устанавливаем метод хеширования
        SetHashMethod();

        // Ищем элемент в хеш-таблице
        string result = hashTable.Search(key);
        if (result != null)
        {
            ResultTextBox.Text = $"Found: {key} = {result}";
        }
        else
        {
            ResultTextBox.Text = $"Key '{key}' not found!";
        }
    }

    // Обработчик для кнопки Delete (удаление элемента из хеш-таблицы)
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        string key = KeyTextBox.Text.Trim();

        if (string.IsNullOrEmpty(key))
        {
            ResultTextBox.Text = "Key cannot be empty!";
            return;
        }

        // Устанавливаем метод хеширования
        SetHashMethod();

        // Удаляем элемент из хеш-таблицы
        bool success = hashTable.Delete(key);
        if (success)
        {
            ResultTextBox.Text = $"Deleted: {key}";
        }
        else
        {
            ResultTextBox.Text = $"Key '{key}' not found!";
        }
    }

    // Устанавливаем метод хеширования на основе выбранного в ComboBox
    private void SetHashMethod()
    {
        // Проверяем выбранный метод хеширования в ComboBox и применяем его
        if (HashMethodComboBox.SelectedIndex == 0)
        {
            hashTable.SetHashingMethod(HashTable.HashingMethod.Division);
        }
        else if (HashMethodComboBox.SelectedIndex == 1)
        {
            hashTable.SetHashingMethod(HashTable.HashingMethod.Multiplication);
        }
        else if (HashMethodComboBox.SelectedIndex == 2)
        {
            hashTable.SetHashingMethod(HashTable.HashingMethod.Custom1);
        }
        else if (HashMethodComboBox.SelectedIndex == 3)
        {
            hashTable.SetHashingMethod(HashTable.HashingMethod.Custom2);
        }
        else if (HashMethodComboBox.SelectedIndex == 4)
        {
            hashTable.SetHashingMethod(HashTable.HashingMethod.Custom3);
        }
    }
    
    private void GenerateAndInsertButton_Click(object sender, RoutedEventArgs e)
    {
        var keys = KeyGenerator.GenerateKeys(100000);
        var random = new Random();
        int count = 0;
        
        foreach (var key in keys)
        {
            string value = random.Next(1000, 9999).ToString(); 
            hashTable.Insert(key, value);
            
            count++;
            if (count % 100 == 0)
            {
                ResultTextBox.AppendText($"Inserted element {count}: Key = {key}, Value = {value}\n");
            }
        }
        
        ResultTextBox.AppendText("100000 elements inserted successfully!\n");
    }
    
    private void StatsButton_Click(object sender, RoutedEventArgs e)
    {
        double loadFactor = hashTable.GetLoadFactor();
        var (shortest, longest) = hashTable.GetChainLengthStatistics();

        ResultTextBox.Text = $"коэффициент заполнения: {loadFactor:F2}\n" +
                             $"самая короткая цепочка: {shortest}\n" +
                             $"самая длинная цепочка: {longest}";
    }
    
    private void CompareHashFunctionsButton_Click(object sender, RoutedEventArgs e)
    {
        // Генерация 100000 случайных ключей
        var keys = KeyGenerator.GenerateKeys(100000);
        var values = new string[100000];
        for (int i = 0; i < 100000; i++)
        {
            values[i] = "Value_" + i.ToString();
        }

        var hashMethods = new HashTable.HashingMethod[]
        {
            HashTable.HashingMethod.Division,
            HashTable.HashingMethod.Multiplication,
            HashTable.HashingMethod.Custom1,
            HashTable.HashingMethod.Custom2,
            HashTable.HashingMethod.Custom3
        };

        foreach (var method in hashMethods)
        {
            // Устанавливаем метод хеширования
            hashTable.SetHashingMethod(method);

            // Вставляем все элементы в хеш-таблицу
            for (int i = 0; i < keys.Length; i++)
            {
                hashTable.Insert(keys[i], values[i]);
            }

            // Получаем коэффициент заполнения
            double loadFactor = hashTable.GetLoadFactor();

            // Получаем статистику длины цепочек
            var (shortest, longest) = hashTable.GetChainLengthStatistics();
            double averageChainLength = (double)(shortest + longest) / 2;

            // Отображаем результаты
            ResultTextBox.Text += $"\nМетод: {method}\n" +
                                  $"коэффициент заполнения: {loadFactor:F4}\n" +
                                  $"самая короткая цепочка: {shortest}\n" +
                                  $"самая длинная цепочка: {longest}\n" +
                                  $"Средняя длина цепочки: {averageChainLength:F2}\n";
        }
    }
}

public class HashTable
{
    private const int TableSize = 1000;
    private LinkedList<Node>[] table;
    private Func<string, int> hashFunction;

    public enum HashingMethod
    {
        Division,
        Multiplication,
        Custom1,
        Custom2,
        Custom3
    }

    private const double A = 0.6180339887;
    
    public HashTable()
    {
        table = new LinkedList<Node>[TableSize];
        for (int i = 0; i < TableSize; i++)
        {
            table[i] = new LinkedList<Node>();
        }
        
        SetHashingMethod(HashingMethod.Division);
    }

    
    public void SetHashingMethod(HashingMethod method)
    {
        switch (method)
        {
            case HashingMethod.Division:
                hashFunction = DivisionMethod;
                break;
            case HashingMethod.Multiplication:
                hashFunction = MultiplicationMethod;
                break;
            case HashingMethod.Custom1:
                hashFunction = CustomMethod1;
                break;
            case HashingMethod.Custom2:
                hashFunction = CustomMethod2;
                break;
            case HashingMethod.Custom3:
                hashFunction = CustomMethod3;
                break;
            default:
                throw new ArgumentException("Unknown hashing method.");
        }
    }
    
    public double GetLoadFactor()
    {
        int filledBuckets = 0;
        foreach (var list in table)
        {
            if (list.Count > 0)
            {
                filledBuckets++;
            }
        }
        return (double)filledBuckets / TableSize;
    }

    // Метод для нахождения длины самой длинной и самой короткой цепочки
    public (int shortest, int longest) GetChainLengthStatistics()
    {
        int shortest = int.MaxValue;
        int longest = 0;

        foreach (var list in table)
        {
            int chainLength = list.Count;
            if (chainLength > longest)
            {
                longest = chainLength;
            }
            if (chainLength > 0 && chainLength < shortest)
            {
                shortest = chainLength;
            }
        }

        // Если таблица пуста, возвращаем 0 для самой короткой цепочки
        if (shortest == int.MaxValue)
        {
            shortest = 0;
        }

        return (shortest, longest);
    }

    // Метод деления
    private int DivisionMethod(string key)
    {
        int sum = 0;
        foreach (var ch in key)
        {
            sum += ch;
        }
        return sum % TableSize;
    }

    // Метод умножения
    private int MultiplicationMethod(string key)
    {
        double sum = 0;
        foreach (var ch in key)
        {
            sum += ch;
        }
        double fractionalPart = sum * A - Math.Floor(sum * A);
        return (int)(TableSize * fractionalPart);
    }

    // Метод кастомный 1
    private int CustomMethod1(string key)
    {
        double hash = 0;
        const double A = 0.6180339887;
        foreach (var ch in key)
        {
            hash += Math.Pow(ch * A, 2);  // Умножаем символ на A и возводим в квадрат
        }
        return Math.Abs((int)(hash % TableSize)); 
    }


    // Метод кастомный 2
    private int CustomMethod2(string key)
    {
        int hash = 0;
        foreach (var ch in key)
        {
            hash = (hash * 31) ^ (ch + 31);  // Используем XOR с числом 31 и умножаем на 31 для повышения сложности
        }
        return Math.Abs(hash % TableSize);
    }

    // Метод кастомный 3
    private int CustomMethod3(string key)
    {
        long hash = 0;
        const long prime = 16777619;
        foreach (var ch in key)
        {
            hash = (hash * prime) ^ ch;  // Умножаем на простое число и применяем XOR с текущим символом
        }
        return Math.Abs((int)(hash % TableSize));
    }


    // Вставка элемента
    public void Insert(string key, string value)
    {
        int index = hashFunction(key);
        var list = table[index];

        foreach (var node in list)
        {
            if (node.Key == key)
            {
                node.Value = value;
                return;
            }
        }

        list.AddLast(new Node(key, value));
    }

    // Поиск элемента
    public string Search(string key)
    {
        int index = hashFunction(key);
        var list = table[index];

        foreach (var node in list)
        {
            if (node.Key == key)
            {
                return node.Value;
            }
        }

        return null;
    }

    // Удаление элемента
    public bool Delete(string key)
    {
        int index = hashFunction(key);
        var list = table[index];

        var nodeToRemove = list.FirstOrDefault(n => n.Key == key);
        if (nodeToRemove != null)
        {
            list.Remove(nodeToRemove);
            return true;
        }
        return false;
    }

    // Класс узла
    private class Node
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public Node(string key, string value)
        {
            Key = key;
            Value = value;
        }
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