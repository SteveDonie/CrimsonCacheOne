using System;

namespace CrimsonCacheOne
{
	/**
	 * The specs say that the RandomCache should be a subclass of the LRUCache, 
	 * but that is just a really stupid way to try to design this. The LRU strategy
	 * is really intrinsic to the implementation of the LRU cache, so to subclass
	 * it means we will have to override pretty much every method. Even the constructor
	 * is different. We would have to really create an abstract class, which might as well
	 * be an interface, because there won't be any implementation in it. 
	 * 
	 */
	public class RandomCache : LRUCache
	{
		private readonly Random _random;

		public RandomCache(int size)
			: base(size)
		{
			_random = new Random();
		}

		public new void Add(object key, object value)
		{
			CacheEntry entry;
			if (_map.TryGetValue(key, out entry))
			{
				remove(entry);
				Insert(entry);
				entry.Value = value;
			}
			else
			{
				entry = new CacheEntry(key, value);
				_map.Add(key, entry);
				Insert(entry);
				if (_map.Count > _size)
				{
					CacheEntry randomEntry = FindRandomEntry();
					_map.Remove(randomEntry.Key);
					remove(randomEntry);
				}
			}

		}

		private CacheEntry FindRandomEntry()
		{
			int randomNumber = _random.Next(_size);
			CacheEntry victim = _head.Next;
			for (int i = 0; i <= randomNumber; i++)
			{
				victim = victim.Next;
			}
			return victim;
		}
	}
}