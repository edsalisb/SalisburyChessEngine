namespace SalisburyChessEngine.Moves
{
    public class Move
    {
        public bool IsCapturable { get; set; }

        public bool IsKingSideCastle { get; set; }

        public bool IsQueenSideCastle { get; set; }
        public Cell CellFrom { get; set; }

        public Cell CellTo { get; set; }
        public bool MustBeChecked { get; set; }

        public bool IsWhitesTurn { get; set; }

        public bool IsValid { get; set; }

        public string AlgebraicCoord { get; set; }

        public Move(string algebraicCoord, bool isWhitesTurn)
        {
            this.IsWhitesTurn = isWhitesTurn;
            this.AlgebraicCoord = algebraicCoord;
            if (algebraicCoord == null)
            {
                this.IsValid = false;
            }
            if (algebraicCoord.Length < 2)
            {
                this.IsValid = false;
            }
        }

    }
}