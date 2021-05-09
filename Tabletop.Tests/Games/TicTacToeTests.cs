using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tabletop.Core.Games;
using System.Linq;

namespace Tabletop.Tests
{
    [TestClass]
    public class TicTacToeTests
    {
        [TestMethod]
        public void TestNormalGames()
        {
            var game = new TicTacToeGame(3);

            TestGame(game, 1, new List<IAction>()
            {
                new TicTacToeMove(1, 0, 0),
                new TicTacToeMove(2, 1, 0),
                new TicTacToeMove(1, 0, 1),
                new TicTacToeMove(2, 1, 1),
                new TicTacToeMove(1, 0, 2)
            });

            TestGame(game, 1, new List<IAction>()
            {
                new TicTacToeMove(1, 0, 0),
                new TicTacToeMove(2, 1, 0),
                new TicTacToeMove(1, 1, 1),
                new TicTacToeMove(2, 2, 0),
                new TicTacToeMove(1, 2, 2)
            });
        }

        [TestMethod]
        public void TestOutOfTurn()
        {
            var game = new TicTacToeGame(3);
            var state = new TicTacToeState(1, new int[3, 3], false, null);
            var result1 = game.DoAction(state, new TicTacToeMove(1, 0, 0));
            var result2 = game.DoAction(result1.Value, new TicTacToeMove(1, 1, 1));

            Assert.IsFalse(result2.Succeeded);
        }

        [TestMethod]
        public void TestInvalidMove()
        {
            var game = new TicTacToeGame(3);
            var state = new TicTacToeState(1, new int[3, 3], false, null);
            var result1 = game.DoAction(state, new TicTacToeMove(1, 0, 0));
            var result2 = game.DoAction(result1.Value, new TicTacToeMove(2, 1, 1));
            var result3 = game.DoAction(result2.Value, new TicTacToeMove(1, 0, 0));

            Assert.IsFalse(result3.Succeeded);
        }


        [TestMethod]
        public void TestTie()
        {
            var game = new TicTacToeGame(3);
            var finalState = RunGame(game, new List<IAction>()
            {
                new TicTacToeMove(1, 0, 0),
                new TicTacToeMove(2, 1, 0),
                new TicTacToeMove(1, 2, 0),
                new TicTacToeMove(2, 0, 1),
                new TicTacToeMove(1, 1, 1),
                new TicTacToeMove(2, 0, 2),
                new TicTacToeMove(1, 2, 1),
                new TicTacToeMove(2, 2, 2),
                new TicTacToeMove(1, 1, 2)
            });

            Assert.IsTrue(finalState.IsTie);
            Assert.IsNull(finalState.Winner);
        }

        private TicTacToeState RunGame(TicTacToeGame game, List<IAction> actions)
        {
            var state = new TicTacToeState(1, new int[3, 3], false, null);
            return actions.Aggregate(state, (acc, x) =>
            {
                var result = game.DoAction(acc, x);
                Assert.IsTrue(result.Succeeded);
                Assert.IsNotNull(result.Value);

                return result.Value;
            });
        }

        private void TestGame(TicTacToeGame game, int expectedWinner, List<IAction> actions)
        {
            var finalState = RunGame(game, actions);
            Assert.IsFalse(finalState.IsTie);
            Assert.AreEqual(expectedWinner, finalState.Winner);
        }
    }
}
