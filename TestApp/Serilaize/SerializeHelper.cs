using Newtonsoft.Json;
using System.Text;

namespace AramexApp.Serilaize
{
    public static class SerializeHelper
    {
        public static class SerializationHelper
        {
            public static StringContent SerializeModel<T>(T model)
            {
                string serializedData = JsonConvert.SerializeObject(model);
                StringContent stringContent = new StringContent(serializedData, Encoding.UTF8, "application/json");

                return stringContent;
            }
        }

    }
}
