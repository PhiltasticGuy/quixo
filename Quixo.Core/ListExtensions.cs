using System;
using System.Collections.Generic;
using System.Text;

namespace Quixo.Core
{
    public static class ListExtensions
    {
        // Code utilitaire emprunté de cette réponse sur StackOverflow:
        // https://stackoverflow.com/questions/273313/randomize-a-listt#answer-1262619
        public static List<T> Shuffle<T>(this List<T> list)
        {
            Random rng = new Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
