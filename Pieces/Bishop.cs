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
            this.EnemyKing = enemyKing;
            this.TypeOfPiece = PieceType.Bishop;
        }


        public override void DetermineValidMoves(string coords, ValidBoardMove checkingMove, List<ValidBoardMove> pinnedMoves)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            if (!this.ValidMovesSet)
            {
                this.AddToValidMoves(coords);
                this.FilterMovesIfChecked(checkingMove);
                this.FilterMovesIfPinned(pinnedMoves);
            }
            
        }
        public override void AddToValidMoves(string coords)
        {
            var downLeftCells = GetValidCellsDownLeft(coords);
            var downRightCells = GetValidCellsDownRight(coords);
            var upLeftCells = GetValidCellsUpLeft(coords);
            var upRightCells = GetValidCellsUpRight(coords);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);

            this.ValidMovesSet = true;
        }

        public override string ToString()
        {
            return "B";
        }
    }
}