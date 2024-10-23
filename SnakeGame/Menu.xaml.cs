using System.Windows;
using System.Windows.Controls;

namespace SnakeGame
{
    public partial class Menu : UserControl
    {
        public event Action<double> StartGameEvent;
        public event Action MapsEvent;
        public event Action OptionsEvent;
        public event Action ExitEvent;

        public Menu()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            double snakeSpeed = SpeedSlider.Value;
            StartGameEvent?.Invoke(snakeSpeed);  // Wywołaj event startu gry
        }

        private void MapsButton_Click(object sender, RoutedEventArgs e)
        {
            MapsEvent?.Invoke();  // Wywołaj event otwarcia map
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            OptionsEvent?.Invoke();  // Wywołaj event otwarcia opcji
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            ExitEvent?.Invoke();  // Wywołaj event zamknięcia aplikacji
        }
    }
}
