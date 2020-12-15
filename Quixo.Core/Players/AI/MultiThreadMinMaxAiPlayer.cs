using Quixo.Core.Players.AI.MinMax;
using System.Collections.Generic;
using System.Threading;

namespace Quixo.Core.Players.AI
{
    public class MultiThreadMinMaxAiPlayer : MinMaxAiPlayer
    {
        private Mutex _mutex = new Mutex();
        private bool _isWinFoundSignal = false;

        public MultiThreadMinMaxAiPlayer(int id, string name, PieceType pieceType)
            : base(id, name, pieceType)
        {
        }

        public MultiThreadMinMaxAiPlayer(int id, string name, PieceType pieceType, int depth)
            : base(id, name, pieceType, depth)
        {
        }

        protected override int MinMax(QuixoBoard board, Node node)
        {
            if (_isWinFoundSignal) return 0;

            if (node.Depth == 0)
            {
                node.Value = MultithreadedMinMax(board, node);
            }
            else
            {
                PieceType winner = board.GetWinner();

                if (node.Depth == MaxDepth || winner != PieceType.Empty)
                {
                    node.Value = node.Evaluate(board);
                }
                else
                {
                    PieceType currentMovePieceType = node.GetCurrentMovePieceType();

                    // Charger la liste de mouvements possibles pour ce joueur.
                    var moves = board.GetValidMoves(currentMovePieceType);

                    var value = 0;
                    foreach (var move in moves)
                    {
                        QuixoBoard copy = board.DeepClone();
                        copy.Play(move, currentMovePieceType);

                        var child = node.CreateChild(move, node.Depth + 1);
                        node.Children.Add(child);

                        value = node.CompareValues(value, MinMax(copy, child));
                    }

                    node.Value = value;
                }
            }

            return node.Value;
        }

        private int MultithreadedMinMax(QuixoBoard board, Node node)
        {
            List<Thread> threads = new List<Thread>();

            PieceType currentMovePieceType = node.GetCurrentMovePieceType();

            // Charger la liste de mouvements possibles pour ce joueur.
            var moves = board.GetValidMoves(currentMovePieceType);

            var value = 0;
            foreach (var move in moves)
            {
                Thread t = new Thread(_ =>
                {
                    QuixoBoard copy = board.DeepClone();
                    copy.Play(move, currentMovePieceType);

                    var child = node.CreateChild(move, node.Depth + 1);
                    node.Children.Add(child);

                    _mutex.WaitOne();
                    value = node.CompareValues(value, MinMax(copy, child));

                    // Nous signalons à toutes les branches (threads) qu'un node
                    // terminal vient d'être trouvé comme enfant du root. Puisque
                    // la valeur sélectionnée à ce niveau sera toujours le MAX, on 
                    // sait qu'il n'y aura pas de valeur supérieur à int.MAX et on
                    // peut donc terminer l'exécution des autres branches.
                    if (value == int.MaxValue)
                    {
                        _isWinFoundSignal = true;
                    }
                    _mutex.ReleaseMutex();
                });
                t.Start();
                threads.Add(t);
            }

            foreach (var t in threads)
            {
                t.Join();
            }

            return value;
        }
    }
}
