using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;

namespace SalisburyChessEngine.Pieces
{
    public class King : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        private Func<string, Cell> getCell;
        public override string CurrentCoordinates { get; set; }

        public King(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.TypeOfPiece = pieceType.King;
            this.getCell = getCell;
        }

       
        public override string ToString()
        {
            return "K";
        }

        public override void determineValidMoves(string coords)
        {
            throw new NotImplementedException();
        }

        public List<string> determineValidMoves(string coords, List<string> enemyPressure, List<string> samePressure)
        {
            var kingCell = getCell(coords);

            var oneLeftCell = getCell(getColumnLetter(kingCell, -1) + kingCell.Row.ToString());
            var oneRightCell = getCell(getColumnLetter(kingCell, + 1) + kingCell.Row.ToString());
            var oneUpCell = getCell(kingCell.columnLetter + (kingCell.Row + 1).ToString());
            var oneDownCell = getCell(kingCell.columnLetter + (kingCell.Row - 1).ToString());

            var oneUoneLCell = getCell(getColumnLetter(kingCell, -1) + (kingCell.Row + 1).ToString());
            var oneUoneRCell = getCell(getColumnLetter(kingCell, 1) + (kingCell.Row + 1).ToString());
            var oneDoneLCell = getCell(getColumnLetter(kingCell, -1) + (kingCell.Row - 1).ToString());
            var oneDoneRCell = getCell(getColumnLetter(kingCell, 1) + (kingCell.Row - 1).ToString());

            if (cellIsValidForKing(kingCell, oneLeftCell, enemyPressure))
            {
                this.ValidMoves.Add(oneLeftCell.Coordinates);
                if (samePressure.IndexOf(oneLeftCell.Coordinates) == -1)
                {
                    samePressure.Add(oneLeftCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneRightCell, enemyPressure))
            {
                this.ValidMoves.Add(oneRightCell.Coordinates);
                if (samePressure.IndexOf(oneRightCell.Coordinates) == -1)
                {
                    samePressure.Add(oneRightCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneUpCell, enemyPressure))
            {
                this.ValidMoves.Add(oneUpCell.Coordinates);
                if (samePressure.IndexOf(oneUpCell.Coordinates) == -1)
                {
                    samePressure.Add(oneUpCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneDownCell, enemyPressure))
            {
                this.ValidMoves.Add(oneDownCell.Coordinates);
                if (samePressure.IndexOf(oneDownCell.Coordinates) == -1)
                {
                    samePressure.Add(oneDownCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneLCell, enemyPressure))
            {
                this.ValidMoves.Add(oneUoneLCell.Coordinates);
                if (samePressure.IndexOf(oneUoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(oneUoneLCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneRCell, enemyPressure))
            {
                this.ValidMoves.Add(oneUoneRCell.Coordinates);
                if (samePressure.IndexOf(oneUoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(oneUoneRCell.Coordinates);
                }
            }
            if (cellIsValidForKing(kingCell, oneDoneLCell, enemyPressure))
            {
                this.ValidMoves.Add(oneDoneLCell.Coordinates);
                if (samePressure.IndexOf(oneDoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(oneDoneLCell.Coordinates);
                }

            }
            if (cellIsValidForKing(kingCell, oneDoneRCell, enemyPressure))
            {
                this.ValidMoves.Add(oneDoneRCell.Coordinates);
                if (samePressure.IndexOf(oneDoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(oneDoneRCell.Coordinates);
                }
            }
            return samePressure;
        }

        public bool cellIsValidForKing(Cell cellFrom, Cell cellTo, List<string> enemyPressure)
        {
            var moveProps = new ValidNotationProperties();
            moveProps = moveProps.determineMoveProperties(cellFrom, cellTo, enemyPressure);
            if (moveProps.IsValid)
            {
                return true;
            }
            return false;
        }
    }
}