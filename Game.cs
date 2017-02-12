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
        public Game(string whiteMode, string blackMode)
        {
            this.whiteMode = whiteMode;
            this.blackMode = blackMode;
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
                this.cb.TryMovePiece(algebraicCoord, out potentialMove);
                if (potentialMove != null)
                {
                    moveList.Add(potentialMove);
                }
            }
        }
    }
}