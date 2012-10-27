CrimsonCacheOne
===============

This is a Visual Studio 2010 solution that contains classes and tests that implement two different in-memory caches.

The first is a cache that uses a Least Recently Used technique to remove the oldest item in the cache when adding an
item that exceeeds the cache capacity. The code uses a doubly-linked list to store the keys and values, with the most recently
used item at the head of the list. As items are accessed, they are moved to the head of the list. A Dictionary (Map) is
used to allow for quick retrieval without traversing the list. 

The second class is a subclass of the first cache, but that uses a technique that randomly removes an item from the cache when
adding an item that exceeds the cache capacity.

