﻿using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class ValidMoveProperties
    {
        public bool IsValid { get; set; }
        public bool IsTerminatable { get; set; }
        public ValidMoveProperties()
        {
            IsValid = false;
            IsTerminatable = false;
        }
        public ValidMoveProperties determineMoveProperties(Cell fromCell, Cell toCell)
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
                if (fromCell.CurrentPiece == null)
                {
                    IsValid = false;
                }
                else
                {
                    IsValid = true;
                }
                return this;
            }

            if (fromCell.CurrentPiece.isWhite != toCell.CurrentPiece.isWhite)
            {
                IsValid = true;
            }

            else
            {
                IsValid = false;
                IsTerminatable = true;
                return this;
            }
            IsValid = true;
            return this;
        }

        internal ValidMoveProperties determineMoveProperties(Cell cellFrom, Cell cellTo, List<string> enemyPressure)
        {
            var instance = determineMoveProperties(cellFrom, cellTo);
            if (instance.IsValid)
            {
                if (enemyPressure.IndexOf(cellTo.Coordinates) != -1)
                {
                    instance.IsValid = false;
                }
            }
            return instance;
        }
    }
}