﻿using System;
using System.Collections.Generic;
using System.Linq;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngine
{
    public class ChessBoard
    {
        public List<List<Cell>> Board { get; set; }
        public ChessBoard()
        {
            Board = new List<List<Cell>>();
            this.initializeBoard();
            this.UpdateBoard();
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
                Board.Add(row);
            }
        }

        private void placePiecesOnBoard()
        {
            //white back rank
            this.getCell("a1").CurrentPiece = new Rook(true, this.getCell);
            this.getCell("b1").CurrentPiece = new Knight(true, this.getCell);
            this.getCell("c1").CurrentPiece = new Bishop(true, this.getCell);
            this.getCell("d1").CurrentPiece = new Queen(true, this.getCell);
            this.getCell("e1").CurrentPiece = new King(true, this.getCell);
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
            this.getCell("e8").CurrentPiece = new King(false, this.getCell);
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
            foreach (var row in Board)
            {
                foreach (var cell in row)
                {
                    cell.CurrentPiece.determineValidMoves(cell.Coordinates);
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

            return Board[BoardProperties.Rows - parsedRow][parsedColumn - 1];
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
            if (parsedRow > BoardProperties.Rows || parsedRow  <= 0)
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
                Console.Write(Board[BoardProperties.Rows - rowNumber][j - 1].ToString() + " ");
            }
        }
        public void displayFooter()
        {
            Console.WriteLine(" ");
            Console.Write("  A B C D E F G H");
        }
        internal void TryMovePiece(string algebraicCoord, out Move move)
        {
            move = null;
            if (algebraicCoord == null)
            {
                return;
            }

            var lastTwoLetters = algebraicCoord.Substring(algebraicCoord.Length - 2);
            var cell = this.getCell(lastTwoLetters);
            if (cell == null)
            {
                return;
            }

            if (cell.CurrentPiece != null)
            {
                return;
            }
            if (algebraicCoord.Length > 2)
            {
                char pieceLetter = algebraicCoord[0];
                if (pieceLetter != 'B' && pieceLetter != 'K' && 
                    pieceLetter != 'N' && pieceLetter != 'R' && pieceLetter != 'Q')
                {
                    return;
                }

               
                
            }
            return;
        }
    }

   
}