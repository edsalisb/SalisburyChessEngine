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
            

            if (cellIsValid(currentCell, twoUpOneLeftCell))
            {
                ValidMoves.Add(twoUpOneLeftCell.Coordinates);
            }
            if (cellIsValid(currentCell, twoUpOneRightCell))
            {
                ValidMoves.Add(twoUpOneRightCell.Coordinates);
            }
            if (cellIsValid(currentCell, oneUpTwoRightCell))
            {
                ValidMoves.Add(oneUpTwoRightCell.Coordinates);
            }
            if (cellIsValid(currentCell, oneDownTwoRightCell))
            {
                ValidMoves.Add(oneDownTwoRightCell.Coordinates);
            }
            if (cellIsValid(currentCell, twoDownOneRightCell))
            {
                ValidMoves.Add(twoDownOneRightCell.Coordinates);
            }
            if (cellIsValid(currentCell, twoDownOneLeftCell))
            {
                ValidMoves.Add(twoDownOneLeftCell.Coordinates);
            }
            if (cellIsValid(currentCell, oneDownTwoLeftCell))
            {
                ValidMoves.Add(oneDownTwoLeftCell.Coordinates);
            }
            if (cellIsValid(currentCell, oneUpTwoLeftCell))
            {
                ValidMoves.Add(oneUpTwoLeftCell.Coordinates);
            }

        }

        private char? getColumnLetter(Cell currentCell, int spacesAway)
        {
            char columnletter;
            if (CellProperties.ColumnNumbersMappedToLetters.TryGetValue(currentCell.ColumnNumber + spacesAway, out columnletter))
            {
                return columnletter;
            }
            return null;
        }

        public override string ToString()
        {
            return "N";
        }
    }
}