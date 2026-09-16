using System;
using System.Collections.Generic;

namespace AdvancedCSharpGenerics
{
    // Q6
    public interface IRepository<T>
    {
        void Add(T entity);
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Delete(T entity);
    }

    // Q15
    public interface ICovariant<out T>
    {
        T GetItem();
    }

    // Q16
    public interface IContravariant<in T>
    {
        void SetItem(T item);
    }

    public class Animal 
    { 
        public string Name { get; set; } = "Animal"; 
    }

    public class Dog : Animal 
    { 
        public Dog() { Name = "Dog"; } 
    }

    // Q2
    public class Container<T>
    {
        private T _item = default!;

        public void Add(T item) => _item = item;
        public T Get() => _item;
    }

    // Q3
    public class Pair<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Pair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    public static class Utilities
    {
        // Q4
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        // Q5
        public static T FindMax<T>(T[] array) where T : IComparable<T>
        {
            if (array == null || array.Length == 0)
                throw new ArgumentException("Array is null or empty.");

            T max = array[0];
            foreach (T item in array)
            {
                if (item.CompareTo(max) > 0)
                    max = item;
            }
            return max;
        }
    }

    // Q7
    public class ValueContainer<T> where T : struct
    {
        public T Value { get; set; }
    }

    // Q8
    public class ReferenceHandler<T> where T : class
    {
        public void Process(T entity)
        {
            if (entity == null) Console.WriteLine("Entity is null.");
        }
    }

    // Q9
    public class Factory<T> where T : new()
    {
        public T CreateInstance() => new T();
    }

    // Q10
    public class ComparerHelper<T> where T : IComparable<T>
    {
        public bool IsGreater(T item1, T item2) => item1.CompareTo(item2) > 0;
    }

    // Q11
    public class AnimalShelter<T> where T : Animal
    {
        public void Keep(T animal) => Console.WriteLine($"Sheltered: {animal.Name}");
    }

    // Q12
    public class AdvancedRepository<T> where T : class, IComparable<T>, new()
    {
        public T GetNewer(T existing)
        {
            T newObj = new T();
            return newObj.CompareTo(existing) > 0 ? newObj : existing;
        }
    }

    // Q13 & Q14
    public class SafeList<T>
    {
        private readonly List<T> _items = new List<T>();

        public void Add(T item) => _items.Add(item);

        public T GetAt(int index)
        {
            if (index < 0 || index >= _items.Count)
                return default(T)!;

            return _items[index];
        }
    }

    // Q18
    public class Counter<T>
    {
        public static int Count;
    }

    // Q19
    public class BaseGeneric<T> { }
    public class IntChild : BaseGeneric<int> { }
    public class GenericChild<T> : BaseGeneric<T> { }
    public class ExtendedChild<T, U> : BaseGeneric<T> { }

    // Q20
    public class CacheItem<TValue>
    {
        public TValue Value { get; }
        public DateTime ExpirationTime { get; }

        public CacheItem(TValue value, TimeSpan timeToLive)
        {
            Value = value;
            ExpirationTime = DateTime.UtcNow.Add(timeToLive);
        }

        public bool IsExpired => DateTime.UtcNow > ExpirationTime;
    }

    public class Cache<TKey, TValue> where TKey : notnull
    {
        private readonly Dictionary<TKey, CacheItem<TValue>> _cache = new Dictionary<TKey, CacheItem<TValue>>();

        public void Add(TKey key, TValue value, TimeSpan timeToLive)
        {
            _cache[key] = new CacheItem<TValue>(value, timeToLive);
        }

        public bool Contains(TKey key)
        {
            if (!_cache.TryGetValue(key, out var item))
                return false;

            if (item.IsExpired)
            {
                _cache.Remove(key);
                return false;
            }

            return true;
        }

        public TValue Get(TKey key)
        {
            if (Contains(key))
                return _cache[key].Value;

            throw new KeyNotFoundException($"Key '{key}' was not found or has expired.");
        }

        public bool Remove(TKey key)
        {
            return _cache.Remove(key);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Container<int> intBox = new Container<int>();
            intBox.Add(100);
            Console.WriteLine($"Container: {intBox.Get()}");

            Pair<int, string> student = new Pair<int, string>(1, "Ahmed");
            Console.WriteLine($"Pair: ID={student.Key}, Name={student.Value}");

            int x = 10, y = 20;
            Utilities.Swap(ref x, ref y);
            Console.WriteLine($"Swap: x={x}, y={y}");

            int[] numbers = { 5, 12, 3, 99, 45 };
            Console.WriteLine($"FindMax: Max is {Utilities.FindMax(numbers)}");

            SafeList<string> names = new SafeList<string>();
            names.Add("Route");
            Console.WriteLine($"SafeList Valid: {names.GetAt(0)}");
            Console.WriteLine($"SafeList Invalid Default: '{names.GetAt(5)}'");

            Counter<int>.Count = 5;
            Counter<string>.Count = 10;
            Console.WriteLine($"Static Counter<int>: {Counter<int>.Count}");
            Console.WriteLine($"Static Counter<string>: {Counter<string>.Count}");

            Cache<string, string> appCache = new Cache<string, string>();
            appCache.Add("User_1", "Mohamed", TimeSpan.FromSeconds(2));

            if (appCache.Contains("User_1"))
            {
                Console.WriteLine($"Cached User: {appCache.Get("User_1")}");
            }
        }
    }
}