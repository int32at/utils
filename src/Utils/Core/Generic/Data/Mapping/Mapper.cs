using Newtonsoft.Json;

namespace int32.Utils.Core.Generic.Data.Mapping
{
    public static class Mapper
    {
        public static TTarget Map<TSource, TTarget>(TSource source) where TSource : class
        {
            var json = JsonConvert.SerializeObject(source, Constants.JsonSerializerAllSettings);
            return JsonConvert.DeserializeObject<TTarget>(json, Constants.JsonSerializerAllSettings);
        }
    }
}
