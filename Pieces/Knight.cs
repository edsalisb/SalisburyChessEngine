using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
   
        public Knight(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates, getCell)
        {
            this.TypeOfPiece = PieceType.Knight;
        }
        public override void DetermineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            AddToValidMoves(coords);

            this.FilterMovesIfChecked(checkingMove);

        }
        public void DetermineValidMoves(string coords, List<ValidBoardMove> pinnedMoves)
        {
            ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            AddToValidMoves(coords);
        }

        public override void AddToValidMoves(string coords)
        {
            var currentCell = this.getCell(coords);

            var twoColumnsLeft = GetColumnLetter(currentCell, -2);
            var oneColumnLeft = GetColumnLetter(currentCell, -1);
            var oneColumnRight = GetColumnLetter(currentCell, 1);
            var twoColumnsRight = GetColumnLetter(currentCell, 1);

            //8 moves knight can make
            var twoUpOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row + 2));
            var twoUpOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row + 2));
            var oneUpTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row + 1));
            var oneDownTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row - 1));
            var twoDownOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row - 2));
            var twoDownOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row - 2));
            var oneDownTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row - 1));
            var oneUpTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row + 1));


            if (CellIsValidForPiece(currentCell, twoUpOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneLeftCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, twoUpOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneRightCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, oneUpTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoRightCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, oneDownTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoRightCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, twoDownOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneRightCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, twoDownOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneLeftCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, oneDownTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoLeftCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (CellIsValidForPiece(currentCell, oneUpTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoLeftCell.Coordinates, ValidBoardMove.MovePath.Invalid, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            this.ValidMovesSet = true;
        }

        public override string ToString()
        {
            return "N";
        }
    }
}