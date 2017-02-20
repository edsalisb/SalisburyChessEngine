namespace SalisburyChessEngine.Board
{
    public class ValidBoardMove
    {
        public bool IsValid { get; set; }

        public bool IsCapturable { get; set; }
        public string CoordinatesFrom { get; set; }
        public string CoordinatesTo { get; set; }
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

        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, movePath? movepath)
        {
            this.CoordinatesTo = coordinatesTo;
            this.CoordinatesFrom = coordinatesFrom;
            this.MovePath = movepath;
        }

        public override string ToString()
        {
            return CoordinatesTo;
        }
    }
}