using InflationAnalyzer.Models;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace InflationAnalyzer
{
    public partial class MainWindow : Window
    {
        // Список данных по инфляции
        private List<InflationData> inflationData =
            new List<InflationData>();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Загрузка CSV файла
        private void LoadButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            // Путь к CSV файлу
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Data",
                "inflation.csv");

            // Проверка существования файла
            if (!File.Exists(filePath))
            {
                MessageBox.Show(
                    "CSV file not found!",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            // Очистка списка
            inflationData.Clear();

            // Чтение данных из CSV
            var lines = File.ReadAllLines(filePath).Skip(1);

            foreach (var line in lines)
            {
                var parts = line.Split(',');

                inflationData.Add(new InflationData
                {
                    Year = int.Parse(parts[0]),

                    Inflation = double.Parse(
                        parts[1],
                        CultureInfo.InvariantCulture)
                });
            }

            // Вывод данных в таблицу
            InflationGrid.ItemsSource = inflationData;

            // Построение графика
            DrawChart();

            MessageBox.Show(
                "Data loaded successfully!",
                "Information",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // Метод построения графика
        private void DrawChart()
        {
            // Создание модели графика
            var plotModel = new PlotModel
            {
                Title = "Inflation Dynamics"
            };

            // Создание линии графика
            var series = new LineSeries
            {
                Title = "Inflation (%)"
            };

            // Добавление точек на график
            foreach (var item in inflationData)
            {
                series.Points.Add(
                    new DataPoint(
                        item.Year,
                        item.Inflation));
            }

            // Добавление линии
            plotModel.Series.Add(series);

            // Отображение графика
            PlotView.Model = plotModel;
        }

        // Метод прогнозирования инфляции
        private void ForecastButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            // Проверка загрузки данных
            if (inflationData.Count == 0)
            {
                MessageBox.Show(
                    "Load CSV first!");

                return;
            }

            // Последнее значение инфляции
            var lastValue = inflationData.Last();

            // Простейший прогноз
            double forecast =
                lastValue.Inflation + 0.5;

            // Вывод прогноза
            MessageBox.Show(
                $"Forecast inflation for " +
                $"{lastValue.Year + 1}: " +
                $"{forecast:F1}%",

                "Forecast");
        }
    }
}