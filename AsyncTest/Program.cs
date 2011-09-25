using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTest
{
	using System.Threading;

	class Program
	{
		static void Main( string[] args )
		{
			Console.WriteLine("Main Thread ID: " + Thread.CurrentThread.ManagedThreadId);
			DoAsyncTest();
			Console.ReadLine();
		}

		async static void DoAsyncTest()
		{
			Console.WriteLine( "DoAsyncTest Thread ID: " + Thread.CurrentThread.ManagedThreadId );
			Console.WriteLine( "Before DoAsyncPrint()" );
			await TaskEx.Run( () => DoAsyncPrint() );
			Console.WriteLine( "After DoAsyncPrint()" );
		}

		async static void DoAsyncPrint()
		{
			Console.WriteLine( "DoAsyncPrint Thread ID: " + Thread.CurrentThread.ManagedThreadId );
			while ( true )
			{
				Console.WriteLine("Beep");
				await TaskEx.Delay( 1000 );
				Console.WriteLine( "DoAsyncPrint Thread ID: " + Thread.CurrentThread.ManagedThreadId );
			}
		}
	}
}
