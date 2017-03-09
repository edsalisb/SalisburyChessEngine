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
                if (algebraicCoord.Length < 2)
                {
                    return potentialMove;
                }

                this.potentialMove = new Move(algebraicCoord, isWhitesTurn);
                if (algebraicCoord == "O-O")
                {
                    this.DetermineIfKingSideCastleValid(isWhitesTurn);
                    return potentialMove;
                }
                else if (algebraicCoord == "O-O-O")
                {
                    this.DetermineIfQueenSideCastleValid(isWhitesTurn);
                    return potentialMove;
                }

                algebraicCoord = algebraicCoord.Trim();
                this.leftToParse = algebraicCoord;
                if (algebraicCoord[algebraicCoord.Length - 1] == '+')
                {
                    this.potentialMove.MustBeChecked = true;
                    this.leftToParse = this.leftToParse.Substring(0,this.leftToParse.Length - 1);
                }
                this.lastTwoLetters = this.leftToParse.Substring(this.leftToParse.Length - 2);
                if (!this.DetermineCellTo())
                {
                    return potentialMove;
                }
                this.DetermineCellFrom();
                if (this.potentialMove.MustBeChecked && potentialMove.IsValid)
                {
                    this.EnsureChecked();
                }
                return potentialMove;
            }

            private void DetermineIfQueenSideCastleValid(bool isWhitesTurn)
            {
                if (isWhitesTurn)
                {
                    var kingCell = this.board.GetCell("e1");
                    var rookCell = this.board.GetCell("a1");
                    var emptyCells = new List<Cell>()
                    { 
                    
                        this.board.GetCell("b1"),
                        this.board.GetCell("c1"),
                        this.board.GetCell("d1")
                    };

                    this.FindIfCastleIsValidBase(isWhitesTurn, kingCell, rookCell, emptyCells);

                }
                else
                {
                    var kingCell = this.board.GetCell("e8");
                    var rookCell = this.board.GetCell("a8");
                    var emptyCells = new List<Cell>()
                    {

                        this.board.GetCell("b8"),
                        this.board.GetCell("c8"),
                        this.board.GetCell("d8")
                    };

                    this.FindIfCastleIsValidBase(isWhitesTurn, kingCell, rookCell, emptyCells);
                }
            }

            private void DetermineIfKingSideCastleValid(bool isWhitesTurn)
            {
                if (isWhitesTurn)
                {


                    var kingCell = this.board.GetCell("e1");
                    var rookCell = this.board.GetCell("h1");
                    var emptyCells = new List<Cell>()
                    {
                        this.board.GetCell("f1"),
                        this.board.GetCell("g1")
                    };

                    this.FindIfCastleIsValidBase(isWhitesTurn,kingCell, rookCell, emptyCells);

                }
                else
                {
                    var kingCell = this.board.GetCell("e8");
                    var rookCell = this.board.GetCell("h8");
                    var emptyCells = new List<Cell>()
                    {
                        this.board.GetCell("f8"),
                        this.board.GetCell("g8")
                    };

                    this.FindIfCastleIsValidBase(isWhitesTurn, kingCell, rookCell, emptyCells);
                }
            }

            private bool FindIfCastleIsValidBase(bool isWhitesTurn, Cell kingCell, Cell rookCell, List<Cell> emptyCells)
            {
                List<string> boardPressureCoordinates;

                if (isWhitesTurn)
                {
                    boardPressureCoordinates = this.board.BlackPiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList();
                }
                else
                {
                    boardPressureCoordinates = this.board.WhitePiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList();
                }
                foreach(var cell in emptyCells)
                {
                    if (Cell.HasPiece(cell))
                    {
                        return false;
                    }
                    if (boardPressureCoordinates.IndexOf(cell.Coordinates) > -1)
                    {
                        return false;
                    }
                }
                
                if (this.board.IsKingOnCell(kingCell, out King king) && this.board.IsRookOnCell(rookCell, out Rook rook))
                {
                    if ((king.isWhite && rook.isWhite || !king.isWhite && !rook.isWhite) && !king.HasMoved && !rook.HasMoved)
                    {
                        this.potentialMove.IsValid = true;
                        if (rook.CurrentCoordinates[0] == 'a')
                        {
                            this.potentialMove.IsQueenSideCastle = true;
                        }
                        else
                        {
                            this.potentialMove.IsKingSideCastle = true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                return true;
            }

            private void EnsureChecked()
            {
                this.board.ReplacePiece(potentialMove);
                var piece = this.board.GetCell(potentialMove.CellTo.Coordinates).CurrentPiece;
                piece.DetermineValidMoves(potentialMove.CellTo.Coordinates, null);

                var validMoveCoords = piece.ValidMoves.Select(GeneralUtilities.SelectCoordinates).ToList();
                if ((this.board.IsWhitesTurn && validMoveCoords.IndexOf(this.board.BlackKing.CurrentCoordinates) == -1) ||
                    !this.board.IsWhitesTurn && validMoveCoords.IndexOf(this.board.WhiteKing.CurrentCoordinates) == -1)
                {
                    this.potentialMove.IsValid = false;
                }
                this.board.Rollback(potentialMove);
                piece = this.board.GetCell(potentialMove.CellFrom.Coordinates).CurrentPiece;
                piece.DetermineValidMoves(potentialMove.CellFrom.Coordinates, null);

            }
           
            public bool DetermineCellTo()
            {
                this.potentialMove.CellTo = this.board.GetCell(lastTwoLetters);
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
            public void DetermineCellFrom()
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
                if (this.potentialMove.IsWhitesTurn)
                {
                    friendlyPieceList = getWhitePieceList();
                }
                else
                {
                    friendlyPieceList = getBlackPieceList();
                }

                if (filter == this.leftToParse[this.leftToParse.Length - 1] &&
                    filter != 'x')
                {
                    if (int.TryParse(filter.ToString(), out int rowNum))
                    {
                        //filter on row number
                        friendlyPieceList = friendlyPieceList.Where(x => x.CurrentCoordinates[1] == filter).ToList();
                    }
                    else
                    {
                        //filter on column letter
                        friendlyPieceList = friendlyPieceList.Where(x => x.CurrentCoordinates[0] == filter).ToList();
                    }
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
                        this.potentialMove.CellFrom = this.board.GetCell(friendlyPiece.CurrentCoordinates);
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
                if (int.TryParse(lastTwoLetters[1].ToString(), out int rowTo))
                {
                    Cell potentialCellFrom;
                    if (this.potentialMove.IsWhitesTurn)
                    {
                        potentialCellFrom = this.board.GetCell(columnFrom.ToString() + (rowTo - 1).ToString());
                    }
                    else
                    {
                        potentialCellFrom = this.board.GetCell(columnFrom.ToString() + (rowTo + 1).ToString());
                    }

                    this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
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
                if (this.potentialMove.IsWhitesTurn)
                {
                    if (lastTwoLetters[1] == '4')
                    {
                        var whiteInitialCell = this.board.GetCell(lastTwoLetters[0] + '2'.ToString());
                        this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(whiteInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid && whiteInitialCell.CurrentPiece.ValidMoves.Count > 0)
                        {
                            this.potentialMove.CellFrom = whiteInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = this.board.GetCell(lastTwoLetters[0] + '3'.ToString());
                        this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(whiteOneRow, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid && whiteOneRow.CurrentPiece.ValidMoves.Count > 0)
                        {
                            this.potentialMove.CellFrom = whiteOneRow;
                            this.potentialMove.IsValid = true;
                            return;
                        }
                    }
                    else
                    {
                        if (int.TryParse(lastTwoLetters[1].ToString(), out int rowNum))
                        {
                            var potentialCellFrom = this.board.GetCell(lastTwoLetters[0] + (rowNum - 1).ToString());
                            this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
                            if (this.validMoveProperties.IsValid && potentialCellFrom.CurrentPiece.ValidMoves.Count > 0)
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
                        var blackInitialCell = this.board.GetCell(lastTwoLetters[0] + '7'.ToString());
                        this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(blackInitialCell, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid && blackInitialCell.CurrentPiece.ValidMoves.Count > 0)
                        {
                            this.potentialMove.CellFrom = blackInitialCell;
                            this.potentialMove.IsValid = true;
                            return;
                        }

                        var whiteOneRow = this.board.GetCell(lastTwoLetters[0] + '6'.ToString());
                        this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(whiteOneRow, this.potentialMove.CellTo);
                        if (this.validMoveProperties.IsValid && whiteOneRow.CurrentPiece.ValidMoves.Count > 0)
                        {
                            this.potentialMove.CellFrom = whiteOneRow;
                            this.potentialMove.IsValid = true;
                            return;
                        }
                    }
                    else
                    {
                        if (int.TryParse(lastTwoLetters[1].ToString(), out int rowNum))
                        {
                            var potentialCellFrom = this.board.GetCell(lastTwoLetters[0] + (rowNum + 1).ToString());
                            this.validMoveProperties = this.validMoveProperties.DetermineMoveProperties(potentialCellFrom, this.potentialMove.CellTo);
                            if (this.validMoveProperties.IsValid && potentialCellFrom.CurrentPiece.ValidMoves.Count > 0)
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