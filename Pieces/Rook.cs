using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using System.Collections.ObjectModel;

namespace SalisburyChessEngine.Pieces
{
    public class Rook : PieceBase
    {
        public bool HasMoved { get; set;
        }
        public Rook(bool isWhite, Func<string, Cell> getCell, string coordinates, King enemyKing) : base(isWhite, coordinates, getCell)
        {
            this.getCell = getCell;
            this.TypeOfPiece = PieceType.Rook;
            this.EnemyKing = enemyKing;
        }
        

        public override void DetermineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            if (!this.ValidMovesSet)
            {
                ValidMoves = new List<ValidBoardMove>();
                PiecePressure = new List<ValidBoardMove>();
                this.ValidMovesSet = true;
                this.AddToValidMoves(coords);
                this.FilterMoves(checkingMove);
            }
            else
            {
                this.FilterMoves(checkingMove);
            }
        }
        private void FilterMoves(ValidBoardMove checkingMove)
        {
            if (checkingMove != null)
            {
                this.FilterMovesIfChecked(checkingMove);
            }
            if (this.PinnedMoves.Count > 0)
            {
                this.FilterMovesIfPinned();
            }
        }
        public override void AddToValidMoves(string coords)
        {
            var downCells = GetValidCellsDown(coords);
            var leftCells = GetValidCellsLeft(coords);
            var rightCells = GetValidCellsRight(coords);
            var upCells = GetValidCellsUp(coords);

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