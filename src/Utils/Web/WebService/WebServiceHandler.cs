using System;
using int32.Utils.Core.Extensions;
using int32.Utils.Web.WebService.Contracts;

namespace int32.Utils.Web.WebService
{
    public static class WebServiceHandler
    {
        public static Response<T> Handle<T>(Func<T> action)
        {
            var response = new Response<T>();

            try
            {
                response.Result = action();

                if (!response.Error.IsNull())
                    SetFailure(response, response.Error);
            }
            catch (Exception ex)
            {
                SetFailure(response, ex.Message);
            }

            return response;
        }

        public static Response<T> Handle<T>(Func<Response<T>, T> action)
        {
            var response = new Response<T>();

            try
            {
                response.Result = action(response);

                if (!response.Error.IsNull())
                    SetFailure(response, response.Error);
            }
            catch (Exception ex)
            {
                SetFailure(response, ex.Message);
            }

            return response;
        }

        private static void SetFailure<T>(IResponse<T> response, string ex)
        {
            response.Status = StatusCode.Error;
            response.Error = ex;
            response.Result = default(T);
        }
    }
}