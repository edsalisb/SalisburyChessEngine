using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;

namespace SalisburyChessEngine.Pieces
{
    public class King : PieceBase
    {
        public override pieceType TypeOfPiece { get; set; }
        private Func<string, Cell> getCell;
        public bool IsChecked { get; internal set; }

        public King(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates)
        {
            this.TypeOfPiece = pieceType.King;
            this.getCell = getCell;
        }

       
        public override string ToString()
        {
            return "K";
        }

        public override void determineValidMoves(string coords, bool isChecked)
        {
            throw new NotImplementedException();
        }

        public List<ValidBoardMove> determineValidMoves(string coords, List<ValidBoardMove> enemyPressure, List<ValidBoardMove> samePressure)
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
                var moveProperty = new ValidBoardMove(coords,oneLeftCell.Coordinates, ValidBoardMove.movePath.Left);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneLeftCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneRightCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords,oneRightCell.Coordinates, ValidBoardMove.movePath.Right);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneRightCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUpCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUpCell.Coordinates, ValidBoardMove.movePath.Up);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneUpCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneDownCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDownCell.Coordinates, ValidBoardMove.movePath.Down);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneDownCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.movePath.UpLeft);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneUoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.movePath.UpRight);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneUoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneDoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneLCell.Coordinates, ValidBoardMove.movePath.DownLeft);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneDoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }

            }
            if (cellIsValidForKing(kingCell, oneDoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneRCell.Coordinates, ValidBoardMove.movePath.DownRight);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(oneDoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            return samePressure;
        }

        public bool cellIsValidForKing(Cell cellFrom, Cell cellTo, List<ValidBoardMove> enemyPressure)
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