using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Bishop : PieceBase
    {
        public bool isWhite;
        public pieceType piece;

        public Bishop(bool isWhite)
        {
            this.piece = pieceType.Bishop;
            this.isWhite = isWhite;
        }

        public override List<Cell> getValidMoves()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "B";
        }
    }
}