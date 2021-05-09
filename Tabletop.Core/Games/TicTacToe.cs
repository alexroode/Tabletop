using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tabletop.Core.Games
{
    public class Result
    {
        public bool Succeeded { get; set; }
        public string ErrorMessage { get; set; }

        public static Result Failed(string errorMessage)
        {
            return new Result()
            {
                Succeeded = false,
                ErrorMessage = errorMessage
            };
        }

        public static readonly Result Success = new Result() { Succeeded = true };
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }
        public static new Result<T> Failed(string errorMessage)
        {
            return new Result<T>()
            {
                ErrorMessage = errorMessage
            };
        }

        public static new Result<T> Success(T value)
        {
            return new Result<T>()
            {
                Succeeded = true,
                Value = value
            };
        }
    }

    public interface IAction { }

    public interface IGameState { }

    public record TicTacToeMove(int PlayerId, int X, int Y) : IAction;

    public record TicTacToeState(int CurrentTurn, int[,] Board, bool IsTie, int? Winner);

    public class TicTacToeGame
    {
        private readonly int _size = 3;

        public TicTacToeGame(int size)
        {
            _size = size;
        }

        public Result<TicTacToeState> DoAction(TicTacToeState state, IAction action)
        {
            var validateResult = ValidateAction(state, action);
            if (!validateResult.Succeeded)
            {
                return Result<TicTacToeState>.Failed(validateResult.ErrorMessage);
            }

            var newState = ReduceState(state, action);
            return Result<TicTacToeState>.Success(newState);
        }

        private static Result ValidateAction(TicTacToeState state, IAction action)
        {
            if (action is TicTacToeMove move)
            {
                if (state.IsTie || state.Winner != null)
                {
                    return Result.Failed("The game is over");
                }

                if (move.PlayerId != state.CurrentTurn)
                {
                    return Result.Failed("It's not your turn");
                }

                if (state.Board[move.X, move.Y] != 0)
                {
                    return Result.Failed("Invalid move");
                }

                return Result.Success;
            }

            throw new Exception("Invalid action");
        }

        private TicTacToeState ReduceState(TicTacToeState state, IAction action)
        {
            if (action is TicTacToeMove move)
            {
                var updatedBoard = (int[,])state.Board.Clone();
                updatedBoard[move.X, move.Y] = move.PlayerId;

                var newState = state with { Board = updatedBoard, CurrentTurn = state.CurrentTurn == 1 ? 2 : 1 };
                return CheckForEndgame(newState);
            }

            throw new Exception("Invalid action");
        }

        private TicTacToeState CheckForEndgame(TicTacToeState state)
        {
            var rows = Enumerable.Range(0, _size)
                .Select(i => Enumerable.Range(0, _size).Select(j => state.Board[i, j]).ToList())
                .ToList();

            var rowVictory = rows.FirstOrDefault(r =>
            {
                var firstValue = r[0];

                return firstValue != 0 && r.All(v => v == firstValue);
            });

            if (rowVictory != null)
            {
                return state with { Winner = rowVictory[0] };
            }

            var columns = Enumerable.Range(0, _size)
                .Select(i => Enumerable.Range(0, _size).Select(j => state.Board[j, i]).ToList())
                .ToList();

            var columnVictory = columns.FirstOrDefault(r =>
            {
                var firstValue = r[0];

                return firstValue != 0 && r.All(v => v == firstValue);
            });

            if (columnVictory != null)
            {
                return state with { Winner = columnVictory[0] };
            }

            var diagonalVictory = Enumerable.Range(0, _size).Select(i => state.Board[i, i]).All(v => v != 0 && v == state.Board[0, 0]);
            if (diagonalVictory)
            {
                return state with { Winner = state.Board[0, 0] };
            }

            var otherDiagonalVictory = Enumerable.Range(0, _size).Select(i => state.Board[i, _size - i - 1]).All(v => v != 0 && v == state.Board[0, 0]);
            if (otherDiagonalVictory)
            {
                return state with { Winner = state.Board[0, _size - 1] };
            }

            if (state.Board.Cast<int>().All(v => v != 0))
            {
                return state with { IsTie = true };
            }

            return state;
        }
    }
}
