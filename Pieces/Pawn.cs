using System;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Pawn: PieceBase
    {

        private Func<string, Cell> getCell { get; set; }
        public List<ValidBoardMove> piecePressureCoords { get; set; }
        
        public Pawn(bool isWhite, Func<string, Cell> getCell, string coordinates) : base(isWhite, coordinates)
        {

            this.TypeOfPiece = pieceType.Pawn;
            this.getCell = getCell;
            this.piecePressureCoords = new List<ValidBoardMove>();
        }

        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            if (this.isWhite)
            {
                this.determineWhiteMoves(coords);
            }
            else
            {
                this.determineBlackMoves(coords);
            }
            this.FilterMovesIfChecked(checkingMove, getCell);

        }

        private void determineWhiteMoves(string coords)
        {
            var startingCell = getCell(coords);
            if (startingCell.Row == 2)
            {
                var twoRowsForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row + 2));
                if (cellIsValidForPawn(startingCell, twoRowsForwardCell))
                {
                    var moveProperty = new ValidBoardMove(coords, twoRowsForwardCell.Coordinates, ValidBoardMove.movePath.Up, this.isWhite);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row + 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new ValidBoardMove(coords, oneRowForwardCell.Coordinates, ValidBoardMove.movePath.Up, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row + 1));
                var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.movePath.UpLeft, this.isWhite);

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
                var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.movePath.UpRight, this.isWhite);

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
                    var moveProperty = new ValidBoardMove(coords, twoRowsForwardCell.Coordinates, ValidBoardMove.movePath.Down, this.isWhite);
                    this.ValidMoves.Add(moveProperty);
                }
            }

            //checking one row forward
            var oneRowForwardCell = getCell(startingCell.columnLetter.ToString() + (startingCell.Row - 1));
            if (cellIsValidForPawn(startingCell, oneRowForwardCell))
            {
                var moveProperty = new ValidBoardMove(coords, oneRowForwardCell.Coordinates, ValidBoardMove.movePath.Down, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }

            //checking cells 1U1L and 1U1R for an enemy piece to take
            var leftColumnLetter = getColumnLetter(startingCell, -1);
            var rightColumnLetter = getColumnLetter(startingCell, 1);

            if (leftColumnLetter != null)
            {
                var oneUoneLCell = getCell(leftColumnLetter.ToString() + (startingCell.Row - 1));
                var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.movePath.DownLeft, this.isWhite);

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
                var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.movePath.DownRight, this.isWhite);
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