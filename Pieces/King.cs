using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;

namespace SalisburyChessEngine.Pieces
{
    public class King : PieceBase
    {
        public delegate void OnCheckCallback();
        public delegate void OnCheckForCheckMateCallback(King k, EventArgs e);
        private bool isChecked;
        
        public event OnCheckCallback onCheckCallbacks;
        public event OnCheckForCheckMateCallback onCheckForCheckMateCallbacks;
        public bool IsChecked {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
                if (value) { 
                    if (onCheckCallbacks != null)
                    {
                        onCheckCallbacks();
                        onCheckForCheckMateCallbacks(this, EventArgs.Empty);

                    }
                }
            }
        }
        public King(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates, getCell)
        {
            this.TypeOfPiece = pieceType.King;
            this.CurrentCoordinates = coordinates;
            
        }
        public override string ToString()
        {
            return "K";
        }

        public override void determineValidMoves(string coords, ValidBoardMove checkingMove)
        {
            throw new NotImplementedException();
        }

        public List<ValidBoardMove> determineValidMoves(string coords, List<ValidBoardMove> enemyPressure, List<ValidBoardMove> samePressure, ValidBoardMove checkingMove)
        {
            this.ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();

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
                var moveProperty = new ValidBoardMove(coords,oneLeftCell.Coordinates, ValidBoardMove.movePath.Left, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneLeftCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneRightCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords,oneRightCell.Coordinates, ValidBoardMove.movePath.Right, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneRightCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUpCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUpCell.Coordinates, ValidBoardMove.movePath.Up, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUpCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneDownCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDownCell.Coordinates, ValidBoardMove.movePath.Down, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDownCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.movePath.UpLeft, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneUoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.movePath.UpRight, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (cellIsValidForKing(kingCell, oneDoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneLCell.Coordinates, ValidBoardMove.movePath.DownLeft, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }

            }
            if (cellIsValidForKing(kingCell, oneDoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneRCell.Coordinates, ValidBoardMove.movePath.DownRight, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
          
            this.FilterMovesIfChecked(checkingMove);
            
            return samePressure;
        }

        public bool cellIsValidForKing(Cell cellFrom, Cell cellTo, List<ValidBoardMove> enemyPressure)
        {
            var moveProps = new ValidNotationProperties();
            moveProps = moveProps.determineMoveProperties(cellFrom, cellTo, enemyPressure);
            if (moveProps.IsValid && !moveProps.IsProtected)
            {
                return true;
            }
            return false;
        }
    }
}