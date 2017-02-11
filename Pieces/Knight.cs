using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public class Knight : PieceBase
    {
        public bool isWhite;
        public pieceType piece; 
        public Knight(bool isWhite)
        {
            this.isWhite = isWhite;
            this.piece = pieceType.Knight;
        }

        public override List<Cell> getValidMoves()
        {
            return null;
        }
        public override string ToString()
        {
            return "N";
        }
    }
}