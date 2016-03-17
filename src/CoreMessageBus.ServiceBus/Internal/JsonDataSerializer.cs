using System;
using System.Text;
using Newtonsoft.Json;

namespace CoreMessageBus.ServiceBus.Internal
{
    public class JsonDataSerializer : IDataSerializer
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public string ContentType => "application/json";
        public Encoding Encoding => Encoding.UTF8;
        public object Serialize<TMessage>(TMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            return JsonConvert.SerializeObject(message, _settings);
        }

        public object Deserialize(string item, Type target)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (target == null) throw new ArgumentNullException(nameof(target));
            return JsonConvert.DeserializeObject(item, target, _settings);
        }
    }
}