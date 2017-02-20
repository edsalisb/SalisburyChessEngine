using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
   
        private Func<string, Cell> getCell;
        public Knight(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates)
        {
            this.TypeOfPiece = pieceType.Knight;
            this.getCell = getCell;
        }
        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            ValidMoves = new List<ValidBoardMove>();

            var currentCell = this.getCell(coords);

            var twoColumnsLeft = getColumnLetter(currentCell, -2);
            var oneColumnLeft = getColumnLetter(currentCell, -1);
            var oneColumnRight = getColumnLetter(currentCell, 1);
            var twoColumnsRight = getColumnLetter(currentCell, 1);

            //8 moves knight can make
            var twoUpOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row + 2));
            var twoUpOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row + 2));
            var oneUpTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row + 1));
            var oneDownTwoRightCell = this.getCell(twoColumnsRight.ToString() + (currentCell.Row - 1));
            var twoDownOneRightCell = this.getCell(oneColumnRight.ToString() + (currentCell.Row - 2));
            var twoDownOneLeftCell = this.getCell(oneColumnLeft.ToString() + (currentCell.Row -2));
            var oneDownTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row - 1));
            var oneUpTwoLeftCell = this.getCell(twoColumnsLeft.ToString() + (currentCell.Row + 1));
            

            if (cellIsValidForPiece(currentCell, twoUpOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneLeftCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoUpOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoUpOneRightCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoRightCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoRightCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneRightCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneRightCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, twoDownOneLeftCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneDownTwoLeftCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoLeftCell).IsValid)
            {
                var moveProperty = new ValidBoardMove(coords, oneUpTwoLeftCell.Coordinates, null, this.isWhite);
                this.ValidMoves.Add(moveProperty);
            }

        }

        public override string ToString()
        {
            return "N";
        }
    }
}