using System;
using System.Collections.Generic;
namespace SalisburyChessEngine
{
    public class Game
    {
        private string blackMode;
        private string whiteMode;
        private ChessBoard cb;
        public List<Move> moveList;
        private bool gameEnded;
        private bool isWhitesTurn;
        private byte turnNumber;
        public Game(string whiteMode, string blackMode)
        {
            this.whiteMode = whiteMode;
            this.blackMode = blackMode;
            this.isWhitesTurn = true;
            this.turnNumber = 1;

            this.gameEnded = false;
            this.moveList = new List<Move>();
            if (this.whiteMode == "ai" || this.blackMode == "ai")
            {
                Console.WriteLine("Not Supported Yet");
                return;
            }
        }

        public void Begin()
        {
            this.cb = new ChessBoard();
            while (!this.gameEnded)
            {
                Console.WriteLine("Move:");
                string algebraicCoord = Console.ReadLine();
                Move potentialMove;
                if (this.cb.TryMovePiece(algebraicCoord, this.isWhitesTurn,out potentialMove))
                {
                    moveList.Add(potentialMove);
                    this.cb.replacePiece(potentialMove.CellFrom, potentialMove.CellTo);
                    this.cb.UpdateBoardState();

                    this.UpdateGameStatus();
                }
            }
        }

        private void UpdateGameStatus()
        {
            if (this.isWhitesTurn)
            {
                this.isWhitesTurn = false;
            }
            else
            {
                this.isWhitesTurn = true;
                this.turnNumber++;
            }
        }
    }
}