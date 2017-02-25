using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
namespace SalisburyChessEngine.Pieces
{
    public class Bishop : PieceBase
    {
        private Func<string, Cell> getCell;
        public Bishop(bool isWhite, Func<string, Cell> getCell, string coordinates) : base(isWhite, coordinates)
        {
            this.TypeOfPiece = pieceType.Bishop;
            this.getCell = getCell;
        }


        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            var downLeftCells = getValidCellsDownLeft(coords, getCell);
            var downRightCells = getValidCellsDownRight(coords, getCell);
            var upLeftCells = getValidCellsUpLeft(coords, getCell);
            var upRightCells = getValidCellsUpRight(coords, getCell);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);

            
            this.FilterMovesIfChecked(checkingMove, getCell);
            
        }

        public override string ToString()
        {
            return "B";
        }
    }
}