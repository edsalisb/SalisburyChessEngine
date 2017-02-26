using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public class ValidBoardMove
    {
        public string CoordinatesFrom { get; set; }
        public string CoordinatesTo { get; set; }
        public movePath MovePath { get; set; }
        public bool IsWhite { get; set; }
        public bool IsPinningMove { get; set; }
        public enum movePath
        {
            Up = 1,
            Down = 2,
            Right = 3,
            Left = 4,
            UpLeft = 5,
            UpRight = 6,
            DownLeft = 7,
            DownRight = 8,
            Invalid = 99
        }

        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, movePath movepath, bool isWhite)
        {
            this.IsWhite = isWhite;
            this.CoordinatesTo = coordinatesTo;
            this.CoordinatesFrom = coordinatesFrom;
            this.MovePath = movepath;
            this.IsPinningMove = false;
        }
        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, movePath movepath, bool isWhite, bool pinningMove): this(coordinatesFrom, coordinatesTo, movepath, isWhite)
        {
            this.IsPinningMove = pinningMove;
        }

        public override string ToString()
        {
            return CoordinatesTo;
        }
        
    }
}