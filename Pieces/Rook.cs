using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Rook : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }

        private Func<string, Cell> getCell;

        public Rook(bool isWhite, Func<string, Cell> getCell, string coordinates) : base(isWhite, coordinates)
        {
            this.getCell = getCell;
            this.TypeOfPiece = pieceType.Rook;
        }
        

        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<ValidBoardMove>();

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