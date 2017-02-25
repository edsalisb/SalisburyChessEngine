﻿using System;
using System.Collections.Generic;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Moves;

namespace SalisburyChessEngine
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
        public List<object>DestroyedBlackPieces { get; set; }
        public List<object> DestroyedWhitePieces { get; set; }

        public Move MostRecentMove { get; set; }
        public Game(string whiteMode, string blackMode)
        {
            this.whiteMode = whiteMode;
            this.blackMode = blackMode;
            this.turnNumber = 1;

            this.gameEnded = false;
            this.moveList = new List<Move>();
            this.DestroyedBlackPieces = new List<object>();
            this.DestroyedWhitePieces = new List<object>();
            if (this.whiteMode == "ai" || this.blackMode == "ai")
            {
                Console.WriteLine("Not Supported Yet");
                return;
            }
        }

        public void Begin()
        {
            this.cb = new ChessBoard();
            this.cb.BlackKing.onCheckCallbacks += UpdateMoveNotation;
            this.cb.WhiteKing.onCheckCallbacks += UpdateMoveNotation;

            this.cb.BlackKing.onCheckForCheckMateCallbacks += CheckIfCheckMateOccurred;
            this.cb.WhiteKing.onCheckForCheckMateCallbacks += CheckIfCheckMateOccurred;

            while (!this.gameEnded)
            {
                this.displayTurnStatus();

                Console.WriteLine("Move:");
                string algebraicCoord = Console.ReadLine();
                Move move;
                if (this.cb.TryMovePiece(algebraicCoord, out move))
                {
                    this.MostRecentMove = move;

                    this.cb.replacePiece(move);
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

        private void displayTurnStatus()
        {
            string output = "";
            if (this.cb.isWhitesTurn)
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
            if (this.cb.isWhitesTurn)
            {
                this.cb.isWhitesTurn = false;
            }
            else
            {
                this.cb.isWhitesTurn = true;
                this.turnNumber++;
            }
        }
    }
}