using Quixo.Core.Players;
using Quixo.Core.Players.AI;
using System;
using Xunit;

namespace Quixo.Core.Tests
{
    public class SingleThreadAiTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        private void TestAiWin_1Move(int depth)
        {
            var board = new QuixoBoard();
            board.Pieces[0].PieceType = PieceType.Crossmark;
            board.Pieces[15].PieceType = PieceType.Crossmark;
            board.Pieces[10].PieceType = PieceType.Crossmark;
            board.Pieces[20].PieceType = PieceType.Crossmark;
            board.Pieces[2].PieceType = PieceType.Circle;
            board.Pieces[3].PieceType = PieceType.Circle;
            board.Pieces[4].PieceType = PieceType.Circle;
            board.Pieces[9].PieceType = PieceType.Circle;
            board.Pieces[14].PieceType = PieceType.Circle;

            var playerPieceType = PieceType.Crossmark;
            var opponentPieceType = PieceType.Circle;
            var player = new SingleThreadMinMaxAiPlayer(2, "AI", playerPieceType, depth);

            player.PlayTurn(board);

            Assert.True(board.CheckPieceWin(playerPieceType));
            Assert.False(board.CheckPieceWin(opponentPieceType));
        }

        [Fact]
        private void TestAiWinLose_PreventMakingOpponentWin_EdgeCase()
        {
            var board = new QuixoBoard();
            board.Pieces[21].PieceType = PieceType.Crossmark;
            board.Pieces[22].PieceType = PieceType.Crossmark;
            board.Pieces[23].PieceType = PieceType.Crossmark;
            board.Pieces[24].PieceType = PieceType.Crossmark;
            board.Pieces[20].PieceType = PieceType.Circle;
            board.Pieces[16].PieceType = PieceType.Circle;
            board.Pieces[17].PieceType = PieceType.Circle;
            board.Pieces[18].PieceType = PieceType.Circle;
            board.Pieces[19].PieceType = PieceType.Circle;

            //var playerPieceType = PieceType.Crossmark;
            var opponentPieceType = PieceType.Circle;
            var player = new SingleThreadMinMaxAiPlayer(2, "AI", PieceType.Crossmark);

            player.PlayTurn(board);

            Assert.False(board.CheckPieceWin(opponentPieceType));
        }

        [Fact]
        private void TestAiBlock_SimpleExample()
        {
            var board = new QuixoBoard();
            board.Pieces[10].PieceType = PieceType.Crossmark;
            board.Pieces[15].PieceType = PieceType.Crossmark;
            board.Pieces[20].PieceType = PieceType.Crossmark;
            board.Pieces[4].PieceType = PieceType.Circle;
            board.Pieces[9].PieceType = PieceType.Circle;
            board.Pieces[14].PieceType = PieceType.Circle;
            board.Pieces[19].PieceType = PieceType.Circle;

            //var playerPieceType = PieceType.Crossmark;
            var opponentPieceType = PieceType.Circle;
            var player = new SingleThreadMinMaxAiPlayer(2, "AI", PieceType.Crossmark);

            player.PlayTurn(board);

            Assert.False(board.CheckPieceWin(opponentPieceType));
        }

        [Fact]
        private void TestAiBlock_LastColumn()
        {
            var board = new QuixoBoard();
            board.Pieces[0].PieceType = PieceType.Crossmark;
            board.Pieces[5].PieceType = PieceType.Crossmark;
            board.Pieces[10].PieceType = PieceType.Crossmark;
            board.Pieces[1].PieceType = PieceType.Crossmark;
            board.Pieces[20].PieceType = PieceType.Circle;
            board.Pieces[4].PieceType = PieceType.Circle;
            board.Pieces[9].PieceType = PieceType.Circle;
            board.Pieces[14].PieceType = PieceType.Circle;
            board.Pieces[24].PieceType = PieceType.Circle;

            //var playerPieceType = PieceType.Crossmark;
            var opponentPieceType = PieceType.Circle;
            var player = new SingleThreadMinMaxAiPlayer(2, "AI", PieceType.Crossmark);

            player.PlayTurn(board);

            Assert.False(board.CheckPieceWin(opponentPieceType));
        }
    }
}
