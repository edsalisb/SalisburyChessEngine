﻿using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public class ValidBoardMove
    {
        public string CoordinatesFrom { get; set; }
        public string CoordinatesTo { get; set; }
        public MovePath Path { get; set; }
        public bool? IsWhite { get; set; }
        public bool IsPinningMove { get; set; }

        public ValidNotationProperties MoveProperties { get; set; }
        public enum MovePath
        {
            Castle = 0,
            Up = 1,
            Down = 2,
            Right = 3,
            Left = 4,
            UpLeft = 5,
            UpRight = 6,
            DownLeft = 7,
            DownRight = 8,
            KnightMove = 10,
            Invalid = 99
        }

        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, MovePath movepath)
        {
            this.CoordinatesTo = coordinatesTo;
            this.CoordinatesFrom = coordinatesFrom;
            this.Path = movepath;
            this.IsPinningMove = false;
            this.MoveProperties = new ValidNotationProperties();
            this.IsWhite = null;
        }

        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, MovePath movepath, bool isWhite) :this(coordinatesFrom, coordinatesTo, movepath)
        {
            this.IsWhite = isWhite;
            this.CoordinatesTo = coordinatesTo;
            this.CoordinatesFrom = coordinatesFrom;
            this.Path = movepath;
            this.IsPinningMove = false;
            this.MoveProperties = new ValidNotationProperties();
        }
        public ValidBoardMove(string coordinatesFrom, string coordinatesTo, MovePath movepath, bool isWhite, bool pinningMove): this(coordinatesFrom, coordinatesTo, movepath, isWhite)
        {
            //TODO : add to Valid
            this.IsPinningMove = pinningMove;
            this.MoveProperties = new ValidNotationProperties(true);
        }

        public static MovePath DetermineMovePath(Cell cellFrom, Cell cellTo)
        {
            if (cellFrom.Row < cellTo.Row)
            {
                if (cellFrom.ColumnLetter < cellTo.ColumnLetter)
                {
                    return MovePath.UpRight;
                }
                else if (cellFrom.ColumnLetter > cellTo.ColumnLetter)
                {
                    return MovePath.UpLeft;
                }
                else
                {
                    return MovePath.Up;
                }
            }
            else if (cellFrom.Row > cellTo.Row)
            {
                if (cellFrom.ColumnLetter < cellTo.ColumnLetter)
                {
                    return MovePath.DownRight;
                }
                else if (cellFrom.ColumnLetter > cellTo.ColumnLetter)
                {
                    return MovePath.DownLeft;
                }
                else
                {
                    return MovePath.Down;
                }
            }
            else
            {
                if (cellFrom.ColumnLetter < cellTo.ColumnLetter)
                {
                    return MovePath.Right;
                }
                else if (cellFrom.ColumnLetter > cellTo.ColumnLetter)
                {
                    return MovePath.Left;
                }
                else
                {
                    return MovePath.Invalid;
                }
            }
        }

        public override string ToString()
        {
            return CoordinatesTo;
        }
        
    }
}