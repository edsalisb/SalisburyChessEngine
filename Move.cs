using System;
using SalisburyChessEngine.Pieces;


namespace SalisburyChessEngine
{
    public class Move
    {
        private Func<string, Cell> getCell;
        private string lastTwoLetters;
        private bool isWhitesTurn;
        private int captureIndex;
        private string algebraicCoord;
        public bool IsValid { get; set; }
        public bool IsCapturable { get; set; }
        public Cell CellFrom { get; set; }
        public Cell CellTo { get; set; }
        private ValidMoveProperties validMoveProperties;

        public Move(Func<string, Cell> getCell, bool isWhitesTurn)
        {
            this.isWhitesTurn = isWhitesTurn;
            this.getCell = getCell;
            this.CellFrom = null;
            this.CellTo = null;
            this.validMoveProperties = new ValidMoveProperties();
        }
        internal void Parse(string algebraicCoord)
        {
            this.algebraicCoord = algebraicCoord;
            if (algebraicCoord == null)
            {
                IsValid = false;
                return;
            }
            if (algebraicCoord.Length < 2)
            {
                IsValid = false;
                return;
            }

            this.lastTwoLetters = algebraicCoord.Substring(algebraicCoord.Length - 2);
            this.CellTo = this.getCell(lastTwoLetters);
            if (this.CellTo == null)
            {
                IsValid = false;
            }

            if (this.CellTo.CurrentPiece != null)
            {
                if (algebraicCoord.IndexOf('x') == -1)
                {
                    IsValid = false;
                    return;
                }
                else
                {
                    captureIndex = algebraicCoord.IndexOf('x');
                    if (captureIndex == 0)
                    {
                        return;
                    }
                    IsCapturable = true;
                }
            }

            this.determineCellFrom();
            //if (algebraicCoord.Length > 2)
            //{
            //    char pieceLetter = algebraicCoord[0];
            //    if (pieceLetter != 'B' && pieceLetter != 'K' &&
            //        pieceLetter != 'N' && pieceLetter != 'R' && pieceLetter != 'Q')
            //    {
            //        return;
            //    }



            //}
            return;
        }

        public void determineCellFrom()
        {
            CheckForPawnMoves();

        }

        public void CheckForPawnMoves()
        {
            if (IsCapturable)
            {
                this.CheckPawnCaptureMoves();
            }
            else
            {
                this.CheckForNonCapturePawnMoves();
            }

        }

        public void CheckPawnCaptureMoves()
        {
            var columnFrom = this.algebraicCoord[captureIndex - 1];
            var columnDifference = Math.Abs(lastTwoLetters[0] - columnFrom);

            if (columnDifference != 1)
            {
                return;
            }

            int rowTo;
            if (int.TryParse(lastTwoLetters[1].ToString(), out rowTo))
            {
                Cell potentialCellFrom;
                if (isWhitesTurn)
                {
                    potentialCellFrom = getCell(columnFrom.ToString() + (rowTo - 1).ToString());
                }
                else
                {
                    potentialCellFrom = getCell(columnFrom.ToString() + (rowTo + 1).ToString());
                }

                this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, CellTo);
                if (this.validMoveProperties.IsValid)
                {
                    this.CellFrom = potentialCellFrom;
                    IsValid = true;
                    return;
                }
            }
            else
            {
                return;
            }
        }

        public void CheckForNonCapturePawnMoves()
        {
            if (isWhitesTurn)
            {
                if (lastTwoLetters[1] == '4')
                {
                    var whiteInitialCell = getCell(lastTwoLetters[0] + '2'.ToString());
                    this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteInitialCell, CellTo);
                    if (this.validMoveProperties.IsValid)
                    {
                        this.CellFrom = whiteInitialCell;
                        IsValid = true;
                        return;
                    }

                    var whiteOneRow = getCell(lastTwoLetters[0] + '3'.ToString());
                    this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteOneRow, CellTo);
                    if (this.validMoveProperties.IsValid)
                    {
                        this.CellFrom = whiteOneRow;
                        IsValid = true;
                        return;
                    }
                }
                else
                {
                    int rowNum;
                    if (int.TryParse(lastTwoLetters[1].ToString(), out rowNum))
                    {
                        var potentialCellFrom = getCell(lastTwoLetters[0] + (rowNum - 1).ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.CellFrom = potentialCellFrom;
                            IsValid = true;
                            return;
                        }

                    }
                    else
                    {
                        IsValid = false;
                        return;
                    }
                }
            }
            else
            {
                if (lastTwoLetters[1] == '5')
                {
                    var blackInitialCell = getCell(lastTwoLetters[0] + '7'.ToString());
                    this.validMoveProperties = this.validMoveProperties.determineMoveProperties(blackInitialCell, CellTo);
                    if (this.validMoveProperties.IsValid)
                    {
                        this.CellFrom = blackInitialCell;
                        IsValid = true;
                        return;
                    }

                    var whiteOneRow = getCell(lastTwoLetters[0] + '6'.ToString());
                    this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteOneRow, CellTo);
                    if (this.validMoveProperties.IsValid)
                    {
                        this.CellFrom = whiteOneRow;
                        IsValid = true;
                        return;
                    }
                }
                else
                {
                    int rowNum;
                    if (int.TryParse(lastTwoLetters[1].ToString(), out rowNum))
                    {
                        var potentialCellFrom = getCell(lastTwoLetters[0] + (rowNum + 1).ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.CellFrom = potentialCellFrom;
                            IsValid = true;
                            return;
                        }

                    }
                    else
                    {
                        IsValid = false;
                        return;
                    }
                }
            }
        }
    }
}