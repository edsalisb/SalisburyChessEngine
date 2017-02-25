using System;
using System.Linq;
using SalisburyChessEngine.Pieces;
using SalisburyChessEngine.Moves;
using SalisburyChessEngine.Utilities;
using System.Collections.Generic;


namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard: List<List<Cell>> {
        public class AlgebraicNotationParser
        {
            private ChessBoard board;
            private string lastTwoLetters;
            private int captureIndex;
            private string leftToParse;
            private ValidNotationProperties validMoveProperties;
            private Move potentialMove;
            public AlgebraicNotationParser(ChessBoard b)
            {
                this.board = b;
                this.validMoveProperties = new ValidNotationProperties();
            }
            internal Move Parse(string algebraicCoord, bool isWhitesTurn)
            {
                this.potentialMove = new Move(algebraicCoord, isWhitesTurn);
                this.leftToParse = algebraicCoord;
                if (algebraicCoord[algebraicCoord.Length - 1] == '+')
                {
                    this.potentialMove.MustBeChecked = true;
                    this.leftToParse = this.leftToParse.Substring(0,this.leftToParse.Length - 1);
                }
                this.lastTwoLetters = this.leftToParse.Substring(this.leftToParse.Length - 2);
                if (!this.determineCellTo())
                {
                    return potentialMove;
                }
                this.determineCellFrom();
                if (this.potentialMove.MustBeChecked && potentialMove.IsValid)
                {
                    this.ensureChecked();
                }
                return potentialMove;
            }

            private void ensureChecked()
            {
                this.board.replacePiece(potentialMove);
                var piece = this.board.getCell(potentialMove.CellTo.Coordinates).CurrentPiece;
                piece.determineValidMoves(potentialMove.CellTo.Coordinates, null);

                var validMoveCoords = piece.ValidMoves.Select(GeneralUtilities.SelectCoordinates).ToList();
                if ((this.board.isWhitesTurn && validMoveCoords.IndexOf(this.board.BlackKing.CurrentCoordinates) == -1) ||
                    !this.board.isWhitesTurn && validMoveCoords.IndexOf(this.board.WhiteKing.CurrentCoordinates) == -1)
                {
                    this.potentialMove.IsValid = false;
                }
                this.board.rollback(potentialMove);
                piece = this.board.getCell(potentialMove.CellFrom.Coordinates).CurrentPiece;
                piece.determineValidMoves(potentialMove.CellFrom.Coordinates, null);

            }
           
            public bool determineCellTo()
            {
                this.potentialMove.CellTo = this.board.getCell(lastTwoLetters);
                if (potentialMove.CellTo == null)
                {
                    this.potentialMove.IsValid = false;
                    return false;
                }

                if (Cell.HasPiece(potentialMove.CellTo))
                {
                    if (this.potentialMove.AlgebraicCoord.IndexOf('x') == -1)
                    {
                        this.potentialMove.IsValid = false;
                        return false;
                    }
                    else
                    {
                        captureIndex = this.potentialMove.AlgebraicCoord.IndexOf('x');
                        if (captureIndex == 0)
                        {
                            this.potentialMove.IsValid = false;
                            return false;
                        }
                        potentialMove.IsCapturable = true;
                    }
                }
                else
                {
                    if (this.potentialMove.AlgebraicCoord.IndexOf('x') > -1)
                    {
                        this.potentialMove.IsValid = false;
                        return false;
                    }
                }

                int index = this.potentialMove.AlgebraicCoord.IndexOf(lastTwoLetters);
                this.leftToParse = this.leftToParse.Substring(0, index);
                return true;
            }
            public void determineCellFrom()
            {
                var pieceLetter = this.potentialMove.AlgebraicCoord[0];
                var filterLetter = this.potentialMove.AlgebraicCoord[1];


                if (pieceLetter == 'N')
                {
                    CheckPieceMovesBase<Knight>(filterLetter, this.board.FindWhiteKnights, this.board.FindBlackKnights);
                }
                else if (pieceLetter == 'B')
                {
                    CheckPieceMovesBase<Bishop>(filterLetter, this.board.FindWhiteBishops, this.board.FindBlackBishops);
                }
                else if (pieceLetter == 'R')
                {
                    CheckPieceMovesBase<Rook>(filterLetter, this.board.FindWhiteRooks, this.board.FindBlackRooks);
                }
                else if (pieceLetter == 'Q')
                {
                    CheckPieceMovesBase<Queen>(filterLetter, this.board.FindWhiteQueens, this.board.FindBlackQueens);
                }
                else if (pieceLetter == 'K')
                {
                    CheckPieceMovesBase<King>(filterLetter, this.board.FindWhiteKings, this.board.FindBlackKings);
                }
                else
                {
                    CheckForPawnMoves();
                }

            }
            private void CheckPieceMovesBase<T>(char filter, Func<List<T>> getWhitePieceList, Func<List<T>> getBlackPieceList) where T : PieceBase
            {
                List<T> friendlyPieceList;
                if (this.potentialMove.isWhitesTurn)
                {
                    friendlyPieceList = getWhitePieceList();
                }
                else
                {
                    friendlyPieceList = getBlackPieceList();
                }
                foreach (var piece in friendlyPieceList)
                {
                    var friendlyPiece = (PieceBase)piece;
                    int index = friendlyPiece.ValidMoves.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.lastTwoLetters);
                    if (index > -1)
                    {
                        if (this.potentialMove.IsCapturable)
                        {
                            var friendlyMoveCoordinates = friendlyPiece.ValidMoves.Select(GeneralUtilities.SelectCoordinates).ToList();
                            int captureIndex = friendlyMoveCoordinates.IndexOf(lastTwoLetters);
                            if (captureIndex > -1)
                            {
                                this.potentialMove.IsValid = true;
                            }

                            else
                            {
                                return;
                            }
                            
                        }
                        this.potentialMove.IsValid = true;
                        this.potentialMove.CellFrom = this.board.getCell(friendlyPiece.CurrentCoordinates);
                        break;

                    }
                }
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
                var columnFrom = this.potentialMove.AlgebraicCoord[captureIndex - 1];
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
                        potentialCellFrom = this.board.getCell(columnFrom.ToString() + (rowTo - 1).ToString());
                    }
                    else
                    {
                        potentialCellFrom = this.board.getCell(columnFrom.ToString() + (rowTo + 1).ToString());
                    }

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
                    return;
                }
            }

            public void CheckForNonCapturePawnMoves()
            {
                if (this.potentialMove.isWhitesTurn)
                {
                    if (lastTwoLetters[1] == '4')
                    {
                        var whiteInitialCell = this.board.getCell(lastTwoLetters[0] + '2'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(whiteInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = whiteInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = this.board.getCell(lastTwoLetters[0] + '3'.ToString());
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
                            var potentialCellFrom = this.board.getCell(lastTwoLetters[0] + (rowNum - 1).ToString());
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
                        var blackInitialCell = this.board.getCell(lastTwoLetters[0] + '7'.ToString());
                        this.validMoveProperties = this.validMoveProperties.determineMoveProperties(blackInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid)
                        {
                            this.potentialMove.CellFrom = blackInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = this.board.getCell(lastTwoLetters[0] + '6'.ToString());
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
                            var potentialCellFrom = this.board.getCell(lastTwoLetters[0] + (rowNum + 1).ToString());
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