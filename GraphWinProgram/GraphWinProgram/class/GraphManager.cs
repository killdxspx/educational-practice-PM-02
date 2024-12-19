using System; 
using System.Collections.Generic; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media; 
using System.Windows.Shapes; 

namespace GraphWinProgram
{
    public class GraphManager // Класс для управления графиком
    {
        private const int CanvasWidth = 760; // Ширина канвы (не используется напрямую)
        private const int CanvasHeight = 400; // Высота канвы (не используется напрямую)
        private const int MaxPoints = 50; // Максимальное количество точек на графике
        private readonly Canvas _canvas; // Ссылка на канву, на которой рисуется график
        private readonly List<double> _windData = new(); // Список для хранения данных о силе ветра
        private readonly Random _random = new(); // Генератор случайных чисел для создания данных

        // Конструктор принимает канву, к которой привязан график
        public GraphManager(Canvas canvas)
        {
            _canvas = canvas; // Присваиваем переданную канву внутреннему полю
        }

        // Метод очищает данные и график
        public void ClearData()
        {
            _windData.Clear(); // Очищаем список данных
            _canvas.Children.Clear(); // Очищаем все элементы на канве
        }

        // Метод добавляет новую случайную точку данных на график
        public void AddDataPoint()
        {
            double newWindSpeed = _random.Next(0, 101); // Генерируем случайное значение ветра от 0 до 100
            _windData.Add(newWindSpeed); // Добавляем новую точку в список данных
            // Ограничиваем количество точек на графике
            if (_windData.Count > MaxPoints)
            {
                _windData.RemoveAt(0); // Удаляем самую старую точку
            }
            DrawDataPoints(); // Перерисовываем график с новыми данными
        }

        // Метод рисует оси графика и подписи
        public void DrawAxis()
        {
            _canvas.Children.Clear(); // Очищаем канву перед отрисовкой осей
            // Рисуем ось X
            Line xAxis = new()
            {
                X1 = 10, // Начальная точка X1
                Y1 = _canvas.ActualHeight - 10, // Начальная точка Y1
                X2 = _canvas.ActualWidth - 10, // Конечная точка X2
                Y2 = _canvas.ActualHeight - 10, // Конечная точка Y2
                Stroke = Brushes.Gray, // Цвет оси
                StrokeThickness = 1 // Толщина линии
            };
            _canvas.Children.Add(xAxis); // Добавляем ось X на канву
            // Рисуем ось Y
            Line yAxis = new()
            {
                X1 = 10, // Начальная точка X1
                Y1 = 10, // Начальная точка Y1
                X2 = 10, // Конечная точка X2
                Y2 = _canvas.ActualHeight - 10, // Конечная точка Y2
                Stroke = Brushes.Gray, // Цвет оси
                StrokeThickness = 1 // Толщина линии
            };
            _canvas.Children.Add(yAxis); // Добавляем ось Y на канву
            // Добавляем подписи для оси X
            for (int i = 0; i <= MaxPoints; i += 10)
            {
                double x = 10 + i * (_canvas.ActualWidth - 20) / MaxPoints; // Позиция подписи на оси X
                TextBlock labelX = new()
                {
                    Text = i.ToString(), // Значение подписи
                    Foreground = Brushes.White, // Цвет текста
                    FontSize = 10 // Размер шрифта
                };
                // Устанавливаем позицию подписи
                Canvas.SetLeft(labelX, x - 10);
                Canvas.SetTop(labelX, _canvas.ActualHeight - 25);
                _canvas.Children.Add(labelX); // Добавляем подпись на канву
            }
            // Добавляем подписи для оси Y
            for (int i = 0; i <= 100; i += 25)
            {
                double y = _canvas.ActualHeight - 10 - i * (_canvas.ActualHeight - 20) / 100; // Позиция подписи на оси Y
                TextBlock labelY = new()
                {
                    Text = i.ToString(), // Значение подписи
                    Foreground = Brushes.White, // Цвет текста
                    FontSize = 10 // Размер шрифта
                };
                // Устанавливаем позицию подписи
                Canvas.SetLeft(labelY, 0);
                Canvas.SetTop(labelY, y - 10);
                _canvas.Children.Add(labelY); // Добавляем подпись на канву
            }
            // Подпись для оси X
            TextBlock titleX = new()
            {
                Text = "Время (секунды)", // Текст подписи
                Foreground = Brushes.White, // Цвет текста
                FontSize = 12, // Размер шрифта
                FontWeight = FontWeights.Bold // Жирное начертание
            };
            Canvas.SetLeft(titleX, _canvas.ActualWidth / 2 - 50); // Позиция текста
            Canvas.SetTop(titleX, _canvas.ActualHeight - 20);
            _canvas.Children.Add(titleX);
            // Подпись для оси Y
            TextBlock titleY = new()
            {
                Text = "Сила ветра", // Текст подписи
                Foreground = Brushes.White, // Цвет текста
                FontSize = 12, // Размер шрифта
                FontWeight = FontWeights.Bold, // Жирное начертание
                RenderTransform = new RotateTransform(-90) // Поворот текста на 90 градусов
            };
            Canvas.SetLeft(titleY, 5); // Позиция текста
            Canvas.SetTop(titleY, _canvas.ActualHeight / 2 - 50);
            _canvas.Children.Add(titleY);
        }

        // Метод отрисовывает точки данных на графике
        private void DrawDataPoints()
        {
            _canvas.Children.Clear(); // Очищаем канву
            DrawAxis(); // Перерисовываем оси
            // Создаём линию для соединения точек
            Polyline polyline = new()
            {
                Stroke = Brushes.White, // Цвет линии
                StrokeThickness = 2 // Толщина линии
            };
            // Масштабируем данные для отображения на канве
            double xScale = (_canvas.ActualWidth - 20) / MaxPoints; // Шаг по оси X
            double yScale = (_canvas.ActualHeight - 20) / 100; // Масштаб по оси Y
            // Добавляем точки данных в линию
            for (int i = 0; i < _windData.Count; i++)
            {
                double x = 10 + i * xScale; // Позиция X
                double y = _canvas.ActualHeight - 10 - _windData[i] * yScale; // Позиция Y
                polyline.Points.Add(new Point(x, y)); // Добавляем точку
            }
            _canvas.Children.Add(polyline); // Добавляем линию на канву
        }
    }
}
