using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Multithreading
{
    public class Singlethreading
    {
        public Dictionary<char, long> Calculate(ConcurrentQueue<char> queue)
        {
            var dictionary = new Dictionary<char, long>();
            while (queue.Count != 0)
            {
                char c;
                if (!queue.TryDequeue(out c))
                {
                    continue;
                }
                if (dictionary.ContainsKey(c))
                {
                    dictionary[c]++;
                }
                else
                {
                    dictionary[c] = 1;
                }
            }

            return dictionary;
        }
    }
}