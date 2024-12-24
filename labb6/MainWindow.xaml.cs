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
        HashTableTwo hashTable = new HashTableTwo();

        // Генерация случайных чисел и вставка в таблицу
        Random rand = new Random();
        for (int i = 0; i < 10000; i++)
        {
            int key = rand.Next();
            hashTable.Insert(key, 0); // 0 - линейное исследование
        }

        // Тестирование поиска
        Console.WriteLine(hashTable.Search(1234, 0));

        // Подсчет длины самого длинного кластера
        Console.WriteLine($"Длина самого длинного кластера: {hashTable.LongestCluster()}");

        // Тестирование производительности хэш-функций
        hashTable.TestHashFunctions();
        TaskTwo taskTwo = new TaskTwo();
        taskTwo.Show();
    }
}