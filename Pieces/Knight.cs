using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell;
        public Knight(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.piece = pieceType.Knight;
            this.getCell = getCell;
        }
        public override void determineValidMoves(string coords)
        {
            ValidMoves = new List<string>();

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
            

            if (cellIsValidForPiece(currentCell, twoUpOneLeftCell))
            {
                ValidMoves.Add(twoUpOneLeftCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, twoUpOneRightCell))
            {
                ValidMoves.Add(twoUpOneRightCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoRightCell))
            {
                ValidMoves.Add(oneUpTwoRightCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoRightCell))
            {
                ValidMoves.Add(oneDownTwoRightCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneRightCell))
            {
                ValidMoves.Add(twoDownOneRightCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, twoDownOneLeftCell))
            {
                ValidMoves.Add(twoDownOneLeftCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, oneDownTwoLeftCell))
            {
                ValidMoves.Add(oneDownTwoLeftCell.Coordinates);
            }
            if (cellIsValidForPiece(currentCell, oneUpTwoLeftCell))
            {
                ValidMoves.Add(oneUpTwoLeftCell.Coordinates);
            }

        }

        public override string ToString()
        {
            return "N";
        }
    }
}