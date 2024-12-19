using System; 
using System.IO; 
using System.Reflection; 
using System.Text; 
using System.Windows; 
using System.Windows.Threading; 

namespace GraphWinProgram
{
    public partial class MainWindow : Window 
    {
        private DispatcherTimer timer; // Таймер для добавления точек на график с интервалом времени
        private GraphManager graphManager; // Менеджер графиков для управления рисованием и данными

        public MainWindow() 
        {
            InitializeComponent();
            graphManager = new GraphManager(GraphCanvas); // Создание экземпляра GraphManager и привязка к Canvas
            graphManager.DrawAxis(); // Отрисовка осей на графике
            timer?.Stop(); // Остановка таймера, если он был запущен
            graphManager.ClearData(); // Очистка данных графика при запуске
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) 
        {
            Close();
        }

        private void BuildButton_Click(object sender, RoutedEventArgs e) 
        {
            if (timer == null) // Проверка, был ли уже создан таймер
            {
                timer = new DispatcherTimer // Создание нового таймера
                {
                    Interval = TimeSpan.FromSeconds(1) // Интервал таймера: 1 секунда
                };
                timer.Tick += (s, args) => graphManager.AddDataPoint(); // Добавление новой точки на график при каждом тике
            }
            timer.Start(); // Запуск таймера
        }

        private void StopButton_Click(object sender, RoutedEventArgs e) 
        {
            timer?.Stop(); 
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e) 
        {
            timer?.Start(); 
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e) 
        {
            timer?.Stop(); 
            graphManager.ClearData(); 
        }

        private void MetricsButton_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                // Получаем текущую директорию, где расположена сборка
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                // Переходим на уровень вверх до корневой папки проекта
                var projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
                // Находим все файлы с расширением .cs в проекте
                var csFiles = Directory.GetFiles(projectDirectory, "*.cs", SearchOption.AllDirectories);
                int totalLines = 0; // Переменная для подсчета общего количества строк кода
                StringBuilder resultMessage = new(); // Строка для накопления результатов
                // Подсчитываем строки кода для каждого файла
                foreach (string file in csFiles)
                {
                    var lineCount = File.ReadLines(file).Count(); // Считываем количество строк в файле
                    totalLines += lineCount; // Добавляем количество строк в общую сумму
                    resultMessage.AppendLine($"{Path.GetFileName(file)}: {lineCount} строк"); // Добавляем результат по файлу
                }
                // Добавляем итоговое количество строк
                resultMessage.AppendLine($"\nОбщее количество строк кода: {totalLines}");
                // Показываем сообщение с метриками в окне MessageBox
                MessageBox.Show(resultMessage.ToString(), "Метрика кода", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ошибка при вычислении метрик: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AboutButton_Click(object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("Программа для построения графика силы ветра на Эвересте", "О программе", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}