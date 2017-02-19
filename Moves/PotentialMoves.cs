namespace SalisburyChessEngine.Moves
{
    public class PotentialMoves
    {
        public string Coordinates { get; set; }
        public movePath? MovePath { get; set; }

        public enum movePath
        {
            Up = 1,
            Down = 2,
            Right = 3,
            Left = 4,
            UpLeft = 5,
            UpRight = 6,
            DownLeft = 7,
            DownRight = 8
        }

        public PotentialMoves(string coordinates, movePath? movepath)
        {
            this.Coordinates = coordinates;
            this.MovePath = movepath;
        }
    }
}