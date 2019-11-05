using System;
using System.Threading.Tasks;

namespace ExceptionTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Console.WriteLine("Hello World!");
            // ExceptionTestClass.HighLevelFunction();
            // List.A
            // for(int i=0;i<10; i++)
            // {
            //    var result = AsyncClient.GetResponseAsync();
            // }

            await TestAsyncAction();

        }

        private static async Task TestAsyncAction()
        {
            var result1 = AsyncClient.GetResponseAsync();
            var result2 = AsyncClient.GetResponseAsync();

            var result3 = AsyncClient.GetResponseAsync();
            var result4 = AsyncClient.GetResponseAsync();
            // void method with async operation
           AsyncClient.GetResponseVoid();
            await AsyncClient.GetResponseTask();
            await result4.ConfigureAwait(false);
            await result3;
            await result2;
            await result1;
            
        }
    }
}
