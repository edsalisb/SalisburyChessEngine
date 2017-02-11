using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    public abstract class PieceBase
    {
        public enum pieceType
        {
            Queen = 9,
            Bishop = 3,
            Knight = 3,
            Rook = 5,
            Pawn = 1,
            King = 0
        }
        public bool isWhite;
        public abstract List<Cell> getValidMoves();
    }
}