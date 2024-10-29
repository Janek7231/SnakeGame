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

namespace SnakeGame
{
    public partial class MainWindow : Window
    {
        private GameSettings gameSettings;

        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake, Images.Body },
            {GridValue.Food, Images.Food }
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0 },
            {Direction.Right, 90 },
            {Direction.Down, 180 },
            {Direction.Left, 270 },
        };

        private int rows = 15, cols = 15;
        private Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;

        public MainWindow()
        {
            InitializeComponent();
            gameSettings = new GameSettings();
            gridImages = SetupGrid();
            gameState = new GameState(gameSettings.Rows, gameSettings.Cols);
            RowsText.Text = gameSettings.Rows.ToString();
            ColsText.Text = gameSettings.Cols.ToString();
        }

        private async Task RunGame()
        {
            rows = gameSettings.Rows;
            cols = gameSettings.Cols;

            ResetGrid();
            gameState = new GameState(gameSettings.Rows, gameSettings.Cols);
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            MenuGrid1.Visibility = Visibility.Collapsed;
            GameGridContainer.Visibility = Visibility.Visible;

            double snakeSpeed = SpeedSlider.Value;
            gameSettings.SnakeSpeed = snakeSpeed;

            gameRunning = true;
            await RunGame();
            gameRunning = false;

            MenuGrid.Visibility = Visibility.Visible;
            MenuGrid1.Visibility = Visibility.Visible;
            GameGridContainer.Visibility = Visibility.Collapsed;
        }

        private void IncreaseRowsButton_Click(object sender, RoutedEventArgs e)
        {
            gameSettings.Rows++;
            RowsText.Text = gameSettings.Rows.ToString();
            UpdateGrid();
        }

        private void DecreaseRowsButton_Click(object sender, RoutedEventArgs e)
        {
            gameSettings.Rows--;
            RowsText.Text = gameSettings.Rows.ToString();
            UpdateGrid();
        }

        private void IncreaseColsButton_Click(object sender, RoutedEventArgs e)
        {
            gameSettings.Cols++;
            ColsText.Text = gameSettings.Cols.ToString();
            UpdateGrid();
        }

        private void DecreaseColsButton_Click(object sender, RoutedEventArgs e)
        {
            gameSettings.Cols--;
            ColsText.Text = gameSettings.Cols.ToString();
            UpdateGrid();
        }


        private void MapsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            MenuGrid1.Visibility = Visibility.Collapsed;
            MapsMenuGrid.Visibility = Visibility.Visible;
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Collapsed;
            MenuGrid1.Visibility = Visibility.Collapsed;
            OptionsMenuGrid.Visibility = Visibility.Visible;
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuGrid.Visibility = Visibility.Visible;
            MenuGrid1.Visibility = Visibility.Visible;
            MapsMenuGrid.Visibility = Visibility.Collapsed;
            OptionsMenuGrid.Visibility = Visibility.Collapsed;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if(!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;

                MenuGrid.Visibility = Visibility.Visible;
                GameGridContainer.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left); break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right); break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up); break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down); break;
            }
        }

        private async Task GameLoop()
        {
            while (!gameState.GameOver)
            {
                int delay = gameSettings.GetDelay();
                await Task.Delay(delay);
                gameState.Move();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            rows = gameSettings.Rows;
            cols = gameSettings.Cols;

            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new  Point(0.5, 0.5)
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeHead();
            UpdateScore();
        }

        private void UpdateScore()
        {
            ScoreText.Text = $"SCORE {gameState.Score}";
        }

        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for(int c = 0;c < cols; c++)
                {
                    GridValue gridVal = gameState.Grid[r, c];
                    gridImages[r, c].Source = gridValToImage[gridVal];
                    gridImages[r, c].RenderTransform = Transform.Identity;

                }
            }
        }

        private void ResetGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    gridImages[r, c].Source = Images.Empty;
                    gridImages[r, c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void UpdateGrid()
        {
            GameGrid.Children.Clear();
            gridImages = SetupGrid();
        }

        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row,headPos.Col];
            image.Source = Images.Head;

            int rotation = dirToRotation[gameState.Direction];
            image.RenderTransform = new RotateTransform(rotation);  
        }

        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for(int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }

        }

        private async Task ShowCountDown()
        {
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}