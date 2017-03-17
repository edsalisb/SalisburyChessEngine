using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board.Positions;
using SalisburyChessEngine.Moves;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine.Board
{
    public class Game
    {
        private string blackMode;
        private string whiteMode;
        private ChessBoard cb;
        public List<Move> moveList;
        private bool gameEnded;
        private byte turnNumber;

        //TODO: add to destroyed pieces List
        public List<PieceBase>DestroyedBlackPieces { get; set; }
        public List<PieceBase> DestroyedWhitePieces { get; set; }

        public Move MostRecentMove { get; set; }

        private Game()
        {
            this.moveList = new List<Move>();
            this.DestroyedBlackPieces = new List<PieceBase>();
            this.DestroyedWhitePieces = new List<PieceBase>();
            this.gameEnded = false;
        }
        public Game(string whiteMode, string blackMode) : this()
        {
            this.whiteMode = whiteMode;
            this.blackMode = blackMode;
            this.turnNumber = 1;
            
            if (this.whiteMode == "ai" || this.blackMode == "ai")
            {
                Console.WriteLine("Not Supported Yet");
                return;
            }
        }

        public Game(string whiteMode, string blackMode, FENNotationPosition fnp): this(whiteMode, blackMode)
        {
            throw new NotImplementedException();
        }

        public void Begin()
        {
            this.cb = new ChessBoard(new BoardPosition());
            this.cb.BlackKing.OnCheckCallbacks += UpdateMoveNotation;
            this.cb.WhiteKing.OnCheckCallbacks += UpdateMoveNotation;

            this.cb.BlackKing.OnCheckForCheckMateCallbacks += CheckIfCheckMateOccurred;
            this.cb.WhiteKing.OnCheckForCheckMateCallbacks += CheckIfCheckMateOccurred;

            while (!this.gameEnded)
            {
                this.DisplayTurnStatus();

                Console.WriteLine("Move:");
                string algebraicCoord = Console.ReadLine();
                if(algebraicCoord == "-print")
                {
                    this.PrintMoves();
                    continue;
                }
                if (this.cb.TryMovePiece(algebraicCoord, out Move move))
                {
                    this.MostRecentMove = move;

                    this.cb.ReplacePiece(move);
                    this.cb.UpdateBoardState();
                    this.UpdateGameStatus();

                    moveList.Add(this.MostRecentMove);
                }
                else
                {
                    Console.WriteLine("Invalid Move entered");
                }
            }
        }

        private void PrintMoves()
        {
            var turnCount = 1;
            for (var i = 0; i < this.moveList.Count; i++)
            {
                var move = this.moveList[i];
                if (i % 2 == 0)
                {
                    //1. 2. 3. 
                    Console.Write(turnCount.ToString() + ". ");
                    turnCount++;
                }
                Console.Write(move.AlgebraicCoord + " ");
            }
            Console.WriteLine();
        }

        public void CheckIfCheckMateOccurred(Pieces.King k, EventArgs e)
        {
            if (k.ValidMoves.Count == 0)
            {
                var moves = this.cb.GetAllMovesForTeam(k.isWhite);
                if (moves.Count == 0)
                {
                    this.gameEnded = true;
                }
            }
        }
        public void UpdateMoveNotation()
        {
            if (this.MostRecentMove == null)
            {
                return;
            }
            this.MostRecentMove.AlgebraicCoord += '+';
        }

        private void DisplayTurnStatus()
        {
            string output = "";
            if (this.cb.IsWhitesTurn)
            {
                output += "White's turn: ";
            }
            else
            {
                output += "Black's turn: ";
            }

            output += "Turn Number: " + this.turnNumber.ToString();
            Console.WriteLine(output);
        }

        private void UpdateGameStatus()
        {
            if (this.cb.IsWhitesTurn)
            {
                this.cb.IsWhitesTurn = false;
            }
            else
            {
                this.cb.IsWhitesTurn = true;
                this.turnNumber++;
            }
        }
    }
}