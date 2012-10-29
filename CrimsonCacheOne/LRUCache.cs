﻿using System;
using System.Collections.Generic;

namespace CrimsonCacheOne
{
	/**
	 * The LRUCache uses a doubly-linked list to store a set of objects,
	 * keyed using objects. By putting all newly added items at the head
	 * of the list, and moving any recently accessed items to the head of 
	 * the list, the list is kept sorted. The doubly-linked list allows 
	 * for easy insertion and removal of items, and a paired Dictionary
	 * allows for quick retrieval of any item in the Cache. The size of
	 * the cache is set at construction time. 
	 */
	public class LRUCache
	{
		protected readonly int _size;
		protected readonly Dictionary<object, CacheEntry> _map;
		protected readonly CacheEntry _head;
		protected readonly CacheEntry _tail;

		public LRUCache(int size)
		{
			_size = size;
			_map = new Dictionary<object, CacheEntry>(size);
			_head = new CacheEntry(null, null);
			_tail = new CacheEntry(null, null);
			_head.Next = _tail;
			_tail.Prev = _head;
		}

		public void Add(object key, object value)
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
					_map.Remove(_tail.Prev.Key);
					_tail.Prev.Remove();
				}
			}
		}


		public bool Exists(string key)
		{
			return (Get(key) != null);
		}

		public object Get(string key)
		{
			CacheEntry entry;
			if (_map.TryGetValue(key, out entry))
			{
				entry.Remove();
				entry.Insert(_head);
				return entry.Value;
			}
			return null;
		}
	}


	public class CacheEntry
	{
		public CacheEntry(object key, object value)
		{
			Key = key;
			Value = value;
		}

		public CacheEntry Next { get; set; }
		public CacheEntry Prev { get; set; }
		public Object Key { get; set; }
		public Object Value { get; set; }

		public void Remove()
		{
			Prev.Next = Next;
			Next.Prev = Prev;
		}

		public void Insert(CacheEntry head)
		{
			Prev = head;
			Next = head.Next;
			Next.Prev = this;
			Prev.Next = this;
		}

	}
}
