using System;
using System.Collections.Generic;
using SalisburyChessEngine.Moves;
namespace SalisburyChessEngine.Pieces
{
    public class Bishop : PieceBase
    {
        private Func<string, Cell> getCell;
        public override pieceType TypeOfPiece { get; set; }
        public override string CurrentCoordinates { get; set; }

        public Bishop(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.TypeOfPiece = pieceType.Bishop;
            this.getCell = getCell;
        }
        

        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<PotentialMoves>();
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