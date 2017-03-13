using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using System.Collections.ObjectModel;

namespace SalisburyChessEngine.Pieces
{
    public class Queen : PieceBase
    {
        
        public Queen(bool isWhite, Func<string, Cell> getCell, string coordinates, King enemyKing) : base(isWhite, coordinates, getCell)
        {
            this.TypeOfPiece = PieceType.Queen;
            this.getCell = getCell;
            this.EnemyKing = enemyKing;
        }

        
        public override string ToString()
        {
            return "Q";
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
            var downCells = GetValidCellsDown(coords);
            var leftCells = GetValidCellsLeft(coords);
            var rightCells = GetValidCellsRight(coords);
            var upCells = GetValidCellsUp(coords);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

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
    }
}