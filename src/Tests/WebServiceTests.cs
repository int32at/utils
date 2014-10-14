using System;
using int32.Utils.Tests;
using int32.Utils.Web.WebService;
using NUnit.Framework;
using Tests.Core;
using Tests.Samples;

namespace Tests
{
    [TestFixture]
    public class WebServiceTests : BaseTest
    {
        [TestCase]
        public void WebService_Handler_HandleSample()
        {
            var response = WebServiceHandler.Handle(GetData);

            MakeSure.That(response.Status).Is(StatusCode.Ok);
            MakeSure.That(response.Error).Is(null);
            MakeSure.That(response.Result.Age).Is(23);
        }

        [TestCase]
        public void WebService_Handler_HandleNull()
        {
            var response = WebServiceHandler.Handle<SampleModel>(GetDataException);

            MakeSure.That(response.Status).Is(StatusCode.Error);
            MakeSure.That(response.Error).Is("Test");
        }

        [TestCase]
        public void WebService_Handler_HandleException()
        {
            var response = WebServiceHandler.Handle(GetDataExceptionDirect);

            Assert.AreEqual(StatusCode.Error, response.Status);
            Assert.AreEqual("TestDirect", response.Error);
            Assert.IsNull(response.Result);

            MakeSure.That(response.Status).Is(StatusCode.Error);
            MakeSure.That(response.Error).Is("TestDirect");
            MakeSure.That(response.Result).Is(null);
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
