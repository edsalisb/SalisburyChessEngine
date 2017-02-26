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
            this.TypeOfPiece = pieceType.Queen;
            this.getCell = getCell;
            this.enemyKing = enemyKing;
        }

        
        public override string ToString()
        {
            return "Q";
        }

        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            var downCells = getValidCellsDown(coords);
            var leftCells = getValidCellsLeft(coords);
            var rightCells = getValidCellsRight(coords);
            var upCells = getValidCellsUp(coords);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

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
    }
}