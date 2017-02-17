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
            Bishop = 4,
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
                    if (cellToLeft == null)
                    {
                        break;
                    }
                    if (cellToLeft.columnLetter + cellToLeft.Row.ToString() == coords)
                    {
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellToLeft);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellToLeft.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
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
                    if (cellToRight == null)
                    {
                        break;
                    }
                    if (cellToRight.columnLetter + cellToRight.Row.ToString() == coords)
                    {
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellToRight);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellToRight.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
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
                
                var cellUp = getCell(startingCell.columnLetter.ToString() + i.ToString());
                if (cellUp == null)
                {
                    break;
                }
                if (cellUp.columnLetter + cellUp.Row.ToString() == coords)
                {
                    continue;
                }
                var validMoveProps = this.cellIsValidForPiece(startingCell, cellUp);
                if (validMoveProps.IsValid)
                {
                    cellList.Add(cellUp.Coordinates);
                }
                else
                {
                    if (validMoveProps.IsTerminatable)
                    {
                        break;
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
                
                var cellDown = getCell(startingCell.columnLetter.ToString() + i.ToString());

                if (cellDown == null)
                {
                    break;
                }
                if (cellDown.columnLetter + cellDown.Row.ToString() == coords)
                {
                    continue;
                }
                var validMoveProps = this.cellIsValidForPiece(startingCell, cellDown);
                if (validMoveProps.IsValid)
                {
                    cellList.Add(cellDown.Coordinates);
                }
                else
                {
                    if (validMoveProps.IsTerminatable)
                    {
                        break;
                    }
                }

            }
            return cellList;
        }

        public List<string> getValidCellsDownLeft(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            var row = startingCell.Row;
            for (var i = startingCell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellUpLeft = getCell(columnLetter.ToString() + row.ToString());
                    if (cellUpLeft == null)
                    {
                        break;
                    }
                    if (cellUpLeft.columnLetter + cellUpLeft.Row.ToString() == coords)
                    {
                        row--;
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellUpLeft);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellUpLeft.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
                    }
                }

                row--;
            }
            return cellList;
        }

        
        public List<string> getValidCellsDownRight(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            var row = startingCell.Row;
            for (var i = startingCell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellDownRight = getCell(columnLetter.ToString() + row.ToString());
                    if (cellDownRight == null)
                    {
                        break;
                    }
                    if (cellDownRight.columnLetter + cellDownRight.Row.ToString() == coords)
                    {
                        row--;
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellDownRight);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellDownRight.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
                    }
                }

                row--;
            }
            return cellList;
        }
        public List<string> getValidCellsUpLeft(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            var row = startingCell.Row;
            for (var i = startingCell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellUpLeft = getCell(columnLetter.ToString() + row.ToString());
                    if (cellUpLeft == null)
                    {
                        break;
                    }
                    if (cellUpLeft.columnLetter + cellUpLeft.Row.ToString() == coords)
                    {
                        row++;
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellUpLeft);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellUpLeft.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
                    }
                }

                row++;
            }
            return cellList;
        }
        public List<string> getValidCellsUpRight(string coords, Func<string, Cell> getCell)
        {
            List<string> cellList = new List<string>();
            var startingCell = getCell(coords);
            var row = startingCell.Row;
            for (var i = startingCell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var cellUpRight = getCell(columnLetter.ToString() + row.ToString());
                    if (cellUpRight == null)
                    {
                        break;
                    }
                    if (cellUpRight.columnLetter + cellUpRight.Row.ToString() == coords)
                    {
                        row++;
                        continue;
                    }
                    var validMoveProps = this.cellIsValidForPiece(startingCell, cellUpRight);
                    if (validMoveProps.IsValid)
                    {
                        cellList.Add(cellUpRight.Coordinates);
                    }
                    else
                    {
                        if (validMoveProps.IsTerminatable)
                        {
                            break;
                        }
                    }
                }

                row++;
            }
            return cellList;
        }

        public ValidMoveProperties cellIsValidForPiece(Cell fromCell, Cell toCell)
        {
            var validMoveProps = new ValidMoveProperties();
            return validMoveProps.determineMoveProperties(fromCell, toCell);
        }

        public char? getColumnLetter(Cell currentCell, int spacesAway)
        {
            char columnletter;
            if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(currentCell.ColumnNumber + spacesAway, out columnletter))
            {
                return columnletter;
            }
            return null;
        }
    }
}