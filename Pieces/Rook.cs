using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using System.Collections.ObjectModel;

namespace SalisburyChessEngine.Pieces
{
    public class Rook : PieceBase
    {
        public bool hasMoved { get; set;
        }
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
            this.addToValidMoves(coords);

            this.FilterMovesIfChecked(checkingMove);
            
        }

        public void determineValidMoves(string coords, List<ValidBoardMove> filterCells)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            this.addToValidMoves(coords);
        }
        public override void addToValidMoves(string coords)
        {
            var downCells = getValidCellsDown(coords);
            var leftCells = getValidCellsLeft(coords);
            var rightCells = getValidCellsRight(coords);
            var upCells = getValidCellsUp(coords);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

            this.ValidMovesSet = true;
        }
        public override string ToString()
        {
            return "R";
        }
    }
}