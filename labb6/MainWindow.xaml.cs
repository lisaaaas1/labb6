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

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void Button2_Click(object sender, RoutedEventArgs e)
    {
        // Предположим, что значения также будут строками
        HashTableTwo<string, string> hashTable = new HashTableTwo<string, string>(); // Используем HashTableTwo с ключами и значениями типа string

        // Генерация 10000 уникальных ключей
        string[] keys = KeyGenerator.GenerateKeys(1000, 10); // Генерируем 10,000 ключей длиной 10 символов

        foreach (var key in keys)
        {
            // Вставка ключа в хеш-таблицу с произвольным значением (например, самим ключом)
            hashTable.Insert(key, key); // Предполагаем, что метод Insert принимает ключ и значение
        }

        // Подсчет длины самого длинного кластера
        int longestCluster = hashTable.LongestClusterLength();
        
        Console.WriteLine($"The length of the longest cluster: {longestCluster}");
        
        TaskTwo taskTwo2 = new TaskTwo();
        taskTwo2.Show();
    }
}

