using Newtonsoft.Json;
using System.Collections;

namespace AspNet.Core.Extensions
{
    public static class CollectionExtensions
    {

        public static string ToJson(this IEnumerable collection)
        {
            return JsonConvert.SerializeObject(collection);
        }
    }
}
