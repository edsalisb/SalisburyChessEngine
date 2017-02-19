using System;
using System.Collections.Generic;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        public override string CurrentCoordinates { get; set; }
        private Func<string, Cell> getCell;
        public Knight(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.TypeOfPiece = pieceType.Knight;
            this.getCell = getCell;
        }
        public override void determineValidMoves(string coords, bool isChecked)
        {
            ValidMoves = new List<PotentialMoves>();

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
                var moveProperty = new PotentialMoves(twoUpOneLeftCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoUpOneRightCell).IsValid)
            {
                var moveProperty = new PotentialMoves(twoUpOneRightCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoRightCell).IsValid)
            {
                var moveProperty = new PotentialMoves(oneUpTwoRightCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoRightCell).IsValid)
            {
                var moveProperty = new PotentialMoves(oneDownTwoRightCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneRightCell).IsValid)
            {
                var moveProperty = new PotentialMoves(twoDownOneRightCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneLeftCell).IsValid)
            {
                var moveProperty = new PotentialMoves(twoDownOneLeftCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoLeftCell).IsValid)
            {
                var moveProperty = new PotentialMoves(oneDownTwoLeftCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoLeftCell).IsValid)
            {
                var moveProperty = new PotentialMoves(oneUpTwoLeftCell.Coordinates, null);
                this.ValidMoves.Add(moveProperty);
            }

        }

        public override string ToString()
        {
            return "N";
        }
    }
}