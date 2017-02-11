using System;

namespace Rp2d
{
	public class RunTests
	{
		public RunTests ()
		{
		}
		
		public void Run()
		{
			SerializationTests serializationTests = new SerializationTests();
			serializationTests.Run();
			
			EntityTests entityTests = new EntityTests();
			entityTests.Run();
			
			AudioTests audioTests = new AudioTests();
			audioTests.Run();
		}
		
		public static void TestFailed(string message)
		{
			throw new Exception(message);
		}
		
		public static void AreEqual(int expected, int actual)
		{
			if(expected != actual)
			{
				TestFailed("AreEqual failed");
			}
		}
		
		public static void AreEqual<T>(IEquatable<T> expected, IEquatable<T> actual)
		{
			if(!expected.Equals(actual))
			{
				TestFailed("AreEqual failed");
			}
		}
	}
}

