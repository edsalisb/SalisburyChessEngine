using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;

namespace SalisburyChessEngine.Pieces
{
    public abstract class PieceBase
    {
        public List<ValidBoardMove> ValidMoves { set; get; }
        public List<ValidBoardMove> PiecePressure { get; set; }
        public List<ValidBoardMove> allowedCellsAfterCheck { get; set; }
        public Cell pinnedCell { get; set; }

        public IReadOnlyDictionary<ValidBoardMove.movePath, List<ValidBoardMove.movePath>> pinnedAllowedMovePaths { get; } =
            new Dictionary<ValidBoardMove.movePath, List<ValidBoardMove.movePath>>
            {
                { ValidBoardMove.movePath.Up, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.Up, ValidBoardMove.movePath.Down } },
                { ValidBoardMove.movePath.Down, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.Up, ValidBoardMove.movePath.Down } },
                { ValidBoardMove.movePath.Left, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.Left, ValidBoardMove.movePath.Right } },
                { ValidBoardMove.movePath.Right, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.Left, ValidBoardMove.movePath.Right } },
                { ValidBoardMove.movePath.UpLeft, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.UpLeft, ValidBoardMove.movePath.DownRight } },
                { ValidBoardMove.movePath.UpRight, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.UpRight, ValidBoardMove.movePath.DownLeft } },
                { ValidBoardMove.movePath.DownLeft, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.UpLeft, ValidBoardMove.movePath.DownRight } },
                { ValidBoardMove.movePath.DownRight, new List<ValidBoardMove.movePath> {ValidBoardMove.movePath.UpRight, ValidBoardMove.movePath.DownLeft } },
            };


        public enum pieceType
        {
            Queen = 9,
            Bishop = 4,
            Knight = 3,
            Rook = 5,
            Pawn = 1,
            King = 0
        }
        public pieceType TypeOfPiece { get; set; }

        public string CurrentCoordinates { get; set; }
        public bool isWhite;
        public abstract void addToValidMoves(string coords);
        public abstract void determineValidMoves(string coords, ValidBoardMove checkingMove);
        public King enemyKing { get; set; }
        public bool ValidMovesSet { get; set; } = false;

        public Func<string, Cell> getCell;
        private Cell startingCell;

        public PieceBase(bool isWhite, string coordinates, Func<string, Cell> getCell)
        {
            this.getCell = getCell;
            this.CurrentCoordinates = coordinates;
            this.isWhite = isWhite;
            this.ValidMoves = new List<ValidBoardMove>();
            this.PiecePressure = new List<ValidBoardMove>();
            this.allowedCellsAfterCheck = new List<ValidBoardMove>();
        }

        public List<ValidBoardMove> executeFunctionOnCellsToLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            ValidBoardMove.movePath path = ValidBoardMove.movePath.Left;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + cell.Row);
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            continue;
                        }

                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            break;
                        }
                        moveList.Add(move);
                    }
                    
                }
            }
            return moveList;
        }

        public List<ValidBoardMove> executeFunctionOnCellsToRight(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            ValidBoardMove.movePath path = ValidBoardMove.movePath.Right;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + cell.Row);
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            continue;
                        }
                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            break;
                        }
                        moveList.Add(move);
                    }
                }
            }
            return moveList;
        }

        public List<ValidBoardMove> executeFunctionOnCellsUp(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            ValidBoardMove.movePath path = ValidBoardMove.movePath.Up;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.Row; i < BoardProperties.Rows; i++)
            {
                var sequentialCell = getCell(cell.columnLetter.ToString() + i.ToString());
                if (Cell.IsNotNull(sequentialCell))
                {
                    if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                    {
                        continue;
                    }
                    var move = func(cell, sequentialCell, path);
                    if (move == null)
                    {
                        break;
                    }
                    moveList.Add(move);
                }
            }
            return moveList;
        }
        public List<ValidBoardMove> executeFunctionOnCellsDown(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            ValidBoardMove.movePath path = ValidBoardMove.movePath.Down;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.Row; i > 0; i--)
            {
                var sequentialCell = getCell(cell.columnLetter.ToString() + i.ToString());
                if (Cell.IsNotNull(sequentialCell))
                {
                    if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                    {
                        continue;
                    }
                    var move = func(cell, sequentialCell, path);
                    if (move == null)
                    {
                        break;
                    }
                    moveList.Add(move);
                }
            }
            return moveList;
        }
        public List<ValidBoardMove> executeFunctionOnCellsDownLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.movePath path = ValidBoardMove.movePath.DownLeft;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            row--;
                            continue;
                        }

                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            row--;
                            break;
                        }
                        moveList.Add(move);
                    }

                }
                row--;
            }
            return moveList;
        }
        public List<ValidBoardMove> executeFunctionOnCellsDownRight(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.movePath path = ValidBoardMove.movePath.DownRight;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            row--;
                            continue;
                        }
                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            row--;
                            break;
                        }
                        moveList.Add(move);
                    }
                }
                row--;
            }
            return moveList;
        }
        public List<ValidBoardMove> executeFunctionOnCellsUpLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.movePath path = ValidBoardMove.movePath.UpLeft;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            row++;
                            continue;
                        }

                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            row++;
                            break;
                        }
                        moveList.Add(move);
                    }

                }
                row++;
            }
            return moveList;
        }
        public List<ValidBoardMove> executeFunctionOnCellsUpRight(Cell cell, Func<Cell, Cell, ValidBoardMove.movePath, ValidBoardMove> func)
        {
            ValidBoardMove.movePath path = ValidBoardMove.movePath.UpRight;
            var row = cell.Row;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                char columnLetter;
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.columnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            row++;
                            continue;
                        }
                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            row++;
                            break;
                        }
                        moveList.Add(move);
                    }
                }
                row++;
            }
            return moveList;
        }
        public List<ValidBoardMove> getValidCellsLeft(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsToLeft(startingCell, determineIfCellValid);
        }

        public List<ValidBoardMove> getValidCellsRight(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsToRight(startingCell, determineIfCellValid);
        }

        public List<ValidBoardMove> getValidCellsUp(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsUp(startingCell, determineIfCellValid);
        }
        public List<ValidBoardMove> getValidCellsDown(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsDown(startingCell, determineIfCellValid);
        }

        public List<ValidBoardMove> getValidCellsDownLeft(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsDownLeft(startingCell, determineIfCellValid);
        }

        
        public List<ValidBoardMove> getValidCellsDownRight(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsDownRight(startingCell, determineIfCellValid);
        }
        public List<ValidBoardMove> getValidCellsUpLeft(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsUpLeft(startingCell, determineIfCellValid);
        }
        public List<ValidBoardMove> getValidCellsUpRight(string coords)
        {
            var startingCell = getCell(coords);
            return executeFunctionOnCellsUpRight(startingCell, determineIfCellValid);
        }
        private ValidBoardMove determineIfCellValid(Cell fromCell, Cell toCell, ValidBoardMove.movePath path)
        {
            var validMoveProps = this.cellIsValidForPiece(fromCell, toCell);
            var moveProperties = new ValidBoardMove(fromCell.Coordinates, toCell.Coordinates, path, this.isWhite);

            if (validMoveProps.IsValid || validMoveProps.IsProtected)
            {
                this.PiecePressure.Add(moveProperties);
            }
            if (validMoveProps.IsPotentiallyPinned)
            {
                this.startingCell = fromCell;
                this.pinnedCell = toCell;
                DetermineIfAbsolutePinned(toCell, path);
            }
            if (validMoveProps.IsTerminatable)
            {
                return null;
            }
            //if (validMoveProps.IsValid)
            //{
            //    return moveProperties;
            //}
            return moveProperties;
        }

        private void DetermineIfAbsolutePinned(Cell pinnedCell,ValidBoardMove.movePath path)
        {
            List<ValidBoardMove> test = new List<ValidBoardMove>();
            switch (path)
            {
                case ValidBoardMove.movePath.Down:
                    test = executeFunctionOnCellsDown(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.Up:
                    test = executeFunctionOnCellsUp(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.Right:
                    test = executeFunctionOnCellsToRight(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.Left:
                    test = executeFunctionOnCellsToLeft(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.DownLeft:
                    test = executeFunctionOnCellsDownLeft(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.DownRight:
                    test = executeFunctionOnCellsDownRight(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.UpLeft:
                    test = executeFunctionOnCellsUpLeft(pinnedCell, determineIfCellPinned);
                    break;
                case ValidBoardMove.movePath.UpRight:
                    test = executeFunctionOnCellsUpRight(pinnedCell, determineIfCellPinned);
                    break;
                default:
                    break;
            }
            List<bool> arePinned = test.Select(x => x.IsPinningMove).Where(x => x == true).ToList();
            if (arePinned.Count == 1)
            {
                // we are absolutely pinned hgere
                var piece= Cell.GetPiece(this.pinnedCell);
                if (piece.TypeOfPiece == pieceType.Rook)
                {
                    var rook = (Rook)piece;
                    rook.determineValidMoves(rook.CurrentCoordinates, test);
                }
                else if (piece.TypeOfPiece == pieceType.Bishop)
                {
                    var bishop = (Bishop)piece;
                    bishop.determineValidMoves(bishop.CurrentCoordinates, test);
                }
                else if (piece.TypeOfPiece == pieceType.Pawn)
                {
                    var pawn = (Pawn)piece;
                    pawn.determineValidMoves(pawn.CurrentCoordinates, test);
                }
                else if (piece.TypeOfPiece == pieceType.King)
                {
                    throw new NotImplementedException();
                }
                else if (piece.TypeOfPiece == pieceType.Queen)
                {
                    var queen = (Queen)piece;
                    queen.determineValidMoves(queen.CurrentCoordinates, test);
                }
                else if (piece.TypeOfPiece == pieceType.Knight)
                {
                    var knight = (Knight)piece;
                    knight.determineValidMoves(knight.CurrentCoordinates, test);
                }
            }
        }

        private ValidBoardMove determineIfCellPinned(Cell pinnedCell, Cell potentialCell, ValidBoardMove.movePath path)
        {
            var piece = Cell.GetPiece(potentialCell);
            if (piece == enemyKing)
            {
                return new ValidBoardMove(pinnedCell.Coordinates, potentialCell.Coordinates, path, piece.isWhite, true);
            }
            else
            {
                if (piece != null)
                {
                    return new ValidBoardMove(pinnedCell.Coordinates, potentialCell.Coordinates, path, piece.isWhite);
                }
                return null;
            }
            
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

        public  List<ValidBoardMove> FindAttackPath(ValidBoardMove checkMove)
        {
            var returnList = new List<ValidBoardMove>();
            switch (checkMove.MovePath)
            {
                case ValidBoardMove.movePath.Down:
                    returnList = getValidCellsUp(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.Up:
                    returnList = getValidCellsDown(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.Left:
                    returnList = getValidCellsDownRight(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.Right:
                    returnList = getValidCellsLeft(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.UpLeft:
                    returnList = getValidCellsDownRight(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.UpRight:
                    returnList = getValidCellsDownLeft(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.DownLeft:
                    returnList = getValidCellsUpRight(checkMove.CoordinatesTo);
                    break;
                case ValidBoardMove.movePath.DownRight:
                    returnList = getValidCellsUpLeft(checkMove.CoordinatesTo);
                    break;
                default:
                    break;

            }
            return returnList.Where(move => MovesAreBetweenCheckMovePath(move, checkMove)).ToList();
        }

        private bool MovesAreBetweenCheckMovePath(ValidBoardMove move, ValidBoardMove checkMove)
        {
            var checkCoordsTo = checkMove.CoordinatesTo;
            var checkCoordsToColumnLetter = checkCoordsTo.getColumnLetter();
            var checkCoordsToRowNumber = checkCoordsTo.getRowNumber();

            var checkCoordsFrom = checkMove.CoordinatesFrom;
            var checkCoordsFromColumnLetter = checkCoordsFrom.getColumnLetter();
            var checkCoordsFromRowNumber = checkCoordsFrom.getRowNumber();

            var moveCoordsTo = move.CoordinatesTo;
            var moveColumnLetter = moveCoordsTo.getColumnLetter();
            var moveRowNumber = moveCoordsTo.getRowNumber();

            if (moveColumnLetter.isBetween<char?>(checkCoordsFromColumnLetter, checkCoordsToColumnLetter) &&
                moveRowNumber.isBetween<char?>(checkCoordsFromRowNumber, checkCoordsToRowNumber))
            {
                return true;
            }
            return false;


        }
        public void FilterMovesIfChecked(ValidBoardMove checkingMove)
        {
            if (checkingMove != null)
            {
                this.allowedCellsAfterCheck = FindAttackPath(checkingMove);

            }
            else
            {
                this.allowedCellsAfterCheck = new List<ValidBoardMove>();
            }
            
            if (this.allowedCellsAfterCheck.Count > 0 &&
                checkingMove.IsWhite != this.isWhite)
            {
                var filteredCoordinateList = this.ValidMoves.Select(GeneralUtilities.SelectCoordinates)
                    .Intersect(this.allowedCellsAfterCheck.Select(GeneralUtilities.SelectCoordinates)).ToList();

                this.ValidMoves = this.ValidMoves.Where(x => filteredCoordinateList.IndexOf(x.CoordinatesTo) > -1).ToList();
            }
        }

        public void FilterMovesIfPinned(List<ValidBoardMove> pinnedMoves)
        {
            List<ValidBoardMove.movePath> combinedMovePaths = new List<ValidBoardMove.movePath>();
            var movePaths = pinnedMoves.Select(move => move.MovePath).ToList();
            foreach(var path in movePaths)
            {
                List<ValidBoardMove.movePath> rangeToAdd;
                if (pinnedAllowedMovePaths.TryGetValue(path, out rangeToAdd))
                {
                    combinedMovePaths.AddRange(rangeToAdd);
                }
            }
            combinedMovePaths = combinedMovePaths.Distinct().ToList();
            this.ValidMoves = this.ValidMoves.Where(x => combinedMovePaths.IndexOf(x.MovePath) > -1).ToList();
        }
    }
}