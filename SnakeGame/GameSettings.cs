namespace SnakeGame
{
    public class GameSettings
    {
        private double snakeSpeed;

        public double SnakeSpeed
        {
            get => snakeSpeed;
            set
            {
                if (value < 1) snakeSpeed = 1;
                else if (value > 10) snakeSpeed = 10;
                else snakeSpeed = value;
            }
        }

        public GameSettings()
        {
            SnakeSpeed = 5; // Domyślna prędkość
        }

        // Funkcja zwracająca odpowiednie opóźnienie w zależności od prędkości węża
        public int GetDelay()
        {
            int baseDelay = 300; // Bazowy delay dla najwolniejszej prędkości
            int minDelay = 50;   // Minimalne opóźnienie dla najszybszej prędkości

            return baseDelay - (int)((SnakeSpeed - 1) * (baseDelay - minDelay) / 9);
        }
    }

}
