using System;
using SalisburyChessEngine.Pieces;
using System.Collections.Generic;

namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard: List<List<Cell>> {
        public class AlgebraicNotationParser
        {
            private ChessBoard board;
            private Func<string, Cell> getCell;
            private string lastTwoLetters;
            private int captureIndex;
            private string algebraicCoord;
            private ValidNotationProperties validMoveProperties;
            private Move potentialMove;
            public AlgebraicNotationParser(ChessBoard b)
            {
                this.board = b;
                this.validMoveProperties = new ValidNotationProperties();
            }
            internal Move Parse(string algebraicCoord, bool isWhitesTurn)
            {
                this.potentialMove = new Move(isWhitesTurn);
                this.algebraicCoord = algebraicCoord;
                if (algebraicCoord == null)
                {
                    this.potentialMove.IsValid = false;
                    return this.potentialMove;
                }
                if (algebraicCoord.Length < 2)
                {
                    return null;
                }

                this.lastTwoLetters = algebraicCoord.Substring(algebraicCoord.Length - 2);
                potentialMove.CellTo = this.getCell(lastTwoLetters);
                if (potentialMove.CellTo == null)
                {
                    this.potentialMove.IsValid = false;
                    return this.potentialMove;
                }

                if (potentialMove.CellTo.CurrentPiece != null)
                {
                    if (algebraicCoord.IndexOf('x') == -1)
                    {
                        this.potentialMove.IsValid = false;
                        return this.potentialMove;
                    }
                    else
                    {
                        captureIndex = algebraicCoord.IndexOf('x');
                        if (captureIndex == 0)
                        {
                            this.potentialMove.IsValid = false;
                            return this.potentialMove;
                        }
                        potentialMove.IsCapturable = true;
                    }
                }

                this.determineCellFrom();
                return potentialMove;
            }

            public void determineCellFrom()
            {
                var pieceLetter = algebraicCoord[0];

                if (pieceLetter == 'N')
                {
                    CheckForKnightMoves();
                }
                else if (pieceLetter == 'B')
                {
                    CheckForBishopMoves();
                }
                else if (pieceLetter == 'R')
                {
                    CheckForRookMoves();
                }
                else if (pieceLetter == 'Q')
                {
                    CheckForQueenMoves();
                }
                else if (pieceLetter == 'K')
                {
                    CheckForKingMoves();
                }
                else
                {
                    CheckForPawnMoves();
                }

            }
            private void CheckForKnightMoves()
            {

            }

            private void CheckForBishopMoves()
            {
                throw new NotImplementedException();
            }

            private void CheckForRookMoves()
            {
                throw new NotImplementedException();
            }

            private void CheckForQueenMoves()
            {
                throw new NotImplementedException();
            }

            private void CheckForKingMoves()
            {
                throw new NotImplementedException();
            }

            public void CheckForPawnMoves()
            {
                if (this.potentialMove.IsCapturable)
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
                    if (this.potentialMove.isWhitesTurn)
                    {
                        potentialCellFrom = getCell(columnFrom.ToString() + (rowTo - 1).ToString());
                    }
                    else
                    {
                        potentialCellFrom = getCell(columnFrom.ToString() + (rowTo + 1).ToString());
                    }

                    this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
                    if (this.validMoveProperties.IsValid)
                    {
                        this.potentialMove.CellFrom = potentialCellFrom;
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
                if (this.potentialMove.isWhitesTurn)
                {
                    if (lastTwoLetters[1] == '4')
                    {
                        var whiteInitialCell = getCell(lastTwoLetters[0] + '2'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = whiteInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = getCell(lastTwoLetters[0] + '3'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteOneRow, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = whiteOneRow;
                            this.potentialMove.IsValid = true;
                            return;
                        }
                    }
                    else
                    {
                        int rowNum;
                        if (int.TryParse(lastTwoLetters[1].ToString(), out rowNum))
                        {
                            var potentialCellFrom = getCell(lastTwoLetters[0] + (rowNum - 1).ToString());
                            this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
                            if (this.validMoveProperties.IsValid)
                            {
                                this.potentialMove.CellFrom = potentialCellFrom;
                                this.potentialMove.IsValid = true;
                                return;
                            }

                        }
                        else
                        {
                            this.potentialMove.IsValid = false;
                            return;
                        }
                    }
                }
                else
                {
                    if (lastTwoLetters[1] == '5')
                    {
                        var blackInitialCell = getCell(lastTwoLetters[0] + '7'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(blackInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = blackInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = getCell(lastTwoLetters[0] + '6'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteOneRow, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = whiteOneRow;
                            this.potentialMove.IsValid = true;
                            return;
                        }
                    }
                    else
                    {
                        int rowNum;
                        if (int.TryParse(lastTwoLetters[1].ToString(), out rowNum))
                        {
                            var potentialCellFrom = getCell(lastTwoLetters[0] + (rowNum + 1).ToString());
                            this.validMoveProperties = this.validMoveProperties.determineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
                            if (this.validMoveProperties.IsValid)
                            {
                                this.potentialMove.CellFrom = potentialCellFrom;
                                this.potentialMove.IsValid = true;
                                return;
                            }

                        }
                        else
                        {
                            this.potentialMove.IsValid = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}