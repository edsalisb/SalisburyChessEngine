using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine.Pieces
{
    public abstract class PieceBase
    {
        public List<ValidBoardMove> ValidMoves{ set; get; }
        public enum pieceType
        {
            Queen = 9,
            Bishop = 4,
            Knight = 3,
            Rook = 5,
            Pawn = 1,
            King = 0
        }

        public string CurrentCoordinates { get; set; }
        public abstract pieceType TypeOfPiece { get; set; }
        public bool isWhite;
        public abstract void determineValidMoves(string coords, bool isChecked);

        public PieceBase(bool isWhite, string coordinates)
        {
            this.isWhite = isWhite;
            this.CurrentCoordinates = coordinates;
            this.ValidMoves = new List<ValidBoardMove>();
        }
        public List<ValidBoardMove> getValidCellsLeft(string coords, Func<string,Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellToLeft.Coordinates, ValidBoardMove.movePath.Left);
                        moveList.Add(moveProperties);
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
            return moveList;
        }
        public List<ValidBoardMove> getValidCellsRight(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellToRight.Coordinates, ValidBoardMove.movePath.Right);
                        moveList.Add(moveProperties);
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
            return moveList;
        }

        public List<ValidBoardMove> getValidCellsUp(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                    var moveProperties = new ValidBoardMove(coords, cellUp.Coordinates, ValidBoardMove.movePath.Up);
                    moveList.Add(moveProperties);
                }
                else
                {
                    if (validMoveProps.IsTerminatable)
                    {
                        break;
                    }
                }

            }
            return moveList;
        }
        public List<ValidBoardMove> getValidCellsDown(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                    var moveProperties = new ValidBoardMove(coords, cellDown.Coordinates, ValidBoardMove.movePath.Down);
                    moveList.Add(moveProperties);
                }
                else
                {
                    if (validMoveProps.IsTerminatable)
                    {
                        break;
                    }
                }

            }
            return moveList;
        }

        public List<ValidBoardMove> getValidCellsDownLeft(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellUpLeft.Coordinates, ValidBoardMove.movePath.UpLeft);
                        moveList.Add(moveProperties);
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
            return moveList;
        }

        
        public List<ValidBoardMove> getValidCellsDownRight(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellDownRight.Coordinates, ValidBoardMove.movePath.DownRight);
                        moveList.Add(moveProperties);
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
            return moveList;
        }
        public List<ValidBoardMove> getValidCellsUpLeft(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellUpLeft.Coordinates, ValidBoardMove.movePath.UpLeft);
                        moveList.Add(moveProperties);
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
            return moveList;
        }
        public List<ValidBoardMove> getValidCellsUpRight(string coords, Func<string, Cell> getCell)
        {
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
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
                        var moveProperties = new ValidBoardMove(coords, cellUpRight.Coordinates, ValidBoardMove.movePath.UpRight);
                        moveList.Add(moveProperties);
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
            return moveList;
        }

        public ValidNotationProperties cellIsValidForPiece(Cell fromCell, Cell toCell)
        {
            var validMoveProps = new ValidNotationProperties();
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