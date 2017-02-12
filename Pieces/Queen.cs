using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Queen : PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell;

        public Queen(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.piece = pieceType.Queen;
            this.getCell = getCell;
        }

        
        public override string ToString()
        {
            return "Q";
        }

        public override void determineValidMoves(string coords)
        {
            ValidMoves = new List<string>();
            var downCells = getValidCellsDown(coords, getCell);
            var leftCells = getValidCellsLeft(coords, getCell);
            var rightCells = getValidCellsRight(coords, getCell);
            var upCells = getValidCellsUp(coords, getCell);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

            var downLeftCells = getValidCellsDownLeft(coords, getCell);
            var downRightCells = getValidCellsDownRight(coords, getCell);
            var upLeftCells = getValidCellsUpLeft(coords, getCell);
            var upRightCells = getValidCellsUpRight(coords, getCell);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);
        }
    }
}