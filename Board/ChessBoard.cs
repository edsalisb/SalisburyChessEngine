using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Pieces;
using SalisburyChessEngine.Moves;
using SalisburyChessEngine.Utilities;
using SalisburyChessEngine.Board.Positions;

namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard : List<List<Cell>>
    {
        public ValidBoardMove CheckingBoardMove { get; set; }

        public King WhiteKing { get; set; }
        public King BlackKing { get; set; }

        public List<ValidBoardMove> BlackPiecePressure { get; set; }
        public List<ValidBoardMove> WhitePiecePressure { get; set; }
        
        public bool IsWhitesTurn { get;  set; }

        private AlgebraicNotationParser parser;

        private ChessBoard()
        {
            this.IsWhitesTurn = true;
            this.parser = new AlgebraicNotationParser(this);
        }
        public ChessBoard(BoardPosition position) : this()
        { 
            this.InitializeBoard(position);
            this.InitializeKingComponents();
            this.UpdateBoardState();
        }

       

        public ChessBoard(FENNotationPosition fen): this()
        {
            this.InitializeBoard(fen);
            this.InitializeKingComponents();
            this.UpdateBoardState();
        }

        public void InitializeBoard(BoardPosition position)
        {
            InitializeCells();
            CreatePieceFactory(position);
        }

        

        public void InitializeBoard(FENNotationPosition fen)
        {
            this.InitializeCells();
            this.CreatePieceFactory(fen);
        }

        private void InitializeKingComponents()
        {
            this.WhiteKing.OnCheckCallbacks += UpdateBoard;
            this.BlackKing.OnCheckCallbacks += UpdateBoard;
            this.BlackPiecePressure = new List<ValidBoardMove>();
            this.WhitePiecePressure = new List<ValidBoardMove>();
        }

        private void CreatePieceFactory(BoardPosition position)
        {
            var bKing = position.Where(x => x.Value == 'k').ToList();
            var wKing = position.Where(x => x.Value == 'K').ToList();

            if (wKing.Count != 1 || bKing.Count != 1)
            {
                return;
            }

            this.WhiteKing = new King(true, this.GetCell, wKing[0].Key);
            this.BlackKing = new King(false, this.GetCell, bKing[0].Key);

            foreach (KeyValuePair<string, char> kvp in position)
            {
                if (kvp.Value == 'K')
                {
                    this.GetCell(kvp.Key).CurrentPiece = this.WhiteKing;
                }
                else if (kvp.Value == 'k')
                {
                    this.GetCell(kvp.Key).CurrentPiece = this.BlackKing;
                }
                else
                {
                    var cell = this.GetCell(kvp.Key);
                    switch (kvp.Value)
                    {
                        case 'R':
                            cell.CurrentPiece = new Rook(true, this.GetCell, kvp.Key, this.BlackKing);
                            break;
                        case 'r':
                            cell.CurrentPiece = new Rook(false, this.GetCell, kvp.Key, this.WhiteKing);
                            break;
                        case 'N':
                            cell.CurrentPiece = new Knight(true, this.GetCell, kvp.Key);
                            break;
                        case 'n':
                            cell.CurrentPiece = new Knight(false, this.GetCell, kvp.Key);
                            break;
                        case 'B':
                            cell.CurrentPiece = new Bishop(true, this.GetCell, kvp.Key, this.BlackKing);
                            break;
                        case 'b':
                            cell.CurrentPiece = new Bishop(false, this.GetCell, kvp.Key, this.WhiteKing);
                            break;
                        case 'Q':
                            cell.CurrentPiece = new Queen(true, this.GetCell, kvp.Key, this.BlackKing);
                            break;
                        case 'q':
                            cell.CurrentPiece = new Queen(false, this.GetCell, kvp.Key, this.WhiteKing);
                            break;
                        case 'P':
                            cell.CurrentPiece = new Pawn(true, this.GetCell, kvp.Key);
                            break;
                        case 'p':
                            cell.CurrentPiece = new Pawn(false, this.GetCell, kvp.Key);
                            break;
                        default:
                            break;
                    }
                }
            }


        }

        public void UpdateBoardState()
        {
            this.BlackPiecePressure = new List<ValidBoardMove>();
            this.WhitePiecePressure = new List<ValidBoardMove>();
            this.ResetPieces();
            this.UpdateBoard();
            this.DetermineTeamPressure();
            this.DetermineKingMoves();
            this.CheckIfKingChecked();
            this.DisplayBoard();
        }

        private void InitializeCells()
        {
            for (var i = BoardProperties.Rows; i >= 1; i--)
            {
                var row = new List<Cell>();
                for (var j = 1; j <= BoardProperties.Columns; j++)
                {
                    row.Add(new Cell(i, j));
                }
                Add(row);
            }
        }

        internal void ReplacePiece(Move move)
        {
            if (move.IsKingSideCastle)
            {
                if (move.IsWhitesTurn)
                {
                    var kingCellFrom = this.GetCell("e1");
                    var kingCellTo = this.GetCell("g1");
                    var rookCellFrom = this.GetCell("h1");
                    var rookCellTo = this.GetCell("f1");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                else
                {
                    var kingCellFrom = this.GetCell("e8");
                    var kingCellTo = this.GetCell("g8");
                    var rookCellFrom = this.GetCell("h8");
                    var rookCellTo = this.GetCell("f8");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                return;
            }
            else if (move.IsQueenSideCastle)
            {
                if (move.IsWhitesTurn)
                {
                    var kingCellFrom = this.GetCell("e1");
                    var kingCellTo = this.GetCell("c1");
                    var rookCellFrom = this.GetCell("a1");
                    var rookCellTo = this.GetCell("d1");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                else
                {
                    var kingCellFrom = this.GetCell("e8");
                    var kingCellTo = this.GetCell("c8");
                    var rookCellFrom = this.GetCell("a8");
                    var rookCellTo = this.GetCell("d8");

                    this.ReplacePieceBase(kingCellFrom, kingCellTo);
                    this.ReplacePieceBase(rookCellFrom, rookCellTo);
                }
                return;
            }
           else
            {
                this.ReplacePieceBase(move.CellFrom, move.CellTo);
            }
        }

        private void ReplacePieceBase(Cell cellFrom, Cell cellTo)
        {
            var currentPiece = cellFrom.CurrentPiece;
            if (currentPiece.TypeOfPiece == PieceBase.PieceType.Rook)
            {
                var rook = (Rook)currentPiece;
                rook.HasMoved = true;
            }
            else if (currentPiece.TypeOfPiece == PieceBase.PieceType.King)
            {
                var king = (King)currentPiece;
                king.HasMoved = true;
            }
            cellTo.CurrentPiece = currentPiece;
            cellTo.CurrentPiece.CurrentCoordinates = cellTo.Coordinates;
            cellFrom.CurrentPiece = null;
        }

        internal void Rollback(Move move)
        {
            var currentPiece = move.CellTo.CurrentPiece;
            move.CellFrom.CurrentPiece = currentPiece;
            move.CellFrom.CurrentPiece.CurrentCoordinates = move.CellFrom.Coordinates;
            move.CellTo.CurrentPiece = null;
        }

        public void ResetPieces()
        {
            ExecuteCellLevelFunction(ResetPiece);
        }

        public void ResetPiece(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                var piece = Cell.GetPiece(cell);
                piece.ValidMovesSet = false;
                piece.PinnedMoves = new List<ValidBoardMove>();
            }
        }
        internal void CheckIfKingChecked()
        {
            int whitePiecePressureIndex = this.WhitePiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.BlackKing.CurrentCoordinates);
            int blackPiecePressureIndex = this.BlackPiecePressure.Select(GeneralUtilities.SelectCoordinates).ToList().IndexOf(this.WhiteKing.CurrentCoordinates);
            if (whitePiecePressureIndex > -1)
            {
                //black king checked
                this.CheckingBoardMove = this.WhitePiecePressure[whitePiecePressureIndex];
                this.BlackKing.IsChecked = true;
            }

            else if (blackPiecePressureIndex > -1)
            {
                //white king checked
                this.CheckingBoardMove = this.BlackPiecePressure[blackPiecePressureIndex];
                this.WhiteKing.IsChecked = true;
            }
            else
            {
                this.BlackKing.IsChecked = false;
                this.WhiteKing.IsChecked = false;
            }
        }
        public void UpdateBoard()
        {
            ExecuteCellLevelFunction(DetermineValidMovesIfNotKing);
            if (this.CheckingBoardMove != null)
            {
                this.CheckingBoardMove = null;
            }
            
        }

        public void DetermineValidMovesIfNotKing(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                if (cell.CurrentPiece.GetType() == typeof(King))
                {
                    return;
                }
                if (!cell.CurrentPiece.ValidMovesSet || (this.WhiteKing.IsChecked || this.BlackKing.IsChecked))
                {
                    cell.CurrentPiece.CurrentCoordinates = cell.Coordinates;
                    cell.CurrentPiece.DetermineValidMoves(cell.Coordinates, this.CheckingBoardMove);
                }
            }
        }

        


        private void DetermineTeamPressure()
        {
            ExecuteCellLevelFunction(DetermineCellPressure);
        }

        private void DetermineCellPressure(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                List<ValidBoardMove> piecePressureList;
                if (cell.CurrentPiece.isWhite)
                {
                    piecePressureList = WhitePiecePressure;
                }
                else
                {
                    piecePressureList = BlackPiecePressure;
                }

                List<ValidBoardMove> validMovesList;
                validMovesList = cell.CurrentPiece.PiecePressure;

                foreach (var move in validMovesList)
                {
                    if (piecePressureList.IndexOf(move) == -1)
                    {
                        piecePressureList.Add(move);
                    }
                }

            }
        }
        private void DetermineKingMoves()
        {
            ExecuteCellLevelFunction(DetermineValidMoveForKing);
        }

        public void DetermineValidMoveForKing(Cell cell)
        {
            if (Cell.HasPiece(cell))
            {
                if (cell.CurrentPiece.GetType() != typeof(King))
                {
                    return;
                }
                var king = (King)cell.CurrentPiece;
                if (king.isWhite)
                {
                    this.WhitePiecePressure = king.DetermineValidMoves(cell.Coordinates, BlackPiecePressure, WhitePiecePressure, this.CheckingBoardMove);
                }
                else
                {
                    this.BlackPiecePressure = king.DetermineValidMoves(cell.Coordinates, WhitePiecePressure, BlackPiecePressure, this.CheckingBoardMove);
                }
            }

            
        }

        //Utility method to extract a cell from the list based on coordinates
        public Cell GetCell(string coords)
        {
            //expected: 2 letters, a1-h8
            if (!DetermineIfCoordinatesMeetLengthRequirement(coords))
            {
                return null;
            }
            char columnLetter = coords[0];
            char row = coords[1];
            int parsedColumn = DetermineIfColumnLetterValid(columnLetter);
            int parsedRow = DetermineIfRowNumberValid(row);
            if (parsedColumn == -1 || parsedRow == -1)
            {
                return null;
            }

            return this[BoardProperties.Rows - parsedRow][parsedColumn - 1];
        }

        private bool DetermineIfCoordinatesMeetLengthRequirement(string coords)
        {
            if (coords.Length != 2)
            {
                return false;
            }
            return true;
        }

        private int DetermineIfRowNumberValid(char row)
        {
            if (!int.TryParse(row.ToString(), out int parsedRow))
            {
                return -1;
            }
            if (parsedRow > BoardProperties.Rows || parsedRow <= 0)
            {
                return -1;
            }
            return parsedRow;
        }

        private int DetermineIfColumnLetterValid(char columnLetter)
        {
            if (columnLetter < 'a' || columnLetter > 'h')
            {
                return -1;
            }
            if (!BoardProperties.ColumnLettersMappedToNumbers.TryGetValue(columnLetter, out int parsedColumn))
            {
                return -1;
            }
            return parsedColumn;
        }
        public void DisplayBoard()
        {
            for (var i = BoardProperties.Rows; i > 0; i--)
            {
                DisplayRows(i);
            }
        }
        public void DisplayRows(int rowNumber)
        {
            Console.Write(rowNumber + " ");
            DisplayCellsInRow(rowNumber);
            Console.WriteLine(" ");
        }
        public void DisplayCellsInRow(int rowNumber)
        {
            for (var j = 1; j <= BoardProperties.Columns; j++)
            {
                var cell = this[BoardProperties.Rows - rowNumber][j - 1];
                if (Cell.HasPiece(cell))
                {
                    var piece = Cell.GetPiece(cell);
                    if (piece.isWhite)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                }
                Console.Write(this[BoardProperties.Rows - rowNumber][j - 1].ToString() + " ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        public void DisplayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal bool TryMovePiece(string algebraicCoord, out Move move)
        {
            move = parser.Parse(algebraicCoord, this.IsWhitesTurn);

            if (move.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }


    
   
}