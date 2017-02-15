using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Multithreading
{
    public class Multithreading
    {
        private ConcurrentDictionary<int, Dictionary<char, long>> _dictionaries;
        private Dictionary<char, long> _merged;

        private void Calculate(object i, ConcurrentQueue<char> queue)
        {
            var calculate = Task.Run(() =>
            {
                try
                {
                    int index = (int) i;
                    _dictionaries[index] = new Dictionary<char, long>();
                    while (queue.Count != 0)
                    {
                        char c;
                        if (!queue.TryDequeue(out c))
                        {
                            continue;
                        }
                        if (_dictionaries[index].ContainsKey(c))
                        {
                            _dictionaries[index][c]++;
                        }
                        else
                        {
                            _dictionaries[index][c] = 1;
                        }
                    }
                }
                catch
                {
                }
            });
            Task.WaitAll(calculate);
        }

        public async Task<Dictionary<char, long>> Calculate(ConcurrentQueue<char> queue)
        {

            _dictionaries = new ConcurrentDictionary<int, Dictionary<char, long>>();


            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                var i1 = i;
                var calculate = Task.Run(() => Calculate(i1, queue));
            }

            await Task.Run(() =>
            {
                _merged = new Dictionary<char, long>();
                try
                {
                    foreach (var dictionary in _dictionaries)
                    {
                        foreach (var i in dictionary.Value)
                        {
                            if (_merged.ContainsKey(i.Key))
                            {
                                _merged[i.Key] += i.Value;
                            }
                            else
                            {
                                _merged[i.Key] = i.Value;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            });
            Task.WaitAll();
            return _merged;
        }
    }
}
