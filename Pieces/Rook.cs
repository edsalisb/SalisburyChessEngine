using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using System.Collections.ObjectModel;

namespace SalisburyChessEngine.Pieces
{
    public class Rook : PieceBase
    {
        public Rook(bool isWhite, Func<string, Cell> getCell, string coordinates, King enemyKing) : base(isWhite, coordinates, getCell)
        {
            this.getCell = getCell;
            this.TypeOfPiece = pieceType.Rook;
            this.enemyKing = enemyKing;
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

           
            this.FilterMovesIfChecked(checkingMove);
            
        }

        public override string ToString()
        {
            return "R";
        }
    }
}