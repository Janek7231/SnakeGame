namespace SnakeGame
{
    public class GameSettings
    {
        private double snakeSpeed;
        private int cols;
        private int rows;

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

        public int Cols
        {
            get => cols;
            set => cols = Math.Clamp(value, 10, 30); // min 10, max 30
        }

        public int Rows
        {
            get => rows;
            set => rows = Math.Clamp(value, 10, 30);
        }


        public GameSettings()
        {
            // Domyslne parametry
            SnakeSpeed = 5;
            Cols = 18;
            Rows = 10;
        }

        
        public int GetDelay()
        {
            int baseDelay = 300; // Bazowy delay dla najwolniejszej prędkości
            int minDelay = 50;   // Minimalne opóźnienie dla najszybszej prędkości

            return baseDelay - (int)((SnakeSpeed - 1) * (baseDelay - minDelay) / 9);
        }
    }

}
