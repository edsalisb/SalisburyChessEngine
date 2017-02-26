using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using System.Collections.ObjectModel;

namespace SalisburyChessEngine.Pieces
{
    public class Bishop : PieceBase
    { 
      
        public Bishop(bool isWhite, Func<string, Cell> getCell, string coordinates, King enemyKing) : base(isWhite, coordinates, getCell)
        {
            this.enemyKing = enemyKing;
            this.TypeOfPiece = pieceType.Bishop;
        }


        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            var downLeftCells = getValidCellsDownLeft(coords);
            var downRightCells = getValidCellsDownRight(coords);
            var upLeftCells = getValidCellsUpLeft(coords);
            var upRightCells = getValidCellsUpRight(coords);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);

            
            this.FilterMovesIfChecked(checkingMove);
            
        }

        public override string ToString()
        {
            return "B";
        }
    }
}