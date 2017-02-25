using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;
namespace SalisburyChessEngine.Board
{
    public class ValidNotationProperties
    {
        public bool IsValid { get; set; }
        public bool IsTerminatable { get; set; }
    
        public ValidNotationProperties()
        {
            IsValid = false;
            IsTerminatable = false;
        }
        public ValidNotationProperties determineMoveProperties(Cell fromCell, Cell toCell)
        {
            if (fromCell == null || toCell == null)
            {
                IsValid = false;
                return this;
            }
            if (fromCell.CurrentPiece == null)
            {
                IsValid = false;
                return this;
            }
            if (toCell.CurrentPiece == null)
            {
                IsValid = true;
                return this;
            }

            if (fromCell.CurrentPiece.isWhite != toCell.CurrentPiece.isWhite)
            {
                IsValid = true;
                IsTerminatable = true;
                return this;
            }

            else
            {
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