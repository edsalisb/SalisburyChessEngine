using System;
using System.Collections.Generic;
using SalisburyChessEngine;

namespace SalisburyChessEngine.Pieces
{
    public abstract class PieceBase
    {
        public List<string> ValidMoves{ set; get; }
        public enum pieceType
        {
            Queen = 9,
            Bishop = 3,
            Knight = 3,
            Rook = 5,
            Pawn = 1,
            King = 0
        }
        
        public bool isWhite;
        public abstract void determineValidMoves(string coords);

        public PieceBase(bool isWhite)
        {
            this.isWhite = isWhite;
            this.ValidMoves = new List<string>();
        }
        public List<string> getValidCellsLeft(string coords, Func<string,Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            for (var i = startingCell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellToLeft = getCell(columnLetter.ToString() + startingCell.Row);
                    if (this.cellIsValid(startingCell, cellToLeft))
                    {
                        cellList.Add(cellToLeft.Coordinates);
                    }
                }
            }
            return cellList;
        }
        public List<string> getValidCellsRight(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            for (var i = startingCell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellToRight = getCell(columnLetter.ToString() + startingCell.Row);
                    if (this.cellIsValid(startingCell, cellToRight))
                    {
                        cellList.Add(cellToRight.Coordinates);
                    }
                }
            }
            return cellList;
        }

        public List<string> getValidCellsUp(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            for (var i = startingCell.Row; i < BoardProperties.Rows; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellUp = getCell(columnLetter.ToString() + startingCell.Row);
                    if (this.cellIsValid(startingCell, cellUp))
                    {
                        cellList.Add(cellUp.Coordinates);
                    }
                }
            }
            return cellList;
        }
        public List<string> getValidCellsDown(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            for (var i = startingCell.Row; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellDown = getCell(columnLetter.ToString() + startingCell.Row);
                    if (cellIsValid(startingCell,cellDown))
                    {
                        cellList.Add(cellDown.Coordinates);
                    }
                }
            }
            return cellList;
        }

        public bool cellIsValid(Cell fromCell, Cell toCell)
        {
            if (fromCell == null)
            {
                return false;
            }
            if (toCell == null)
            {
                return false;
            }
            if (toCell.CurrentPiece == null)
            {
                return true;
            }

            if (fromCell.CurrentPiece.isWhite != toCell.CurrentPiece.isWhite)
            {
                return true;
            }
            return false;
        }
    }
}