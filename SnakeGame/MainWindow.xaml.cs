using System.Windows;

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private GameSettings gameSettings;

        public MainWindow()
        {
            InitializeComponent();
            gameSettings = new GameSettings();

            MenuControl.StartGameEvent += StartGame;
            MenuControl.MapsEvent += ShowMaps;
            MenuControl.OptionsEvent += ShowOptions;
            MenuControl.ExitEvent += ExitGame;
        }

        private void StartGame(double snakeSpeed)
        {
            gameSettings.SnakeSpeed = snakeSpeed;
            GameControl.SetGameSettings(gameSettings);

            MenuControl.Visibility = Visibility.Collapsed;
            GameControl.Visibility = Visibility.Visible;
            GameControl.RunGame();
        }

        private void ShowMaps()
        {
            // Logika do wyświetlenia map
        }

        private void ShowOptions()
        {
            // Logika do wyświetlenia opcji
        }

        private void ExitGame()
        {
            Application.Current.Shutdown();
        }
    }
}
