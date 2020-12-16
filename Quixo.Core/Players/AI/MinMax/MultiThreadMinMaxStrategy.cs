using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Quixo.Core.Players.AI.MinMax
{
    public class MultiThreadMinMaxStrategy : MinMaxStrategy
    {
        private readonly Mutex _mutex = new Mutex();
        private readonly SingleThreadMinMaxStrategy _internalMinMax;

        private bool _isWinFoundSignal = false;

        public MultiThreadMinMaxStrategy(int depth)
            : base(depth)
        {
            _internalMinMax = new SingleThreadMinMaxStrategy(depth);
        }

        public override int MinMax(QuixoBoard board, Node node)
        {
            if (_isWinFoundSignal) return 0;

            if (node.Depth == 0)
            {
                node.Value = MultithreadedMinMax(board, node);
            }
            else
            {
                _internalMinMax.MinMax(board, node);
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
