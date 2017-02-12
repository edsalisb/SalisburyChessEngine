using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Rook : PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell;

        public Rook(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.getCell = getCell;
            this.piece = pieceType.Rook;
        }
        

        public override void determineValidMoves(string coords)
        {
            var downCells = getValidCellsDown(coords, getCell);
            var leftCells = getValidCellsLeft(coords, getCell);
            var rightCells = getValidCellsRight(coords, getCell);
            var upCells = getValidCellsUp(coords, getCell);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

        }

        public override string ToString()
        {
            return "R";
        }
    }
}