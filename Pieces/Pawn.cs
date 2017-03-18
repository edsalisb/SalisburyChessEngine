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
            
            if (!this.ValidMovesSet)
            {
                ValidMoves = new List<ValidBoardMove>();
                PiecePressure = new List<ValidBoardMove>();
                this.ValidMovesSet = true;
                this.AddToValidMoves(coords);
                this.FilterMoves(checkingMove);
            }
            else
            {
                this.FilterMoves(checkingMove);
            }

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
        }
        private void FilterMoves(ValidBoardMove checkingMove)
        {
            if (checkingMove != null)
            {
                this.FilterMovesIfChecked(checkingMove);
            }
            if (this.PinnedMoves.Count > 0)
            {
                this.FilterMovesIfPinned();
            }
        }
        private void DetermineWhiteMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 2)
            {
                var twoRowsForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row + 2));
                var twoCellsForwardMove = DetermineIfCellValid(startingCell, twoRowsForwardCell, ValidBoardMove.MovePath.Up);
                if (twoCellsForwardMove.MoveProperties.IsValid)
                {
                    this.ValidMoves.Add(twoCellsForwardMove);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row + 1));
            var oneCellForwardMove = DetermineIfCellValid(startingCell, oneRowForwardCell, ValidBoardMove.MovePath.Up);
            if (oneCellForwardMove.MoveProperties.IsValid)
            {
                this.ValidMoves.Add(oneCellForwardMove);
            }


            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = GetColumnLetter(startingCell, -1);
            var rightColumnLetter = GetColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row + 1));
                if (Cell.IsNotNull(oneUoneLCell))
                {
                    var oneUoneLMove = DetermineIfCellValid(startingCell, oneUoneLCell, ValidBoardMove.MovePath.UpLeft);
                    if (oneUoneLMove.MoveProperties.IsValid)
                    {
                        this.ValidMoves.Add(oneUoneLMove);
                    }
                }
               
            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row + 1));
                if (Cell.IsNotNull(oneUoneRCell))
                {
                    var oneUoneRMove = DetermineIfCellValid(startingCell, oneUoneRCell, ValidBoardMove.MovePath.UpRight);
                    if (oneUoneRMove.MoveProperties.IsValid)
                    {
                        this.ValidMoves.Add(oneUoneRMove);
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
                var twoCellsForwardMove = DetermineIfCellValid(startingCell, twoRowsForwardCell, ValidBoardMove.MovePath.Down);
                if (twoCellsForwardMove.MoveProperties.IsValid)
                {
                    this.ValidMoves.Add(twoCellsForwardMove);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.ColumnLetter.ToString() + (startingCell.Row - 1));
            var oneCellForwardMove = DetermineIfCellValid(startingCell, oneRowForwardCell, ValidBoardMove.MovePath.Down);
            if (oneCellForwardMove.MoveProperties.IsValid)
            {
                this.ValidMoves.Add(oneCellForwardMove);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = GetColumnLetter(startingCell, -1);
            var rightColumnLetter = GetColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row - 1));
                if (Cell.IsNotNull(oneUoneLCell))
                {
                    var oneUoneLMove = DetermineIfCellValid(startingCell, oneUoneLCell, ValidBoardMove.MovePath.DownLeft);
                    if (oneUoneLMove.MoveProperties.IsValid)
                    {
                        this.ValidMoves.Add(oneUoneLMove);
                    }
                }

            }
            if (rightColumnLetter != null)
            {
                var oneUoneRCell = getCell(rightColumnLetter.ToString() + (startingCell.Row - 1));
                if (Cell.IsNotNull(oneUoneRCell))
                {
                    var oneUoneRMove = DetermineIfCellValid(startingCell, oneUoneRCell, ValidBoardMove.MovePath.DownRight);
                    if (oneUoneRMove.MoveProperties.IsValid)
                    {
                        this.ValidMoves.Add(oneUoneRMove);
                    }
                }
            }
        }

       
        public override string ToString()
        {
            return "P";
        }
    }
}