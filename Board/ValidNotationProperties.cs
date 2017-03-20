using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{

    public class ValidNotationProperties
    {

        public bool IsValid { get; set; }
        public bool IsTerminatable { get; set; }
        public bool IsProtected { get; set; }

        public bool IsAbsolutePinned { get; set; }

        public bool IsPotentiallyPinned { get; set; }
        public ValidNotationProperties()
        {
            IsProtected = false;
            IsValid = false;
            IsTerminatable = false;
            this.IsAbsolutePinned = false;
        }

        public ValidNotationProperties(bool isAbsolutePinned): this()
        {
            this.IsAbsolutePinned = isAbsolutePinned;
        }

        public ValidNotationProperties DetermineMovePropertiesForPiece(Cell fromCell, Cell toCell)
        {
            if (!Cell.IsNotNull(fromCell) || !Cell.IsNotNull(toCell))
            {
                IsValid = false;
                return this;
            }
            if (!Cell.HasPiece(fromCell))
            {
                IsValid = false;
                return this;
            }
            if (!Cell.HasPiece(toCell))
            {
                IsValid = true;
                return this;
            }

            var fromCellPiece = Cell.GetPiece(fromCell);
            var toCellPiece = Cell.GetPiece(toCell);

            if (fromCellPiece.isWhite != toCellPiece.isWhite)
            {
                IsValid = true;
                IsPotentiallyPinned = true;
                IsTerminatable = true;
                return this;
            }

            else
            {
                IsProtected = true;
                IsValid = false;
                IsTerminatable = true;
                return this;
            }
        }
        
        public ValidNotationProperties DetermineMoveProperties(Cell cellFrom, Cell cellTo, List<ValidBoardMove> enemyPressure)
        {
            var instance = DetermineMovePropertiesForPiece(cellFrom, cellTo);
            if (instance.IsValid)
            {
                var enemyCoordinatePressure = enemyPressure.Select(GeneralUtilities.SelectCoordinates).ToList();
                int index = enemyCoordinatePressure.IndexOf(cellTo.Coordinates);
                if (index != -1)
                {
                    instance.IsValid = false;
                }
            }
            return instance;
        }

        public ValidNotationProperties DetermineMovePropertiesForPawn(Cell fromCell, Cell toCell)
        {
            if (!Cell.IsNotNull(fromCell) || !Cell.IsNotNull(toCell))
            {
                IsValid = false;
                return this;
            }
            if (!Cell.HasPiece(fromCell))
            {
                IsValid = false;
                return this;
            }

            var fromCellPiece = Cell.GetPiece(fromCell);
            var movePath = ValidBoardMove.DetermineMovePath(fromCell, toCell);
            if (movePath == ValidBoardMove.MovePath.DownLeft || movePath == ValidBoardMove.MovePath.DownRight ||
                movePath == ValidBoardMove.MovePath.UpRight || movePath == ValidBoardMove.MovePath.UpLeft)
            {
                if (!Cell.HasPiece(toCell))
                {
                    IsValid = false;
                    return this;
                }
                var toCellPiece = Cell.GetPiece(toCell);
                if (fromCellPiece.isWhite != toCellPiece.isWhite)
                {
                    IsValid = true;
                    IsPotentiallyPinned = true;
                    IsTerminatable = true;
                    return this;
                }

                else
                {
                    IsProtected = true;
                    IsValid = false;
                    IsTerminatable = true;
                    return this;
                }

            }
            else if (movePath == ValidBoardMove.MovePath.Up || movePath == ValidBoardMove.MovePath.Down)
            {
                if (!Cell.HasPiece(toCell))
                {
                    IsValid = true;
                    return this;
                }
                IsValid = false;
                return this;
            }
            else
            {
                IsValid = false;
                return this;
            }

            return this;
        }
    }

}