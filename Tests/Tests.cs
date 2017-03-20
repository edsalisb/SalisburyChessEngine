using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalisburyChessEngine.Board;
using SalisburyChessEngine.Pieces;

namespace SalisburyChessEngineTests.Board
{
    [TestClass]
    public class ChessBoardTests
    {
        public ChessBoard CB { get; set; }
        [TestMethod]
        public void TestInitialBoard()
        {
            this.CB = new ChessBoard(new SalisburyChessEngine.Board.Positions.BoardPosition());

            var blackRook = this.CB[0][0].CurrentPiece;
            Assert.IsNotNull(blackRook);
            Assert.IsInstanceOfType(blackRook, typeof(Rook));
            Assert.AreEqual("a8", blackRook.CurrentCoordinates);
            Assert.AreEqual(false,blackRook.isWhite);
            AssertValidMoves(new List<ValidBoardMove>(), blackRook.ValidMoves);
            Assert.AreEqual(new List<ValidBoardMove>(), blackRook.ValidMoves);

            var blackKnight = this.CB[0][1].CurrentPiece;
            Assert.IsNotNull(blackKnight);
            Assert.IsInstanceOfType(blackKnight, typeof(Knight));
            Assert.AreEqual("b8", blackKnight.CurrentCoordinates);
            Assert.AreEqual(false, blackKnight.isWhite);
            Assert.AreEqual(new List<ValidBoardMove>()
            {
                new ValidBoardMove("b8", "a6", ValidBoardMove.MovePath.KnightMove),
                new ValidBoardMove("b8", "c6", ValidBoardMove.MovePath.KnightMove)
            }, blackKnight.ValidMoves);

            Assert.IsNotNull(this.CB[0][2].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][2].CurrentPiece, typeof(Bishop));

            Assert.IsNotNull(this.CB[0][3].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][3].CurrentPiece, typeof(Queen));

            Assert.IsNotNull(this.CB[0][4].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][4].CurrentPiece, typeof(King));

            Assert.IsNotNull(this.CB[0][5].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][5].CurrentPiece, typeof(Bishop));

            Assert.IsNotNull(this.CB[0][6].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][6].CurrentPiece, typeof(Knight));

            Assert.IsNotNull(this.CB[0][7].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[0][7].CurrentPiece, typeof(Rook));

            Assert.IsNotNull(this.CB[1][0].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][0].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][1].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][1].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][2].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][2].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][3].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][3].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][4].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][4].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][5].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][5].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][6].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][6].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[1][7].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[1][7].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][0].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][0].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][1].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][1].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][2].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][2].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][3].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][3].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][4].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][4].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][5].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][5].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][6].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][6].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[6][7].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[6][7].CurrentPiece, typeof(Pawn));

            Assert.IsNotNull(this.CB[7][0].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][0].CurrentPiece, typeof(Rook));

            Assert.IsNotNull(this.CB[7][1].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][1].CurrentPiece, typeof(Knight));

            Assert.IsNotNull(this.CB[7][2].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][2].CurrentPiece, typeof(Bishop));

            Assert.IsNotNull(this.CB[7][3].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][3].CurrentPiece, typeof(Queen));

            Assert.IsNotNull(this.CB[7][4].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][4].CurrentPiece, typeof(King));

            Assert.IsNotNull(this.CB[7][5].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][5].CurrentPiece, typeof(Bishop));

            Assert.IsNotNull(this.CB[7][6].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][6].CurrentPiece, typeof(Knight));

            Assert.IsNotNull(this.CB[7][7].CurrentPiece);
            Assert.IsInstanceOfType(this.CB[7][7].CurrentPiece, typeof(Rook));
        }

        private void AssertValidMoves(List<ValidBoardMove> expected, List<ValidBoardMove> actual)
        {
            if (expected.Count != actual.Count)
            {
                Assert.Fail();
            }

            foreach (var move in expected)
            {
                
            }
        }
    }
}
