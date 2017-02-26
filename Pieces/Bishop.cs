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

            this.addToValidMoves(coords);
            this.FilterMovesIfChecked(checkingMove);
            
        }

        public void determineValidMoves(string coords, List<ValidBoardMove> filterMoves)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            this.addToValidMoves(coords);
            this.FilterMovesIfPinned(filterMoves);
        }

        public override void addToValidMoves(string coords)
        {
            var downLeftCells = getValidCellsDownLeft(coords);
            var downRightCells = getValidCellsDownRight(coords);
            var upLeftCells = getValidCellsUpLeft(coords);
            var upRightCells = getValidCellsUpRight(coords);

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