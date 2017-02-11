using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Rook : PieceBase
    {
        public bool isWhite;
        public pieceType piece;
        public Rook(bool isWhite)
        {
            this.isWhite = isWhite;
            this.piece = pieceType.Rook;
        }

        public override List<Cell> getValidMoves()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "R";
        }
    }
}