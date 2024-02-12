namespace Retryrr.Application;

public static class EnumerableExtensions
{
   public static Queue<T> ToQueue<T>(this IEnumerable<T> collection)
      where T : IComparable
   {
      var queue = new Queue<T>();
      var sortedList = collection.ToList();
      sortedList.Sort();

      foreach (var item in sortedList)
      {
         queue.Enqueue(item);
      }

      return queue;
   }
}
