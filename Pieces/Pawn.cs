using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Pawn: PieceBase
    {
        public bool isWhite;
        public pieceType piece;
        public Pawn(bool isWhite)
        {
            this.piece = pieceType.Pawn;
            this.isWhite = isWhite;
        }

        public override List<Cell> getValidMoves()
        {
            return null;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}