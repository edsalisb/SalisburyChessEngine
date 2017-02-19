﻿using System;
using System.Collections.Generic;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine.Pieces
{
    public class Queen : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        private Func<string, Cell> getCell;
        public override string CurrentCoordinates { get; set; }

        public Queen(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.TypeOfPiece = pieceType.Queen;
            this.getCell = getCell;
        }

        
        public override string ToString()
        {
            return "Q";
        }

        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<PotentialMoves>();
            var downCells = getValidCellsDown(coords, getCell);
            var leftCells = getValidCellsLeft(coords, getCell);
            var rightCells = getValidCellsRight(coords, getCell);
            var upCells = getValidCellsUp(coords, getCell);

            this.ValidMoves.AddRange(downCells);
            this.ValidMoves.AddRange(leftCells);
            this.ValidMoves.AddRange(rightCells);
            this.ValidMoves.AddRange(upCells);

            var downLeftCells = getValidCellsDownLeft(coords, getCell);
            var downRightCells = getValidCellsDownRight(coords, getCell);
            var upLeftCells = getValidCellsUpLeft(coords, getCell);
            var upRightCells = getValidCellsUpRight(coords, getCell);

            this.ValidMoves.AddRange(downLeftCells);
            this.ValidMoves.AddRange(downRightCells);
            this.ValidMoves.AddRange(upLeftCells);
            this.ValidMoves.AddRange(upRightCells);
        }
    }
}