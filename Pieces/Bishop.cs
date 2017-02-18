using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class Bishop : PieceBase
    {
        private Func<string, Cell> getCell;
        public override pieceType TypeOfPiece { get; set; }

        public Bishop(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.TypeOfPiece = pieceType.Bishop;
            this.getCell = getCell;
        }
        

        public override void determineValidMoves(string coords)
        {
            ValidMoves = new List<string>();
            var downLeftCells = getValidCellsDownLeft(coords, getCell);
            var downRightCells = getValidCellsDownRight(coords, getCell);
            var upLeftCells = getValidCellsUpLeft(coords, getCell);
            var upRightCells = getValidCellsUpRight(coords, getCell);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);
        }

        public override string ToString()
        {
            return "B";
        }
    }
}