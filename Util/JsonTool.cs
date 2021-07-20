using Newtonsoft.Json;

namespace KakaoManagerBeta.Util
{
    public static class JsonTool
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ConvertJsonToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
