using System.Windows;
using System.Windows.Controls;

namespace labb6;

public partial class TaskTwo : Window
{
    private HashTableTwo<string, string> hashTable; // Изменено на HashTableTwo<string>

    public TaskTwo()
    {
        InitializeComponent();
        hashTable = new HashTableTwo<string, string>(); // Инициализация с типом string
    }
    private void InsertButton_Click(object sender, RoutedEventArgs e)
    {
        string key = KeyInput.Text; // Ключ теперь строкового типа
        string value = ValueInput.Text;

        if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
        {
            string selectedHashFunction = ((ComboBoxItem)HashFunctionComboBox.SelectedItem)?.Content?.ToString();
            string selectedCollisionMethod = ((ComboBoxItem)HashMethodComboBox.SelectedItem)?.Content?.ToString();

            if (string.IsNullOrEmpty(selectedHashFunction) || string.IsNullOrEmpty(selectedCollisionMethod))
            {
                throw new ArgumentException("choose correct collision and function.");
            }
            hashTable.SetHashFunction(selectedHashFunction); // Установка выбранной хеш-функции
            hashTable.SetCollisionResolution(selectedCollisionMethod); // Установка метода разрешения коллизий

            hashTable.Insert(key, value); // Вставка элемента

            MessageBox.Show($"Key '{key}' with value '{value}' success inserted.") ;
        }
        else
        {
            MessageBox.Show("Enter right format key and value.") ;

        }
    }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string key = KeyInput.Text; // Ключ теперь строкового типа
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    string selectedHashFunction = ((ComboBoxItem)HashFunctionComboBox.SelectedItem)?.Content.ToString();
                    hashTable.SetHashFunction(selectedHashFunction); // Установка выбранной хеш-функции
    
                    string value = hashTable.Search(key); // Поиск элемента
                    OutputText.Text = value != null ? $"Ключ '{key}' найден со значением: '{value}'." : $"Ключ '{key}' не найден.";
                    
                }
                else
                {
                    MessageBox.Show("Enter right format key .");
                }
            }
            catch (KeyNotFoundException ex)
            {
                // Обработка исключения: показываем сообщение пользователю
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            string key = KeyInput.Text; // Ключ теперь строкового типа

            if (!string.IsNullOrEmpty(key))
            {
                hashTable.Delete(key); // Удаление элемента
                MessageBox.Show($"Ключ '{key}' удален.");
            }
            else
            {
                MessageBox.Show("Введите корректный ключ.") ;

            }
        }

        private void LongestClusterButton_Click(object sender, RoutedEventArgs e)
        {
            int longestCluster = hashTable.LongestClusterLength(); // Подсчет длины самого длинного кластера
            MessageBox.Show($"Длина самого длинного кластера: {longestCluster}.");
        }
    }
