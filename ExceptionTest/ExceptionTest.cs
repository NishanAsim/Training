using System;
using System.Diagnostics;

namespace ExceptionTest
{
    public static class ExceptionTestClass
    {
        private static bool ThrowExceptionMethod()
        {
            throw new Exception();
        }

        public static void Level1Sample1()
        {
            try
            {
                ThrowExceptionMethod();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public static void Level1Sample10()
        {
                ThrowExceptionMethod();
        }

        public static void Level1Sample2()
        {
            try
            {
                ThrowExceptionMethod();
            }
            catch 
            {
                //ggggg
                throw;
            }

        }

        public static void ThrowExceptionOnCondition()
        {
            if (DateTime.Now.Minute % 5 != 0)
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                throw new Exception();
            }
        }

         public static void Level1Sample4()
        {
            try
            {
                ThrowExceptionOnCondition();
            }
            catch(UnauthorizedAccessException unauthorizedAccessException)
            {
                Debug.WriteLine("Access was denied, please check your access and try again");

                Debug.WriteLine("The details of the unauthorized exception is as follows :");
                Debug.WriteLine(unauthorizedAccessException.StackTrace);
            }
            catch 
            {
                throw;
            }
            

        }

        public static void Level1Sample3()
        {
            try
            {
                ThrowExceptionMethod();
            }
            catch
            {
                Debug.WriteLine("An exception was encountered in Level1Sample3 method");
                throw;
            }

        }

        public static void HighLevelFunction()
        {
            try
            {
                // Level1Sample1();
                // Level1Sample2();
                // Level1Sample3();
                Level1Sample4();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unhandled exception : " + ex.Message + Environment.NewLine + ex.StackTrace);
            }

            Debug.WriteLine("Done!");
        }
    }
}