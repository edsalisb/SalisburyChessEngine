using System;
using System.Collections.Generic;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine.Pieces
{
    public class Pawn: PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        private Func<string, Cell> getCell { get; set; }
        public override string CurrentCoordinates { get; set; }
        public List<PotentialMoves> piecePressureCoords { get; set; }

        public Pawn(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.TypeOfPiece = pieceType.Pawn;
            this.getCell = getCell;
            this.piecePressureCoords = new List<PotentialMoves>();
        }
        
        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<PotentialMoves>();

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
                    var moveProperty = new PotentialMoves(twoRowsForwardCell.Coordinates, PotentialMoves.movePath.Up);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row + 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new PotentialMoves(oneRowForwardCell.Coordinates, PotentialMoves.movePath.Up);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row + 1));
                var moveProperty = new PotentialMoves(oneUoneLCell.Coordinates, PotentialMoves.movePath.UpLeft);

                if (oneUoneLCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                    { 
                        this.ValidMoves.Add(moveProperty);
                        piecePressureCoords.Add(moveProperty);
                    }
                }
                else
                {
                    piecePressureCoords.Add(moveProperty);
                }
               
            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row + 1));
                var moveProperty = new PotentialMoves(oneUoneRCell.Coordinates, PotentialMoves.movePath.UpRight);

                if (oneUoneRCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                    {
                        this.ValidMoves.Add(moveProperty);
                        piecePressureCoords.Add(moveProperty);
                    }
                }
                else
                {
                    piecePressureCoords.Add(moveProperty);
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
                    var moveProperty = new PotentialMoves(twoRowsForwardCell.Coordinates, PotentialMoves.movePath.Down);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row - 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new PotentialMoves(oneRowForwardCell.Coordinates, PotentialMoves.movePath.Down);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row - 1));
                var moveProperty = new PotentialMoves(oneUoneLCell.Coordinates, PotentialMoves.movePath.DownLeft);

                if (oneUoneLCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                    {
                        this.ValidMoves.Add(moveProperty);
                        piecePressureCoords.Add(moveProperty);
                    }
                }
                else
                {
                    piecePressureCoords.Add(moveProperty);
                }

            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row - 1));
                var moveProperty = new PotentialMoves(oneUoneRCell.Coordinates, PotentialMoves.movePath.DownRight);
                if (oneUoneRCell.CurrentPiece != null)
                {
                    if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                    {
                        this.ValidMoves.Add(moveProperty);
                        piecePressureCoords.Add(moveProperty);
                    }
                }
                else
                {
                    piecePressureCoords.Add(moveProperty);
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