using System;
using System.Collections.Generic;
using NUnit.Framework;
using Should;

namespace CrimsonCacheOne
{
	[TestFixture]
	public class RandomCacheTests
	{
		[Test]
		public void Should_be_able_to_add_to_a_new_cache()
		{
			RandomCache cache = new RandomCache(10);

			string key = "key1";
			string value = "value1";

			cache.Add(key, value);
			cache.Exists(key).ShouldBeTrue();
		}

		[Test]
		public void adding_duplicate_keys_should_replace_value()
		{
			RandomCache cache = new RandomCache(10);

			string key = "key1";
			string value = "value1";

			cache.Add(key, value);
			cache.Exists(key).ShouldBeTrue();
			cache.Get(key).ShouldEqual(value);

			value = "some new value";
			cache.Add(key, value);
			cache.Get(key).ShouldEqual(value);
		}

		// Testing behavior that is supposed to be 'random' is somewhat tricky. What you want to do is 
		// run a test many times, and ensure that the results can be categorized into different 'buckets'. 
		// After many runs, each of the buckets should contain approximately the same number of results. 
		// For this particular case, we run the test above (add 4 items to a cache of size 3) a large 
		// number of times. Each time we run the test, we check which item was removed. After many runs, 
		// we expect that each item (key1..key3) should have been removed from the cache 1/3 times, plus or
		// minus some expected variation. I'm not sure what variance to expect from the system random
		// number generator. Once we've implemented a system that doesn't always choose the same item, and 
		// at least chooses each item sometimes,, we start getting into the realm of testing the underlying 
		// psuedo-random number generator used. I am going to use the .NET provided random number generator, 
		// and I don't typically write tests of other people's code, so 20% variance seems reasonable.
		[Test]
		public void check_how_random_the_removal_is()
		{
			Dictionary<string, int> testResults = new Dictionary<string, int>();
			testResults.Add("key1", 0);
			testResults.Add("key2", 0);
			testResults.Add("key3", 0);

			int numTests = 100000;
			int oneThirdOfTotalTests = numTests / 3;
			int variance = (int) (numTests * 0.20);

			int lowValue = oneThirdOfTotalTests - variance;
			int highValue = oneThirdOfTotalTests + variance;

			for (int i = 0; i < numTests; i++)
			{
				RunOneTest(testResults);
			}

			Console.WriteLine("key1 was removed " + testResults["key1"] + " times out of " + numTests + " tries.");
			Console.WriteLine("key2 was removed " + testResults["key2"] + " times out of " + numTests + " tries.");
			Console.WriteLine("key3 was removed " + testResults["key3"] + " times out of " + numTests + " tries.");

			testResults["key1"].ShouldBeInRange(lowValue, highValue);
			testResults["key2"].ShouldBeInRange(lowValue, highValue);
			testResults["key3"].ShouldBeInRange(lowValue, highValue);

		}

		private static void RunOneTest(Dictionary<string, int> testResults)
		{
			RandomCache cache = new RandomCache(3);

			cache.Add("key1", "value1");
			cache.Add("key2", "value2");
			cache.Add("key3", "value3");
			// the fourth add should cause one of the first three items to be removed. Spec isn't clear 
			// whether removing a random item could include the item just added. I decided it should not.
			cache.Add("key4", "value4");

			updateTestResults(testResults, cache, "key1");
			updateTestResults(testResults, cache, "key2");
			updateTestResults(testResults, cache, "key3");
		}

		private static void updateTestResults(Dictionary<string, int> testResults, RandomCache cache, string key)
		{
			if (!cache.Exists(key))
			{
				testResults[key] += 1;
			}
		}

	}
}