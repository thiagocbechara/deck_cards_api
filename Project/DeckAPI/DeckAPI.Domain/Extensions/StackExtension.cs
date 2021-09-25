using System.Collections.Generic;

namespace DeckAPI.Domain.Extensions
{
    public static class StackExtension
    {
        public static IEnumerable<T> PopRange<T>(this Stack<T> stack, int amount)
        {
            var result = new List<T>(amount);
            while (amount-- > 0 && stack.Count > 0)
            {
                result.Add(stack.Pop());
            }
            return result;
        }
    }
}
