using System;
using int32.Utils.Web.WebService;
using NUnit.Framework;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WebServiceTests
    {
        [TestCase]
        public void WebService_Handler_HandleSample()
        {
            var response = WebServiceHandler.Handle(GetData);

            Assert.AreEqual(StatusCode.Ok, response.Status);
            Assert.IsNull(response.Error);
            Assert.AreEqual(23, response.Result.Age);
        }

        [TestCase]
        public void WebService_Handler_HandleNull()
        {
            var response = WebServiceHandler.Handle<SampleModel>(GetDataException);

            Assert.AreEqual(StatusCode.Error, response.Status);
            Assert.AreEqual("Test", response.Error);
        }

        [TestCase]
        public void WebService_Handler_HandleException()
        {
            var response = WebServiceHandler.Handle(GetDataExceptionDirect);

            Assert.AreEqual(StatusCode.Error, response.Status);
            Assert.AreEqual("TestDirect", response.Error);
            Assert.IsNull(response.Result);
        }

        private SampleModel GetData()
        {
            return new SampleModel { Age = 23 };
        }

        private SampleModel GetDataException(Response<SampleModel> response)
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return null;
        }

        private SampleModel GetDataExceptionDirect()
        {
            throw new Exception("TestDirect");
        }
    }
}
