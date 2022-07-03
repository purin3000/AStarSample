using System.Collections.Generic;
using System.Linq;

public class PriorityQueue<TKey, TValue> {
  SortedDictionary<TKey, Queue<TValue>> dict = new SortedDictionary<TKey, Queue<TValue>>();
  int count = 0;

  public int Count {
    get {
      return count;
    }
  }

  public void Push(TKey key, TValue value) {
    if (!dict.ContainsKey(key)) {
      dict[key] = new Queue<TValue>();
    }

    dict[key].Enqueue(value);
    count++;
  }

  public KeyValuePair<TKey, TValue> Pop() {
    var queue = dict.First();
    if (queue.Value.Count <= 1) {
      dict.Remove(queue.Key);
    }
    count--;
    return new KeyValuePair<TKey, TValue>(queue.Key, queue.Value.Dequeue());
  }

  public TValue Peek() {
    var queue = dict.First();
    return queue.Value.Peek();
  }

  public void Remove(TKey key) {
    var queue = dict[key];
    if (queue.Count <= 1) {
      dict.Remove(key);
    }
    count--;
    queue.Dequeue();
  }
}