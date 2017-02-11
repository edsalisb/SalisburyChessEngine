using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class King : PieceBase
    {
        public pieceType piece;
        public bool isWhite;

        public King(bool isWhite)
        {
            this.piece = pieceType.King;
            this.isWhite = isWhite;
        }

        public override List<Cell> getValidMoves()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "K";
        }
    }
}