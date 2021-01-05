using System;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleDelegateExample();

            //DoSomeWorkAsyncWithJustAsynchronousWaiting();

            //DoSomeWorkAsyncWithContinuation();

            var backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += DoWork;
            backgroundWorker1.ProgressChanged += ProgressChanged;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.RunWorkerAsync(); 
        }

        private static void DoWork(object sender, DoWorkEventArgs eventArgs)
        {
            var backgroundWorker = sender as BackgroundWorker;

            for (int i = 0; i < 100; i++)  
            {
                Thread.Sleep(1000);
                backgroundWorker?.ReportProgress(i);
            }
        }

        private static void ProgressChanged(object sender, ProgressChangedEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.ProgressPercentage);
        }

        #region Delegate basis
        public delegate double UsefulLogicDelegate(int firstParameter, int secondParameter);

        static void SimpleDelegateExample()
        {
            Console.WriteLine("Hello World!");

            int firstValue = 32;
            int secondValue = 44;

            UsefulLogicDelegate baseMathOperations = Sum;
            baseMathOperations += Difference;
            baseMathOperations += Multiplication;
            baseMathOperations += Division;

            var lastValue = baseMathOperations.Invoke(firstValue, secondValue);
        }

        static double Sum(int firstParameter, int secondParameter)
        {
            var result = firstParameter + secondParameter;
            Console.WriteLine($"{firstParameter} + {secondParameter} = {result}");
            return result;
        }

        static double Difference(int firstParameter, int secondParameter)
        {
            var result = firstParameter - secondParameter;
            Console.WriteLine($"{firstParameter} - {secondParameter} = {result}");
            return result;
        }

        static double Multiplication(int firstParameter, int secondParameter)
        {
            var result = firstParameter * secondParameter;
            Console.WriteLine($"{firstParameter} * {secondParameter} = {result}");
            return result;
        }

        static double Division(int firstParameter, int secondParameter)
        {
            var result = (double)firstParameter / (double)secondParameter;
            Console.WriteLine($"{firstParameter} / {secondParameter} = {result}");
            return result;
        }
        #endregion

        #region Asynchronous waiting
        public delegate void AsyncMethodCaller(int callDuration, out int threadId);

        private static void DoSomeWorkAsyncWithJustAsynchronousWaiting()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;

            // Create the delegate.
            AsyncMethodCaller asyncCaller = TestMethod;

            // Initiate the asynchronous call.
            IAsyncResult result = asyncCaller.BeginInvoke(5000,
                out threadId, null, null);

            Console.WriteLine($"Main thread {threadId} can do some useful work here while waiting");

            // Wait for the WaitHandle to become signaled.
            result.AsyncWaitHandle.WaitOne();

            // Call EndInvoke to retrieve the results.
            asyncCaller.EndInvoke(out threadId, result);

            // Close the wait handle.
            result.AsyncWaitHandle.Close();

            Console.WriteLine($"The call executed on thread {threadId}.");
            Console.ReadLine();
        }

        // The method to be executed asynchronously.
        public static void TestMethod(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine($"My call time was {callDuration}.");
        }

        #endregion

        #region Callback example
        public delegate string AsyncCaller(int callDuration, out int threadId);

        // The method to be executed asynchronously.
        public static string TestMethodWithReturnedValue(int callDuration, out int threadId)
        {
            Console.WriteLine("Test method begins.");
            Thread.Sleep(callDuration);
            threadId = Thread.CurrentThread.ManagedThreadId;
            return $"My call time was {callDuration}.";
        }

        private static void DoSomeWorkAsyncWithContinuation()
        {
            // Create the delegate.
            AsyncCaller caller = TestMethodWithReturnedValue;

            var defaultValue = 0;

            IAsyncResult result = caller.BeginInvoke(3000,
                out defaultValue,
                CallbackMethod,
                "The call executed on thread {0}, with return value \"{1}\".");

            Console.WriteLine("The main thread {0} continues to execute...",
                Thread.CurrentThread.ManagedThreadId);

            Console.ReadLine();
        }

        // The callback method must have the same signature as the
        // AsyncCallback delegate.
        static void CallbackMethod(IAsyncResult asyncResult)
        {
            // Retrieve the delegate.
            AsyncResult result = (AsyncResult)asyncResult;
            AsyncCaller caller = (AsyncCaller)result.AsyncDelegate;

            // Retrieve the format string that was passed as state
            // information.
            string formatString = (string)asyncResult.AsyncState;

            // Define a variable to receive the value of the out parameter.
            // If the parameter were ref rather than out then it would have to
            // be a class-level field so it could also be passed to BeginInvoke.
            int threadId = 0;

            // Call EndInvoke to retrieve the results.
            string returnValue = caller.EndInvoke(out threadId, asyncResult);

            // Use the format string to format the output message.
            Console.WriteLine(formatString, threadId, returnValue);
        }
        #endregion
    }
}
