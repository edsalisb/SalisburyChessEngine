using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Utilities;

namespace SalisburyChessEngine.Pieces
{
    public class King : PieceBase
    {
        public bool HasMoved { get; set; }
        public delegate void OnCheckCallback();
        public delegate void OnCheckForCheckMateCallback(King k, EventArgs e);
        private bool isChecked;
        private List<ValidBoardMove> enemyPressure;
        private List<ValidBoardMove> samePressure;


        public event OnCheckCallback OnCheckCallbacks;
        public event OnCheckForCheckMateCallback OnCheckForCheckMateCallbacks;
        public bool IsChecked {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
                if (value) { 
                    if (OnCheckCallbacks != null)
                    {
                        OnCheckCallbacks();
                        OnCheckForCheckMateCallbacks(this, EventArgs.Empty);

                    }
                }
            }
        }
        public King(bool isWhite, Func<string, Cell> getCell, string coordinates): base(isWhite, coordinates, getCell)
        {
            this.TypeOfPiece = PieceType.King;
            this.CurrentCoordinates = coordinates;
            
        }
        public override string ToString()
        {
            return "K";
        }

        public override void DetermineValidMoves(string coords, ValidBoardMove checkingMove, List<ValidBoardMove> pinnedMove)
        {
            return;
        }

        public List<ValidBoardMove> DetermineValidMoves(string coords, List<ValidBoardMove> enemyPressure, List<ValidBoardMove> samePressure, ValidBoardMove checkingMove)
        {
            this.ValidMoves = new List<ValidBoardMove>();
            PiecePressure = new List<ValidBoardMove>();
            this.enemyPressure = enemyPressure;
            this.samePressure = samePressure;

            this.AddToValidMoves(coords);
            this.FilterMovesIfChecked(checkingMove);
            return samePressure;
        }
        public override void AddToValidMoves(string coords)
        {
            var kingCell = getCell(coords);

            var oneLeftCell = getCell(GetColumnLetter(kingCell, - 1) + kingCell.Row.ToString());
            var oneRightCell = getCell(GetColumnLetter(kingCell, + 1) + kingCell.Row.ToString());

            var twoLeftCell = getCell(GetColumnLetter(kingCell, - 2) + kingCell.Row.ToString());
            var twoRightCell = getCell(GetColumnLetter(kingCell, + 2) + kingCell.Row.ToString());

            var oneUpCell = getCell(kingCell.ColumnLetter + (kingCell.Row + 1).ToString());
            var oneDownCell = getCell(kingCell.ColumnLetter + (kingCell.Row - 1).ToString());

            var oneUoneLCell = getCell(GetColumnLetter(kingCell, -1) + (kingCell.Row + 1).ToString());
            var oneUoneRCell = getCell(GetColumnLetter(kingCell, 1) + (kingCell.Row + 1).ToString());
            var oneDoneLCell = getCell(GetColumnLetter(kingCell, -1) + (kingCell.Row - 1).ToString());
            var oneDoneRCell = getCell(GetColumnLetter(kingCell, 1) + (kingCell.Row - 1).ToString());

            //castling moves, maybe break out into their own function?
            if (CellIsValidForKing(kingCell, oneLeftCell, enemyPressure) && 
                CellIsValidForKing(kingCell, twoLeftCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, twoLeftCell.Coordinates, ValidBoardMove.MovePath.Castle, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(twoLeftCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }

            if (CellIsValidForKing(kingCell, oneRightCell, enemyPressure) &&
                CellIsValidForKing(kingCell, twoRightCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, twoRightCell.Coordinates, ValidBoardMove.MovePath.Castle, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(twoRightCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }


            if (CellIsValidForKing(kingCell, oneLeftCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneLeftCell.Coordinates, ValidBoardMove.MovePath.Left, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneLeftCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneRightCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneRightCell.Coordinates, ValidBoardMove.MovePath.Right, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneRightCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneUpCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUpCell.Coordinates, ValidBoardMove.MovePath.Up, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUpCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneDownCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDownCell.Coordinates, ValidBoardMove.MovePath.Down, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDownCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneUoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneLCell.Coordinates, ValidBoardMove.MovePath.UpLeft, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneUoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneUoneRCell.Coordinates, ValidBoardMove.MovePath.UpRight, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneUoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }
            if (CellIsValidForKing(kingCell, oneDoneLCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneLCell.Coordinates, ValidBoardMove.MovePath.DownLeft, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDoneLCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }

            }
            if (CellIsValidForKing(kingCell, oneDoneRCell, enemyPressure))
            {
                var moveProperty = new ValidBoardMove(coords, oneDoneRCell.Coordinates, ValidBoardMove.MovePath.DownRight, this.isWhite);
                this.ValidMoves.Add(moveProperty);
                if (samePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(oneDoneRCell.Coordinates) == -1)
                {
                    samePressure.Add(moveProperty);
                }
            }

            this.CheckForCastleCells();
        }

        private void CheckForCastleCells()
        {
            //TODO: Add castle cells to king valid moves
        }

        public bool CellIsValidForKing(Cell cellFrom, Cell cellTo, List<ValidBoardMove> enemyPressure)
        {
            var moveProps = new ValidNotationProperties();
            moveProps = moveProps.DetermineMoveProperties(cellFrom, cellTo, enemyPressure);
            if (moveProps.IsValid && !moveProps.IsProtected)
            {
                return true;
            }
            return false;
        }
    }
}