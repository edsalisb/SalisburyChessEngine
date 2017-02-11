using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Queen : PieceBase
    {
        public pieceType piece;
        public bool isWhite;

        public Queen(bool isWhite)
        {
            this.piece = pieceType.Queen;
            this.isWhite = isWhite;
        }

        public override List<Cell> getValidMoves()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "Q";
        }
    }
}