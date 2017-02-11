using System;

namespace SalisburyChessEngine
{
    public class Game
    {
        private string blackMode;
        private string whiteMode;
        private ChessBoard cb;
        public Game(string whiteMode, string blackMode)
        {
            this.whiteMode = whiteMode;
            this.blackMode = blackMode;
            if (this.whiteMode == "ai" || this.blackMode == "ai")
            {
                Console.WriteLine("Not Supported Yet");
                return;
            }
        }

        public void Begin()
        {
            this.cb = new ChessBoard();
            while (!this.cb.gameEnded)
            {
                Console.WriteLine("Move:");
                string algebraicCoord = Console.ReadLine();
                if (!this.cb.TryMovePiece(algebraicCoord))
                {
                    //
                }
                else
                {
                    //moveSuccessful
                    throw new NotImplementedException();
                }
            }
        }
    }
}