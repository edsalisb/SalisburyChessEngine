using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Pieces;
using SalisburyChessEngine.Moves;
using SalisburyChessEngine.Utilities;
namespace SalisburyChessEngine.Board
{
    public partial class ChessBoard : List<List<Cell>>
    {
        public King WhiteKing { get; set; }
        public King BlackKing { get; set; }

        public List<PotentialMoves> blackPiecePressure { get; set; }
        public List<PotentialMoves> whitePiecePressure { get; set; }
        public bool isWhitesTurn { get;  set; }

        private AlgebraicNotationParser parser;
        public ChessBoard()
        {
            this.isWhitesTurn = true;
            this.parser = new AlgebraicNotationParser(this);

            this.WhiteKing = new King(true, this.getCell);
            this.BlackKing = new King(false, this.getCell);
            this.blackPiecePressure = new List<PotentialMoves>();
            this.whitePiecePressure = new List<PotentialMoves>();


            this.initializeBoard();
            this.UpdateBoardState();
        }
        public void UpdateBoardState()
        {
            this.UpdateBoard();
            this.determineTeamPressure();
            this.determineKingMoves();
            this.displayBoard();
        }

        public void initializeBoard()
        {
            initializeCells();
            placePiecesOnBoard();
        }
        private void initializeCells()
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

        internal void replacePiece(Cell cellFrom, Cell cellTo)
        {
            var currentPiece = cellFrom.CurrentPiece;
            cellTo.CurrentPiece = currentPiece;
            cellFrom.CurrentPiece = null;
        }

        private void placePiecesOnBoard()
        {
            //white back rank
            this.getCell("a1").CurrentPiece = new Rook(true, this.getCell);
            this.getCell("b1").CurrentPiece = new Knight(true, this.getCell);
            this.getCell("c1").CurrentPiece = new Bishop(true, this.getCell);
            this.getCell("d1").CurrentPiece = new Queen(true, this.getCell);
            this.getCell("e1").CurrentPiece = this.WhiteKing;
            this.getCell("f1").CurrentPiece = new Bishop(true, this.getCell);
            this.getCell("g1").CurrentPiece = new Knight(true, this.getCell);
            this.getCell("h1").CurrentPiece = new Rook(true, this.getCell);
            //white pawn rank
            this.getCell("a2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("b2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("c2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("d2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("e2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("f2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("g2").CurrentPiece = new Pawn(true, this.getCell);
            this.getCell("h2").CurrentPiece = new Pawn(true, this.getCell);

            //black back rank
            this.getCell("a8").CurrentPiece = new Rook(false, this.getCell);
            this.getCell("b8").CurrentPiece = new Knight(false, this.getCell);
            this.getCell("c8").CurrentPiece = new Bishop(false, this.getCell);
            this.getCell("d8").CurrentPiece = new Queen(false, this.getCell);
            this.getCell("e8").CurrentPiece = this.BlackKing;
            this.getCell("f8").CurrentPiece = new Bishop(false, this.getCell);
            this.getCell("g8").CurrentPiece = new Knight(false, this.getCell);
            this.getCell("h8").CurrentPiece = new Rook(false, this.getCell);

            this.getCell("a7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("b7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("c7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("d7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("e7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("f7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("g7").CurrentPiece = new Pawn(false, this.getCell);
            this.getCell("h7").CurrentPiece = new Pawn(false, this.getCell);
        }

        public void UpdateBoard()
        {
            executeCellLevelFunction(DetermineValidMovesIfNotKing);
            if (this.whitePiecePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(this.BlackKing.CurrentCoordinates) > -1)
            {
                //black king checked
                this.WhiteKing.IsChecked = true;
                executeCellLevelFunction(DetermineValidMovesIfNotKing);
            }

            else if (this.blackPiecePressure.Select(ListUtilities.SelectCoordinates).ToList().IndexOf(this.WhiteKing.CurrentCoordinates) > -1)
            {
                //white king checked
                this.BlackKing.IsChecked = true;
                executeCellLevelFunction(DetermineValidMovesIfNotKing);
            }
            
        }
        public void DetermineValidMovesIfNotKing(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                if (cell.CurrentPiece.GetType() == typeof(King))
                {
                    return;
                }
                cell.CurrentPiece.CurrentCoordinates = cell.Coordinates;
                bool isChecked = false;

                if (cell.CurrentPiece.isWhite && this.WhiteKing.IsChecked)
                {
                    isChecked = true;
                }
                else if (!cell.CurrentPiece.isWhite && this.BlackKing.IsChecked)
                {
                    isChecked = true;
                }
                
                cell.CurrentPiece.determineValidMoves(cell.Coordinates, isChecked);
            }
        }

      
        private void determineTeamPressure()
        {
            executeCellLevelFunction(determineCellPressure);
        }

        private void determineCellPressure(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                List<PotentialMoves> piecePressureList;
                if (cell.CurrentPiece.isWhite)
                {
                    piecePressureList = whitePiecePressure;
                }
                else
                {
                    piecePressureList = blackPiecePressure;
                }

                List<PotentialMoves> validMovesList;
                if (cell.CurrentPiece.GetType() == typeof(Pawn))
                {
                    var pawn = (Pawn)cell.CurrentPiece;
                    validMovesList = pawn.piecePressureCoords;
                }
                else
                {
                    validMovesList = cell.CurrentPiece.ValidMoves;
                }

                foreach (var move in validMovesList)
                {
                    if (piecePressureList.IndexOf(move) == -1)
                    {
                        piecePressureList.Add(move);
                    }
                }

            }
        }
        private void determineKingMoves()
        {
            this.whitePiecePressure = new List<PotentialMoves>();
            this.blackPiecePressure = new List<PotentialMoves>();

            executeCellLevelFunction(DetermineValidMoveForKing);
        }

        public void DetermineValidMoveForKing(Cell cell)
        {
            if (cell.CurrentPiece != null)
            {
                if (cell.CurrentPiece.GetType() != typeof(King))
                {
                    return;
                }
                var king = (King)cell.CurrentPiece;
                if (king.isWhite)
                {
                    this.whitePiecePressure = king.determineValidMoves(cell.Coordinates, blackPiecePressure, whitePiecePressure);
                }
                else
                {
                    this.blackPiecePressure = king.determineValidMoves(cell.Coordinates, whitePiecePressure, blackPiecePressure);
                }
            }
        }

        //Utility method to extract a cell from the list based on coordinates
        public Cell getCell(string coords)
        {
            //expected: 2 letters, a1-h8
            if (!determineIfCoordinatesMeetLengthRequirement(coords))
            {
                return null;
            }
            char columnLetter = coords[0];
            char row = coords[1];
            int parsedColumn = determineIfColumnLetterValid(columnLetter);
            int parsedRow = determineIfRowNumberValid(row);
            if (parsedColumn == -1 || parsedRow == -1)
            {
                return null;
            }

            return this[BoardProperties.Rows - parsedRow][parsedColumn - 1];
        }

        private bool determineIfCoordinatesMeetLengthRequirement(string coords)
        {
            if (coords.Length != 2)
            {
                return false;
            }
            return true;
        }

        private int determineIfRowNumberValid(char row)
        {
            int parsedRow;
            if (!int.TryParse(row.ToString(), out parsedRow))
            {
                return -1;
            }
            if (parsedRow > BoardProperties.Rows || parsedRow <= 0)
            {
                return -1;
            }
            return parsedRow;
        }

        private int determineIfColumnLetterValid(char columnLetter)
        {
            if (columnLetter < 'a' || columnLetter > 'h')
            {
                return -1;
            }
            int parsedColumn;
            if (!CellProperties.ColumnLettersMappedToNumbers.TryGetValue(columnLetter, out parsedColumn))
            {
                return -1;
            }
            return parsedColumn;
        }
        public void displayBoard()
        {
            for (var i = BoardProperties.Rows; i > 0; i--)
            {
                displayRows(i);
            }
        }
        public void displayRows(int rowNumber)
        {
            Console.Write(rowNumber + " ");
            displayCellsInRow(rowNumber);
            Console.WriteLine(" ");
        }
        public void displayCellsInRow(int rowNumber)
        {
            for (var j = 1; j <= BoardProperties.Columns; j++)
            {
                Console.Write(this[BoardProperties.Rows - rowNumber][j - 1].ToString() + " ");
            }
        }
        public void displayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal bool TryMovePiece(string algebraicCoord, out Move move)
        {
            move = parser.Parse(algebraicCoord, this.isWhitesTurn);

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