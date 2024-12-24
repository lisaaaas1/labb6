using System.Windows;

namespace labb6;

public partial class TaskTwo : Window
{
    private HashTableTwo hashTable;
    
    public TaskTwo()
    {
        
        InitializeComponent();
        hashTable = new HashTableTwo();
    }
    // Вставка элемента
    private void InsertButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(KeyTextBox.Text, out int key))
        {
            try
            {
                // Выбор метода хеширования (0 - Линейное, 1 - Квадратичное, 2 - Двойное хеширование)
                int method = HashingMethodComboBox.SelectedIndex;

                hashTable.Insert(key, method);
                ResultTextBox.Text = $"Элемент {key} успешно вставлен!";
            }
            catch (Exception ex)
            {
                ResultTextBox.Text = $"Ошибка: {ex.Message}";
            }
        }
        else
        {
            ResultTextBox.Text = "Введите корректное число!";
        }
    }

    // Поиск элемента
    private void SearchButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(KeyTextBox.Text, out int key))
        {
            bool found = hashTable.Search(key, 0); // 0 - Линейное исследование
            ResultTextBox.Text = found ? $"Элемент {key} найден!" : $"Элемент {key} не найден.";
        }
        else
        {
            ResultTextBox.Text = "Введите корректное число!";
        }
    }

    // Удаление элемента
    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(KeyTextBox.Text, out int key))
        {
            bool deleted = hashTable.Delete(key, 0); // 0 - Линейное исследование
            ResultTextBox.Text = deleted ? $"Элемент {key} удален." : $"Элемент {key} не найден для удаления.";
        }
        else
        {
            ResultTextBox.Text = "Введите корректное число!";
        }
    }

    // Тестирование хеш-функций
    private void TestButton_Click(object sender, RoutedEventArgs e)
    {
        ResultTextBox.Text = "Тестирование хеш-функций...\n";
        hashTable.TestHashFunctions();
        ResultTextBox.Text += "Тестирование завершено. Проверьте консоль для подробных данных.";
    }
}