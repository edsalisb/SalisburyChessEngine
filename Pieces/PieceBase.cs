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
        public List<ValidBoardMove> AllowedCellsAfterCheck { get; set; }
        public Cell PinnedCell { get; set; }

        public Dictionary<ValidBoardMove.MovePath, List<ValidBoardMove.MovePath>> PinnedAllowedMovePaths { get; } =
            new Dictionary<ValidBoardMove.MovePath, List<ValidBoardMove.MovePath>>
            {
                { ValidBoardMove.MovePath.Up, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.Up, ValidBoardMove.MovePath.Down } },
                { ValidBoardMove.MovePath.Down, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.Up, ValidBoardMove.MovePath.Down } },
                { ValidBoardMove.MovePath.Left, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.Left, ValidBoardMove.MovePath.Right } },
                { ValidBoardMove.MovePath.Right, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.Left, ValidBoardMove.MovePath.Right } },
                { ValidBoardMove.MovePath.UpLeft, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.UpLeft, ValidBoardMove.MovePath.DownRight } },
                { ValidBoardMove.MovePath.UpRight, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.UpRight, ValidBoardMove.MovePath.DownLeft } },
                { ValidBoardMove.MovePath.DownLeft, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.UpLeft, ValidBoardMove.MovePath.DownRight } },
                { ValidBoardMove.MovePath.DownRight, new List<ValidBoardMove.MovePath> {ValidBoardMove.MovePath.UpRight, ValidBoardMove.MovePath.DownLeft } },
            };


        public enum PieceType
        {
            Queen = 9,
            Bishop = 4,
            Knight = 3,
            Rook = 5,
            Pawn = 1,
            King = 0
        }
        public PieceType TypeOfPiece { get; set; }

        public string CurrentCoordinates { get; set; }
        public bool isWhite;
        public abstract void AddToValidMoves(string coords);
        public abstract void DetermineValidMoves(string coords, ValidBoardMove checkingMove, List<ValidBoardMove> pinnedMoves);
        public King EnemyKing { get; set; }
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
            this.AllowedCellsAfterCheck = new List<ValidBoardMove>();
        }

        public List<ValidBoardMove> ExecuteFunctionOnCellsToLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.Left;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + cell.Row);
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            continue;
                        }

                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            break;
                        }
                        moveList.Add(move);
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }
                    
                }
            }
            return moveList;
        }

        public List<ValidBoardMove> ExecuteFunctionOnCellsToRight(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.Right;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + cell.Row);
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            continue;
                        }
                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            break;
                        }
                        moveList.Add(move);
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }
                }
            }
            return moveList;
        }

        public List<ValidBoardMove> ExecuteFunctionOnCellsUp(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.Up;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.Row; i <= BoardProperties.Rows; i++)
            {
                var sequentialCell = getCell(cell.ColumnLetter.ToString() + i.ToString());
                if (Cell.IsNotNull(sequentialCell))
                {
                    if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                    {
                        continue;
                    }
                    var move = func(cell, sequentialCell, path);
                    if (move == null)
                    {
                        break;
                    }
                    moveList.Add(move);
                    if (move.MoveProperties.IsTerminatable)
                    {
                        break;
                    }
                }
            }
            return moveList;
        }
        public List<ValidBoardMove> ExecuteFunctionOnCellsDown(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.Down;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.Row; i > 0; i--)
            {
                var sequentialCell = getCell(cell.ColumnLetter.ToString() + i.ToString());
                if (Cell.IsNotNull(sequentialCell))
                {
                    if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                    {
                        continue;
                    }
                    var move = func(cell, sequentialCell, path);
                    if (move == null)
                    {
                        break;
                    }

                    moveList.Add(move);
                    if (move.MoveProperties.IsTerminatable)
                    {
                        break;
                    }
                }
            }
            return moveList;
        }
        public List<ValidBoardMove> ExecuteFunctionOnCellsDownLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.DownLeft;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
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
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }

                }
                row--;
            }
            return moveList;
        }
        public List<ValidBoardMove> ExecuteFunctionOnCellsDownRight(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.DownRight;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
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
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }
                }
                row--;
            }
            return moveList;
        }
        public List<ValidBoardMove> ExecuteFunctionOnCellsUpLeft(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            var row = cell.Row;
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.UpLeft;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i > 0; i--)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
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
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }

                }
                row++;
            }
            return moveList;
        }
        public List<ValidBoardMove> ExecuteFunctionOnCellsUpRight(Cell cell, Func<Cell, Cell, ValidBoardMove.MovePath, ValidBoardMove> func)
        {
            ValidBoardMove.MovePath path = ValidBoardMove.MovePath.UpRight;
            var row = cell.Row;
            List<ValidBoardMove> moveList = new List<ValidBoardMove>();
            for (var i = cell.ColumnNumber; i <= BoardProperties.Columns; i++)
            {
                if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(i, out char columnLetter))
                {
                    var sequentialCell = getCell(columnLetter.ToString() + row.ToString());
                    if (Cell.IsNotNull(sequentialCell))
                    {
                        if (sequentialCell.ColumnLetter + sequentialCell.Row.ToString() == cell.Coordinates)
                        {
                            row++;
                            continue;
                        }
                        var move = func(cell, sequentialCell, path);
                        if (move == null)
                        {
                            row++;
                            continue;
                        }
                        
                        moveList.Add(move);
                        if (move.MoveProperties.IsTerminatable)
                        {
                            break;
                        }
                    }
                }
                row++;
            }
            return moveList;
        }
        public List<ValidBoardMove> GetValidCellsLeft(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsToLeft(startingCell, DetermineIfCellValid);
        }

        public List<ValidBoardMove> GetValidCellsRight(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsToRight(startingCell, DetermineIfCellValid);
        }

        public List<ValidBoardMove> GetValidCellsUp(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsUp(startingCell, DetermineIfCellValid);
        }
        public List<ValidBoardMove> GetValidCellsDown(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsDown(startingCell, DetermineIfCellValid);
        }

        public List<ValidBoardMove> GetValidCellsDownLeft(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsDownLeft(startingCell, DetermineIfCellValid);
        }

        
        public List<ValidBoardMove> GetValidCellsDownRight(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsDownRight(startingCell, DetermineIfCellValid);
        }
        public List<ValidBoardMove> GetValidCellsUpLeft(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsUpLeft(startingCell, DetermineIfCellValid);
        }
        public List<ValidBoardMove> GetValidCellsUpRight(string coords)
        {
            var startingCell = getCell(coords);
            return ExecuteFunctionOnCellsUpRight(startingCell, DetermineIfCellValid);
        }
        public ValidBoardMove DetermineIfCellValid(Cell fromCell, Cell toCell, ValidBoardMove.MovePath path)
        {
            ValidNotationProperties validMoveProps = this.CellIsValid(fromCell, toCell);
            var move = new ValidBoardMove(fromCell.Coordinates, toCell.Coordinates, path, this.isWhite);

            if (validMoveProps.IsValid || validMoveProps.IsProtected)
            {
                this.PiecePressure.Add(move);
            }
            if (validMoveProps.IsPotentiallyPinned)
            {
                this.startingCell = fromCell;
                this.PinnedCell = toCell;
                DetermineIfAbsolutePinned(toCell, path);
            }
            move.MoveProperties = validMoveProps;
            return move;
        }

        private void DetermineIfAbsolutePinned(Cell pinnedCell,ValidBoardMove.MovePath path)
        {
            List<ValidBoardMove> test = new List<ValidBoardMove>();
            switch (path)
            {
                case ValidBoardMove.MovePath.Down:
                    test = ExecuteFunctionOnCellsDown(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.Up:
                    test = ExecuteFunctionOnCellsUp(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.Right:
                    test = ExecuteFunctionOnCellsToRight(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.Left:
                    test = ExecuteFunctionOnCellsToLeft(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.DownLeft:
                    test = ExecuteFunctionOnCellsDownLeft(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.DownRight:
                    test = ExecuteFunctionOnCellsDownRight(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.UpLeft:
                    test = ExecuteFunctionOnCellsUpLeft(pinnedCell, DetermineIfCellPinned);
                    break;
                case ValidBoardMove.MovePath.UpRight:
                    test = ExecuteFunctionOnCellsUpRight(pinnedCell, DetermineIfCellPinned);
                    break;
                default:
                    break;
            }
            List<ValidBoardMove> arePinned = test.Where(x => x.IsPinningMove == true && x.CoordinatesTo == EnemyKing.CurrentCoordinates).ToList();
            if (arePinned.Count == 1)
            {
                var movesOnPath = FindPiecesBetween(arePinned[0]);
                var filteredMoves = movesOnPath.Where(IsPieceColorSameAsSelf).ToList();

                if (filteredMoves.Count == 1)
                {

                    // we are absolutely pinned hgere
                    var piece = Cell.GetPiece(this.PinnedCell);
                    if (piece.TypeOfPiece == PieceType.Rook)
                    {
                        var rook = (Rook)piece;
                        rook.DetermineValidMoves(rook.CurrentCoordinates, null, test);
                    }
                    else if (piece.TypeOfPiece == PieceType.Bishop)
                    {
                        var bishop = (Bishop)piece;
                        bishop.DetermineValidMoves(bishop.CurrentCoordinates, null, test);
                    }
                    else if (piece.TypeOfPiece == PieceType.Pawn)
                    {
                        var pawn = (Pawn)piece;
                        pawn.DetermineValidMoves(pawn.CurrentCoordinates, null, test);
                    }
                    else if (piece.TypeOfPiece == PieceType.King)
                    {
                        throw new NotImplementedException();
                    }
                    else if (piece.TypeOfPiece == PieceType.Queen)
                    {
                        var queen = (Queen)piece;
                        queen.DetermineValidMoves(queen.CurrentCoordinates, null, test);
                    }
                    else if (piece.TypeOfPiece == PieceType.Knight)
                    {
                        var knight = (Knight)piece;
                        knight.DetermineValidMoves(knight.CurrentCoordinates, null, test);
                    }
                }
            }
        }

        private bool IsPieceColorSameAsSelf(ValidBoardMove move)
        {
            var cell = this.getCell(move.CoordinatesTo);
            if (Cell.HasPiece(cell))
            {
                var piece = Cell.GetPiece(cell);
                if (piece.isWhite != isWhite)
                {
                    return true;
                }
            }
            return false;
        }

        private ValidBoardMove DetermineIfCellPinned(Cell pinnedCell, Cell potentialCell, ValidBoardMove.MovePath path)
        {
            var piece = Cell.GetPiece(potentialCell);
            if (piece == EnemyKing)
            {
                if (piece != null)
                {
                    return new ValidBoardMove(pinnedCell.Coordinates, potentialCell.Coordinates, path, piece.isWhite, true);
                }
                return null;
                
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

        public ValidNotationProperties CellIsValid(Cell fromCell, Cell toCell)
        {
            var validMoveProps = new ValidNotationProperties();
            return validMoveProps.DetermineMoveProperties(fromCell, toCell);
        }

        public char? GetColumnLetter(Cell currentCell, int spacesAway)
        {
            if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(currentCell.ColumnNumber + spacesAway, out char columnletter))
            {
                return columnletter;
            }
            return null;
        }

        public List<ValidBoardMove> FindPiecesBetween(ValidBoardMove move)
        {
            var cell = this.getCell(move.CoordinatesFrom);
            var moves = new List<ValidBoardMove>();
            switch (move.Path)
            {
                case ValidBoardMove.MovePath.Down:
                    moves = ExecuteFunctionOnCellsDown(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.Up:
                    moves = ExecuteFunctionOnCellsUp(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.Right:
                    moves = ExecuteFunctionOnCellsToRight(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.Left:
                    moves = ExecuteFunctionOnCellsToLeft(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.DownLeft:
                    moves = ExecuteFunctionOnCellsDownLeft(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.DownRight:
                    moves = ExecuteFunctionOnCellsDownRight(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.UpLeft:
                    moves = ExecuteFunctionOnCellsUpLeft(cell, DetermineAllCellsOnPath);
                    break;
                case ValidBoardMove.MovePath.UpRight:
                    moves = ExecuteFunctionOnCellsUpRight(cell, DetermineAllCellsOnPath);
                    break;
                default:
                    break;
            }

            return moves;
        }

        private ValidBoardMove DetermineAllCellsOnPath(Cell fromCell, Cell toCell, ValidBoardMove.MovePath path)
        {
            return new ValidBoardMove(fromCell.Coordinates, toCell.Coordinates, path, isWhite);
        }

        public  List<ValidBoardMove> FindAttackPath(ValidBoardMove boardPath)
        {
            var returnList = new List<ValidBoardMove>();
            switch (boardPath.Path)
            {
                case ValidBoardMove.MovePath.Down:
                    returnList = GetValidCellsUp(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.Up:
                    returnList = GetValidCellsDown(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.Left:
                    returnList = GetValidCellsDownRight(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.Right:
                    returnList = GetValidCellsLeft(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.UpLeft:
                    returnList = GetValidCellsDownRight(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.UpRight:
                    returnList = GetValidCellsDownLeft(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.DownLeft:
                    returnList = GetValidCellsUpRight(boardPath.CoordinatesTo);
                    break;
                case ValidBoardMove.MovePath.DownRight:
                    returnList = GetValidCellsUpLeft(boardPath.CoordinatesTo);
                    break;
                default:
                    break;

            }
            return returnList.Where(move => MovesAreBetweenCheckMovePath(move, boardPath)).ToList();
        }
        private bool MovesAreBetweenCheckMovePath(ValidBoardMove move, ValidBoardMove checkMove)
        {
            var checkCoordsTo = checkMove.CoordinatesTo;
            var checkCoordsToColumnLetter = checkCoordsTo.GetColumnLetter();
            var checkCoordsToRowNumber = checkCoordsTo.GetRowNumber();

            var checkCoordsFrom = checkMove.CoordinatesFrom;
            var checkCoordsFromColumnLetter = checkCoordsFrom.GetColumnLetter();
            var checkCoordsFromRowNumber = checkCoordsFrom.GetRowNumber();

            var moveCoordsTo = move.CoordinatesTo;
            var moveColumnLetter = moveCoordsTo.GetColumnLetter();
            var moveRowNumber = moveCoordsTo.GetRowNumber();

            if (moveColumnLetter.IsBetween<char?>(checkCoordsFromColumnLetter, checkCoordsToColumnLetter) &&
                moveRowNumber.IsBetween<char?>(checkCoordsFromRowNumber, checkCoordsToRowNumber))
            {
                return true;
            }
            return false;


        }
        public void FilterMovesIfChecked(ValidBoardMove checkingMove)
        {
            if (checkingMove != null)
            {
                this.AllowedCellsAfterCheck = FindAttackPath(checkingMove);

            }
            else
            {
                this.AllowedCellsAfterCheck = new List<ValidBoardMove>();
            }
            
            if (this.AllowedCellsAfterCheck.Count > 0 &&
                checkingMove.IsWhite != this.isWhite)
            {
                var filteredCoordinateList = this.ValidMoves.Select(GeneralUtilities.SelectCoordinates)
                    .Intersect(this.AllowedCellsAfterCheck.Select(GeneralUtilities.SelectCoordinates)).ToList();

                this.ValidMoves = this.ValidMoves.Where(x => filteredCoordinateList.IndexOf(x.CoordinatesTo) > -1).ToList();
            }
        }

        public void FilterMovesIfPinned(List<ValidBoardMove> pinnedMoves)
        {
            if (pinnedMoves == null)
            {
                return;
            }
            List<ValidBoardMove.MovePath> combinedMovePaths = new List<ValidBoardMove.MovePath>();
            var movePaths = pinnedMoves.Select(move => move.Path).ToList();
            foreach(var path in movePaths)
            {
                if (PinnedAllowedMovePaths.TryGetValue(path, out List<ValidBoardMove.MovePath>  rangeToAdd))
                {
                    combinedMovePaths.AddRange(rangeToAdd);
                }
            }
            combinedMovePaths = combinedMovePaths.Distinct().ToList();
            this.ValidMoves = this.ValidMoves.Where(x => combinedMovePaths.IndexOf(x.Path) > -1).ToList();
        }
    }
}