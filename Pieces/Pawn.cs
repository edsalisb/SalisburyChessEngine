using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class Pawn: PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        private Func<string, Cell> getCell { get; set; }
        public override string CurrentCoordinates { get; set; }
        public List<string> piecePressureCoords { get; set; }

        public Pawn(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.TypeOfPiece = pieceType.Pawn;
            this.getCell = getCell;
            this.piecePressureCoords = new List<string>();
        }
        
        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<string>();

            if (this.isWhite)
            {
                this.determineWhiteMoves(coords);
            }
            else
            {
                this.determineBlackMoves(coords);
            }
        }

        private void determineWhiteMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 2)
            {
                var twoRowsForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row + 2));
                if (cellIsValidForPawn(startingCell, twoRowsForwardCell))
                {
                    this.ValidMoves.Add(twoRowsForwardCell.Coordinates);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row + 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                this.ValidMoves.Add(oneRowForwardCell.Coordinates);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row + 1));
                if (oneUoneLCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                    {
                        ValidMoves.Add(oneUoneLCell.Coordinates);
                        piecePressureCoords.Add(oneUoneLCell.Coordinates);
                    }
                }
                else
                {
                    piecePressureCoords.Add(oneUoneLCell.Coordinates);
                }
               
            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row + 1));
                if (oneUoneRCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                    {
                        ValidMoves.Add(oneUoneRCell.Coordinates);
                        piecePressureCoords.Add(oneUoneRCell.Coordinates);
                    }
                }
                else
                {
                    piecePressureCoords.Add(oneUoneRCell.Coordinates);
                }
            }
        }

        private void determineBlackMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 7)
            {
                var twoRowsForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row - 2));
                if (cellIsValidForPawn(startingCell, twoRowsForwardCell))
                {
                    this.ValidMoves.Add(twoRowsForwardCell.Coordinates);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row - 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                this.ValidMoves.Add(oneRowForwardCell.Coordinates);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row - 1));
                if (oneUoneLCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                    {
                        ValidMoves.Add(oneUoneLCell.Coordinates);
                        piecePressureCoords.Add(oneUoneLCell.Coordinates);
                    }
                }
                else
                {
                    piecePressureCoords.Add(oneUoneLCell.Coordinates);
                }

            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row - 1));
                if (oneUoneRCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                    {
                        ValidMoves.Add(oneUoneRCell.Coordinates);
                        piecePressureCoords.Add(oneUoneRCell.Coordinates);
                    }
                }
                else
                {
                    piecePressureCoords.Add(oneUoneRCell.Coordinates);
                }
            }
        }

        private bool cellIsValidForPawn(Cell fromCell, Cell toCell)
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
            return false;
        }
        public override string ToString()
        {
            return "P";
        }
    }
}