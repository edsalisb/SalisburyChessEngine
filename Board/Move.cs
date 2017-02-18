namespace SalisburyChessEngine.Board
{
    public  class Move
    {
        public bool IsCapturable { get; set; }
        public Cell CellFrom { get; set; }

        public Cell CellTo { get; set; }

        public string algebraicCoords { get; set; }

        public bool isWhitesTurn { get; set; }

        public bool IsValid { get; set; }
        
        public Move(bool isWhitesTurn)
        {
            this.isWhitesTurn = isWhitesTurn;
        }
        
    }
}