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

        public bool IsPotentiallyPinned { get; set; }
        public ValidNotationProperties()
        {
            IsProtected = false;
            IsValid = false;
            IsTerminatable = false;
        }

        public ValidNotationProperties determineMoveProperties(Cell fromCell, Cell toCell)
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
                IsTerminatable = true;
                IsPotentiallyPinned = true;
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



        public ValidNotationProperties determineMoveProperties(Cell cellFrom, Cell cellTo, List<ValidBoardMove> enemyPressure)
        {
            var instance = determineMoveProperties(cellFrom, cellTo);
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
    }

}