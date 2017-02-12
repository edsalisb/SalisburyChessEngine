using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Bishop : PieceBase
    {
        private Func<string, Cell> getCell;
        public pieceType piece;

        public Bishop(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.piece = pieceType.Bishop;
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