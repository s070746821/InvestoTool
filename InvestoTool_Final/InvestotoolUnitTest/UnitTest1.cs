using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using InvestoToolUnitTest;

namespace InvestotoolUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            Double[,] Inarray = new Double[,]
            {
                { 1, 1.2, 0.89},
                { 0.88, 1, 5.1 },
                { 1.1, 0.15, 1 }

            };

            int d = 3;


            FW TestFW = new FW(Inarray, d);

            String[] codes =  { "AUS", "USD", "JPY" };

            


            String Expected = "no arbitrage found";
            String Actual = TestFW.ReturnProft(0.1, 20, codes);

            Assert.AreNotEqual(Expected, Actual);


        }

        [TestMethod]
        public void TestMethod2()
        {

            Double[,] Inarray = new Double[,]
            {
                { 1, 0, 0},
                { 0, 1, 0 },
                { 0, 0, 1 }

            };

            int d = 3;


            FW TestFW = new FW(Inarray, d);

            String[] codes = { "AUS", "USD", "JPY" };

           

            String Expected = "no arbitrage found";
            String Actual = TestFW.ReturnProft(0.1, 20, codes);

            Assert.AreEqual(Expected, Actual);


        }


        [TestMethod]
        public void TestMethod3()
        {

            Double[,] Inarray = new Double[,]
            {
                { 1, 1.2, 0.89},
                { 0.88, 1, 5.1 },
                { 1.1, 0.15, 1 }

            };

            int d = 3;


            FW TestFW = new FW(Inarray, d);

            String[] codes = { "AUS", "USD", "JPY" };

            


            String Expected = "no arbitrage found";
            String Actual = TestFW.ReturnProft(0.99, 2, codes);

            Assert.AreEqual(Expected, Actual);


        }


        [TestMethod]

        public void TestMethod4()
        {
            ErrcheckTEST1 e = new ErrcheckTEST1();
            bool actual = e.ErrCheck(".\\Input1.txt");
            bool expected = true;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]

        public void TestMethod5()
        {
            ErrcheckTEST1 e = new ErrcheckTEST1();
            bool actual = e.ErrCheck(".\\input2.txt");
            bool expected = false;

            Assert.AreEqual(expected, actual);

        }

        [TestMethod]

        public void TestMethod6()
        {
            ErrcheckTEST1 e = new ErrcheckTEST1();
            bool actual = e.ErrCheck(".\\input3.txt");
            bool expected = false;

            Assert.AreEqual(expected, actual);


        }
    }
}
