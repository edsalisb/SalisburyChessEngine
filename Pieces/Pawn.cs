using System;
using System.Collections.Generic;

namespace SalisburyChessEngine.Pieces
{
    internal class Pawn: PieceBase
    {
        public pieceType piece;
        private Func<string, Cell> getCell { get; set; }

        public Pawn(bool isWhite, Func<string, Cell> getCell) : base(isWhite)
        {
            this.piece = pieceType.Pawn;
            this.getCell = getCell;
        }
        
        public override void determineValidMoves(string coords)
        {
            if (this.isWhite)
            {
                this.determineWhiteMoves(coords);
            }
            else
            {
                this.determineBlackMoves(coords);
            }
        }

        private void determineWhiteMoves(string coords)
        {
            throw new NotImplementedException();
        }

        private void determineBlackMoves(string coords)
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return "P";
        }
    }
}