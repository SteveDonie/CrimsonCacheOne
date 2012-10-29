using System;

namespace CrimsonCacheOne
{
	/**
	 * The specs say that the RandomCache should be a subclass of the LRUCache.
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
				entry.Remove();
				entry.Insert(_head);
				entry.Value = value;
			}
			else
			{
				entry = new CacheEntry(key, value);
				_map.Add(key, entry);
				entry.Insert(_head);
				if (_map.Count > _size)
				{
					CacheEntry randomEntry = FindRandomEntry();
					_map.Remove(randomEntry.Key);
					randomEntry.Remove();
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