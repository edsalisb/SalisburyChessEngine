using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class King : PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell;

        public King(bool isWhite, Func<string, Cell> getCell): base(isWhite)
        {
            this.piece = pieceType.King;
            this.getCell = getCell;
        }

       
        public override string ToString()
        {
            return "K";
        }

        public override void determineValidMoves(string coords)
        {
            throw new NotImplementedException();
        }
    }
}