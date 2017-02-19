using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class Rook : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        public override string CurrentCoordinates { get; set; }

        private Func<string, Cell> getCell;

        public Rook(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.getCell = getCell;
            this.TypeOfPiece = pieceType.Rook;
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

        }

        public override string ToString()
        {
            return "R";
        }
    }
}