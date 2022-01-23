using UnityEngine;

namespace _Script.Extensions
{
    public class JsonExtension
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        public static string arrayToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T> { array = array };
            string json = JsonUtility.ToJson(wrapper);
            var pos = json.IndexOf(":");
            json = json.Substring(pos+1); // cut away "{ \"array\":"
            pos = json.LastIndexOf('}');
            json = json.Substring(0, pos-1); // cut away "}" at the end
            return json;
        }
    }
   
    [System.Serializable]
    public class Wrapper<T>
    {
        public T[] array;
    }
    
}