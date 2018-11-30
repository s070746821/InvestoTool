using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]

        public void TestMethod1()
        {
            ErrcheckTEST e = new ErrcheckTEST();
            bool actual = e.ErrCheck("C:\\Users\\ljnio\\Desktop\\UnitTestFiles\\input1.txt");
            bool expected = true;

            Assert.AreEqual(expected, actual);

        }


        public void TestMethod2()
        {
            ErrcheckTEST e = new ErrcheckTEST();
            bool actual = e.ErrCheck("C:\\Users\\ljnio\\Desktop\\UnitTestFiles\\input2.txt");
            bool expected = false;
                
            Assert.AreEqual(expected, actual);

        }



        public void TestMethod3()
        {
            ErrcheckTEST e = new ErrcheckTEST();
            bool actual = e.ErrCheck("C:\\Users\\ljnio\\Desktop\\UnitTestFiles\\input3.txt");
            bool expected = false;

            Assert.AreEqual(expected, actual);


        }


    }
}
