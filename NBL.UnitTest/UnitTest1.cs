using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBL.Models;
using NBL.Models.Validators;

namespace NBL.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            var result = Validator.ValidateProductBarCode("12211AC023613");
            Assert.AreEqual(true, result);
        }
        [TestMethod]
        public void TestMethod1()
        {

           var  result= Generator.GenerateDateFromBarCode("1222902199636999");
            
            Assert.AreEqual(true, result);
        }
        
    }
}
