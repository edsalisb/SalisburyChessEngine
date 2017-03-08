using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Pawn: PieceBase
    {
        
        public Pawn(bool isWhite, Func<string, Cell> getCell, string coordinates) : base(isWhite, coordinates, getCell)
        {

            this.TypeOfPiece = PieceType.Pawn;
        }

        public override void DetermineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            this.AddToValidMoves(coords);

            this.FilterMovesIfChecked(checkingMove);

        }

        public void DetermineValidMoves(string coords, List<ValidBoardMove> filterMoves)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

            this.AddToValidMoves(coords);
            this.FilterMovesIfPinned(filterMoves);
        }

        public override void AddToValidMoves(string coords)
        {
            if (this.isWhite)
            {
                this.DetermineWhiteMoves(coords);
            }
            else
            {
                this.DetermineBlackMoves(coords);
            }
            this.ValidMovesSet = true;
        }

        private void DetermineWhiteMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 2)
            {
                var twoRowsForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row + 2));
                if (CellIsValidForPawn(startingCell, twoRowsForwardCell))
                {
                    var moveProperty = new ValidBoardMove(coords, twoRowsForwardCell.Coordinates, ValidBoardMove.MovePath.Up, this.isWhite);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row + 1));
            if (CellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new ValidBoardMove(coords, oneRowForwardCell.Coordinates, ValidBoardMove.MovePath.Up, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = GetColumnLetter(startingCell, -1);
            var rightColumnLetter = GetColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row + 1));
                if (Cell.IsNotNull(oneUoneLCell))
                {
                    var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.MovePath.UpLeft, this.isWhite);
                    if (Cell.HasPiece(oneUoneLCell))
                    {
                        if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                        {
                            this.ValidMoves.Add(moveProperty);
                            this.PiecePressure.Add(moveProperty);
                        }
                    }
                    else
                    {
                        this.PiecePressure.Add(moveProperty);
                    }
                }
               
            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row + 1));
                if (Cell.IsNotNull(oneUoneRCell))
                {
                    var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.MovePath.UpLeft, this.isWhite);
                    if (Cell.HasPiece(oneUoneRCell))
                    {
                        if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                        {
                            this.ValidMoves.Add(moveProperty);
                            this.PiecePressure.Add(moveProperty);
                        }
                    }
                    else
                    {
                        this.PiecePressure.Add(moveProperty);
                    }
                }
            }
        }

        private void DetermineBlackMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 7)
            {
                var twoRowsForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row - 2));
                if (CellIsValidForPawn(startingCell, twoRowsForwardCell))
                {
                    var moveProperty = new ValidBoardMove(coords, twoRowsForwardCell.Coordinates, ValidBoardMove.MovePath.Down, this.isWhite);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row - 1));
            if (CellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new ValidBoardMove(coords, oneRowForwardCell.Coordinates, ValidBoardMove.MovePath.Down, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = GetColumnLetter(startingCell, -1);
            var rightColumnLetter = GetColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row - 1));
                if (Cell.IsNotNull(oneUoneLCell))
                {
                    var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.MovePath.UpLeft, this.isWhite);
                    if (Cell.HasPiece(oneUoneLCell))
                    {
                        if (startingCell.CurrentPiece.isWhite != oneUoneLCell.CurrentPiece.isWhite)
                        {
                            this.ValidMoves.Add(moveProperty);
                            this.PiecePressure.Add(moveProperty);
                        }
                    }
                    else
                    {
                        this.PiecePressure.Add(moveProperty);
                    }
                }

            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row - 1));
                if (Cell.IsNotNull(oneUoneRCell))
                {
                    var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.MovePath.UpLeft, this.isWhite);
                    if (Cell.HasPiece(oneUoneRCell))
                    {
                        if (startingCell.CurrentPiece.isWhite != oneUoneRCell.CurrentPiece.isWhite)
                        {
                            this.ValidMoves.Add(moveProperty);
                            this.PiecePressure.Add(moveProperty);
                        }
                    }
                    else
                    {
                        this.PiecePressure.Add(moveProperty);
                    }
                }
            }
        }

        private bool CellIsValidForPawn(Cell fromCell, Cell toCell)
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