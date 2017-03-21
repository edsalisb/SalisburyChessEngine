using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using SalisburyChessEngine.Pieces;
using SalisburyChessEngine.Board;

//A FEN "record" defines a particular game position, all in one text line and using only the ASCII character set.A text file with only FEN data records should have the file extension ".fen".[1]

//A FEN record contains six fields. The separator between fields is a space. The fields are:

//1. Piece placement (from white's perspective). Each rank is described, starting with rank 8 and ending with rank 1; within each rank, the contents of each square are described from file "a" through file "h". Following the Standard Algebraic Notation (SAN), each piece is identified by a single letter taken from the standard English names (pawn = "P", knight = "N", bishop = "B", rook = "R", queen = "Q" and king = "K").[1] White pieces are designated using upper-case letters ("PNBRQK") while black pieces use lowercase ("pnbrqk"). Empty squares are noted using digits 1 through 8 (the number of empty squares), and "/" separates ranks.
//2. Active color. "w" means White moves next, "b" means Black.
//3. Castling availability. If neither side can castle, this is "-". Otherwise, this has one or more letters: "K" (White can castle kingside), "Q" (White can castle queenside), "k" (Black can castle kingside), and/or "q" (Black can castle queenside).
//4. En passant target square in algebraic notation.If there's no en passant target square, this is "-". If a pawn has just made a two-square move, this is the position "behind" the pawn. This is recorded regardless of whether there is a pawn in position to make an en passant capture.[2]
//5. Halfmove clock: This is the number of halfmoves since the last capture or pawn advance.This is used to determine if a draw can be claimed under the fifty-move rule.
//6. Fullmove number: The number of the full move. It starts at 1, and is incremented after Black's move.

namespace SalisburyChessEngine.Board.Positions
{
    public class FENNotationPosition : BoardPosition
    {

        private enum FenRecordFields {
            PiecePlacement = 0,
            ActiveColor = 1,
            CastlingAvailability = 2,
            EnPessantSquare = 3,
            HalfMoveClock = 4,
            FullMoveNumber = 5
        }
        public List<string> FENContents { get; private set; }
        public bool? IsWhitesTurn { get; private set; }

        public bool IsWhiteQueenSideCastleValid { get; private set; }

        public bool IsBlackQueenSideCastleValid { get; private set; }

        public bool IsWhiteKingSideCastleValid { get; private set; }

        public bool IsBlackKingSideCastleValid { get; private set; }

        public string EnPessantSquare { get; private set; }
        public int TurnNumber { get; private set; }
        public int HalfMoveNumber { get; private set; }

        public FENNotationPosition() : base()
        {
            IsWhiteKingSideCastleValid = false;
            IsWhiteQueenSideCastleValid = false;
            IsBlackKingSideCastleValid = false;
            IsBlackQueenSideCastleValid = false;
        }
        public FENNotationPosition(List<string> fenContents) : this()
        {
            //To Account for lichess FEN notation board editor not providing turn numbers (count = 4), I am going to default the halfMoveClock and 
            //Full Move number to 1 if they are not found

            this.Clear();
            this.FENContents = fenContents;

            if (this.FENContents.Count == 4)
            {
                this.FENContents.Add("1");
                this.FENContents.Add("1");
            }

            this.DeterminePositionInformation();
        }

        public void DeterminePositionInformation()
        {
            

            try
            {
                

                DeterminePiecePlacement(this.FENContents[(int)FenRecordFields.PiecePlacement]);
                DetermineActiveColor(this.FENContents[(int)FenRecordFields.ActiveColor]);
                DetermineCastlingAvailability(this.FENContents[(int)FenRecordFields.CastlingAvailability]);
                DetermineEnPessantSquare(this.FENContents[(int)FenRecordFields.EnPessantSquare]);
                DetermineHalfMoveAmount(this.FENContents[(int)FenRecordFields.HalfMoveClock]);
                DetermineTurnNumber(this.FENContents[(int)FenRecordFields.FullMoveNumber]);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Not Valid FEN Notation");
            }
            
        }
        private void DeterminePiecePlacement(string notation)
        {
            bool isWaiting = false;
            int continueLoopCounter = 0;
            int fenCharNumber = 0;
            List<string> rows = notation.Split('/').Reverse().ToList();
            for (var i = BoardProperties.Rows; i >= 1; i--)
            {
                try
                {
                    var row = rows[i - 1];
                    
                    var columnNumber = 1;
                    foreach(var fenChar in row) 
                    {
                        if (BoardProperties.ColumnNumbersMappedToLetters.TryGetValue(columnNumber, out char columnletter))
                        {

                            if (isWaiting)
                            {
                                continueLoopCounter++;
                                if (continueLoopCounter == fenCharNumber)
                                {
                                    isWaiting = false;
                                }
                                continue;
                            }

                            if (char.IsLetter(fenChar))
                            {
                                var coordinates = columnletter.ToString() + i;
                                this.Add(coordinates, fenChar);
                                columnNumber++;
                            }
                            else if (char.IsDigit(fenChar))
                            {
                                int.TryParse(fenChar.ToString(), out fenCharNumber);
                                columnNumber += fenCharNumber;
                            }
                            else
                            {
                                throw new NotSupportedException("Invalid Character" + fenChar.ToString());
                            }
                        }
                    }
                }
                catch(IndexOutOfRangeException ex)
                {
                    return;
                }
            }
        }

        private void DetermineActiveColor(string color)
        {
            if (color == "w")
            {
                IsWhitesTurn = true;
                return;
            }
            else if (color == "b")
            {
                IsWhitesTurn = false;
                return;
            }
            return;
        }

        private void DetermineCastlingAvailability(string castleString)
        {
            if (castleString.IndexOf("-") > -1)
            {
                return;
            }

            if (castleString.IndexOf("Q") > -1){
                IsWhiteQueenSideCastleValid = true;
            }
            if (castleString.IndexOf("K") > -1)
            {
                IsWhiteKingSideCastleValid = true;
            }
            if (castleString.IndexOf("q") > -1)
            {
                IsBlackQueenSideCastleValid = true;
            }
            if (castleString.IndexOf("k") > -1)
            {
                IsBlackKingSideCastleValid = true;
            }

        }

        private void DetermineEnPessantSquare(string enPessantString)
        {
            if (enPessantString == "-")
            {
                return;
            }
            this.EnPessantSquare = enPessantString;
        }
        private void DetermineHalfMoveAmount(string halfMove)
        {
            if (int.TryParse(halfMove, out int halfMoveNumber))
            {
                this.HalfMoveNumber = halfMoveNumber;
            }
            else
            {
                throw new Exception();
            }
        }
        private void DetermineTurnNumber(string turn)
        {
            if (int.TryParse(turn, out int turnNumber))
            {
                this.TurnNumber = turnNumber;
            }
            else
            {
                throw new Exception();
            }
        }
    }
}