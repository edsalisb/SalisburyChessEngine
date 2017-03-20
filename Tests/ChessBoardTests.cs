using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Board.Positions;
using SalisburyChessEngine.Pieces;
using System;

namespace SalisburyChessEngineTests.Board
{
    [TestClass]
    public class ChessBoardTests
    {
        public ChessBoard CB { get; set; }

        [TestMethod]
        public void TestInitialBoard()
        {
            this.CB = new ChessBoard(new BoardPosition());

            var blackRook = this.CB[0][0].CurrentPiece;
            Assert.IsNotNull(blackRook);
            Assert.IsInstanceOfType(blackRook, typeof(Rook));
            Assert.AreEqual("a8", blackRook.CurrentCoordinates);
            Assert.AreEqual(false,blackRook.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackRook.ValidMoves);

            var blackKnight = this.CB[0][1].CurrentPiece;
            Assert.IsNotNull(blackKnight);
            Assert.IsInstanceOfType(blackKnight, typeof(Knight));
            Assert.AreEqual("b8", blackKnight.CurrentCoordinates);
            Assert.AreEqual(false, blackKnight.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("b8", "a6", ValidBoardMove.MovePath.KnightMove),
                new ValidBoardMove("b8", "c6", ValidBoardMove.MovePath.KnightMove)
            }, blackKnight.ValidMoves);

            var blackQueenBishop = this.CB[0][2].CurrentPiece;
            Assert.IsNotNull(this.CB[0][2].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][2].CurrentPiece, typeof(Bishop));
            Assert.AreEqual("c8", blackQueenBishop.CurrentCoordinates);
            Assert.AreEqual(false, blackQueenBishop.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackQueenBishop.ValidMoves);

            var blackQueen = this.CB[0][3].CurrentPiece;
            Assert.IsNotNull(blackQueen);
            Assert.IsInstanceOfType(blackQueen, typeof(Queen));
            Assert.AreEqual("d8", blackQueen.CurrentCoordinates);
            Assert.AreEqual(false, blackQueen.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackQueen.ValidMoves);

            var blackKing = this.CB[0][4].CurrentPiece;

            Assert.IsNotNull(blackKing);
            Assert.IsInstanceOfType(blackKing, typeof(King));
            Assert.AreEqual("e8", blackKing.CurrentCoordinates);
            Assert.AreEqual(false, blackKing.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackKing.ValidMoves);

            var blackKingBishop = this.CB[0][5].CurrentPiece;
            Assert.IsNotNull(blackKingBishop);
            Assert.IsInstanceOfType(blackKingBishop, typeof(Bishop));
            Assert.AreEqual("f8", blackKingBishop.CurrentCoordinates);
            Assert.AreEqual(false, blackKingBishop.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackKing.ValidMoves);

            var blackKingKnight = this.CB[0][6].CurrentPiece;
            Assert.IsNotNull(blackKingKnight);
            Assert.IsInstanceOfType(blackKingKnight, typeof(Knight));
            Assert.AreEqual("g8", blackKingKnight.CurrentCoordinates);
            Assert.AreEqual(false, blackKingKnight.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("g8", "f6", ValidBoardMove.MovePath.KnightMove),
                new ValidBoardMove("g8", "h6", ValidBoardMove.MovePath.KnightMove)
            }, blackKingKnight.ValidMoves);

            var blackKingRook = this.CB[0][7].CurrentPiece;
            Assert.IsNotNull(blackKingRook);
            Assert.IsInstanceOfType(blackKingRook, typeof(Rook));
            Assert.AreEqual("h8", blackKingRook.CurrentCoordinates);
            Assert.AreEqual(false, blackKingRook.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackKingRook.ValidMoves);

            var blackAPawn = this.CB[1][0].CurrentPiece;
            AssertInitialPawnData(blackAPawn, "a", false);

            var blackBPawn = this.CB[1][1].CurrentPiece;
            AssertInitialPawnData(blackBPawn, "b", false);

            var blackCPawn = this.CB[1][2].CurrentPiece;
            AssertInitialPawnData(blackCPawn, "c", false);

            var blackDPawn = this.CB[1][3].CurrentPiece;
            AssertInitialPawnData(blackDPawn, "d", false);

            var blackEPawn = this.CB[1][4].CurrentPiece;
            AssertInitialPawnData(blackEPawn, "e", false);

            var blackFPawn = this.CB[1][5].CurrentPiece;
            AssertInitialPawnData(blackFPawn, "f", false);

            var blackGPawn = this.CB[1][6].CurrentPiece;
            AssertInitialPawnData(blackGPawn, "g", false);

            var blackHPawn = this.CB[1][7].CurrentPiece;
            AssertInitialPawnData(blackHPawn, "h", false);

            var whiteAPawn = this.CB[6][0].CurrentPiece;
            AssertInitialPawnData(whiteAPawn, "a", true);

            var whiteBPawn = this.CB[6][1].CurrentPiece;
            AssertInitialPawnData(whiteBPawn, "b", true);

            var whiteCPawn = this.CB[6][2].CurrentPiece;
            AssertInitialPawnData(whiteCPawn, "c", true);

            var whiteDPawn = this.CB[6][3].CurrentPiece;
            AssertInitialPawnData(whiteDPawn, "d", true);

            var whiteEPawn = this.CB[6][4].CurrentPiece;
            AssertInitialPawnData(whiteEPawn, "e", true);

            var whiteFPawn = this.CB[6][5].CurrentPiece;
            AssertInitialPawnData(whiteFPawn, "f", true);

            var whiteGPawn = this.CB[6][6].CurrentPiece;
            AssertInitialPawnData(whiteGPawn, "g", true);

            var whiteHPawn = this.CB[6][7].CurrentPiece;
            AssertInitialPawnData(whiteHPawn, "h", true);

            var whiteQueenRook = this.CB[7][0].CurrentPiece;
            Assert.IsNotNull(whiteQueenRook);
            Assert.IsInstanceOfType(whiteQueenRook, typeof(Rook));
            Assert.AreEqual("a1", whiteQueenRook.CurrentCoordinates);
            Assert.AreEqual(true, whiteQueenRook.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteQueenRook.ValidMoves);

            var whiteQueenKnight = this.CB[7][1].CurrentPiece;
            Assert.IsNotNull(whiteQueenKnight);
            Assert.IsInstanceOfType(whiteQueenKnight, typeof(Knight));
            Assert.AreEqual("b1", whiteQueenKnight.CurrentCoordinates);
            Assert.AreEqual(true, whiteQueenKnight.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("b1", "a3", ValidBoardMove.MovePath.KnightMove),
                new ValidBoardMove("b1", "c3", ValidBoardMove.MovePath.KnightMove)
            }, whiteQueenKnight.ValidMoves);

            var whiteQueenBishop = this.CB[7][2].CurrentPiece;
            Assert.IsNotNull(whiteQueenBishop);
            Assert.IsInstanceOfType(whiteQueenBishop, typeof(Bishop));
            Assert.AreEqual("c1", whiteQueenBishop.CurrentCoordinates);
            Assert.AreEqual(true, whiteQueenBishop.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteQueenBishop.ValidMoves);

            var whiteQueen = this.CB[7][3].CurrentPiece;
            Assert.IsNotNull(whiteQueen);
            Assert.IsInstanceOfType(whiteQueen, typeof(Queen));
            Assert.AreEqual("d1", whiteQueen.CurrentCoordinates);
            Assert.AreEqual(true, whiteQueen.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteQueen.ValidMoves);

            var whiteKing = this.CB[7][4].CurrentPiece;
            Assert.IsNotNull(whiteKing);
            Assert.IsInstanceOfType(whiteKing, typeof(King));
            Assert.AreEqual("e1", whiteKing.CurrentCoordinates);
            Assert.AreEqual(true, whiteKing.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteKing.ValidMoves);

            var whiteKingBishop = this.CB[7][5].CurrentPiece;
            Assert.IsNotNull(whiteKingBishop);
            Assert.IsInstanceOfType(whiteKingBishop, typeof(Bishop));
            Assert.AreEqual("f1", whiteKingBishop.CurrentCoordinates);
            Assert.AreEqual(true, whiteKingBishop.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteKingBishop.ValidMoves);

            var whiteKingKnight = this.CB[7][6].CurrentPiece;
            Assert.IsNotNull(whiteKingKnight);
            Assert.IsInstanceOfType(whiteKingKnight, typeof(Knight));
            Assert.AreEqual("g1", whiteKingKnight.CurrentCoordinates);
            Assert.AreEqual(true, whiteKingKnight.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("g1", "f3", ValidBoardMove.MovePath.KnightMove),
                new ValidBoardMove("g1", "h3", ValidBoardMove.MovePath.KnightMove)
            }, whiteKingKnight.ValidMoves);

            var whiteKingRook = this.CB[7][7].CurrentPiece;
            Assert.IsNotNull(whiteKingRook);
            Assert.IsInstanceOfType(whiteKingRook, typeof(Rook));
            Assert.AreEqual("h1", whiteKingRook.CurrentCoordinates);
            Assert.AreEqual(true, whiteKingRook.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), whiteKingRook.ValidMoves);
        }
        [TestMethod]
        public void TestDoubleQueenPin()
        {
            var fenList = new List<string>()
            {
                "rnb1kbnr/pppp1ppp/4q3/8/8/4Q3/PPPP1PPP/RNB1KBNR w KQkq",
                "w",
                "KQkq",
                "-",
                "1",
                "1"
            };
            var fenPosition = new FENNotationPosition(fenList);

            this.CB = new ChessBoard(fenPosition);

            var blackQueen = this.CB[2][4].CurrentPiece;
            Assert.IsNotNull(blackQueen);
            Assert.IsInstanceOfType(blackQueen, typeof(Queen));
            Assert.AreEqual("e6", blackQueen.CurrentCoordinates);
            Assert.AreEqual(false, blackQueen.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("e6", "e7", ValidBoardMove.MovePath.Up),
                new ValidBoardMove("e6", "e5", ValidBoardMove.MovePath.Down),
                new ValidBoardMove("e6", "e4", ValidBoardMove.MovePath.Down),
                new ValidBoardMove("e6", "e3", ValidBoardMove.MovePath.Down)
            }, blackQueen.ValidMoves);

            var whiteQueen = this.CB[5][4].CurrentPiece;
            Assert.IsNotNull(whiteQueen);
            Assert.IsInstanceOfType(whiteQueen, typeof(Queen));
            Assert.AreEqual("e3", whiteQueen.CurrentCoordinates);
            Assert.AreEqual(true, whiteQueen.isWhite);
            AssertValidMoves(new List<ValidBoardMove>()
            {
                new ValidBoardMove("e3", "e2", ValidBoardMove.MovePath.Down),
                new ValidBoardMove("e3", "e4", ValidBoardMove.MovePath.Up),
                new ValidBoardMove("e3", "e5", ValidBoardMove.MovePath.Up),
                new ValidBoardMove("e3", "e6", ValidBoardMove.MovePath.Up)
            }, whiteQueen.ValidMoves);

        } 

        private void AssertInitialPawnData(PieceBase pawn, string columnLetter, bool isWhite)
        {
            Assert.IsNotNull(pawn);
            Assert.IsInstanceOfType(pawn, typeof(Pawn));
            Assert.AreEqual(isWhite, pawn.isWhite);
            if (!isWhite)
            {
                Assert.AreEqual(columnLetter + "7", pawn.CurrentCoordinates);
                AssertValidMoves(new List<ValidBoardMove>()
                {
                    new ValidBoardMove(columnLetter + "7", columnLetter + "6", ValidBoardMove.MovePath.Down),
                    new ValidBoardMove(columnLetter + "7", columnLetter + "5", ValidBoardMove.MovePath.Down)
                }, pawn.ValidMoves);
            }
            else
            {
                Assert.AreEqual(columnLetter + "2", pawn.CurrentCoordinates);
                AssertValidMoves(new List<ValidBoardMove>()
                {
                    new ValidBoardMove(columnLetter + "2", columnLetter + "3", ValidBoardMove.MovePath.Up),
                    new ValidBoardMove(columnLetter + "2", columnLetter + "4", ValidBoardMove.MovePath.Up)
                }, pawn.ValidMoves);
            }
        }

        private void AssertValidMoves(List<ValidBoardMove> expected, List<ValidBoardMove> actual)
        {
            if (expected.Count != actual.Count)
            {
                Assert.Fail();
            }

            foreach (var move in expected)
            {
                var coordsTo = move.CoordinatesTo;
                var coordsFrom = move.CoordinatesFrom;
                var movePath = move.Path;

                var actualMoves = actual.Where(x => x.Path == movePath && x.CoordinatesFrom == coordsFrom
                && x.CoordinatesTo == coordsTo).ToList();

                if (actualMoves.Count != 1)
                {
                    Assert.Fail();
                }
                
            }
        }
    }
}
