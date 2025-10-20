using System.Text;
using System.Text.Json;
using Application.Messaging;

namespace Application.Mapping
{
    internal static class StatesMapping
    {
        internal static MemoryStream MapMessageToJson(this StatesMessage message)
        {
            string json = JsonSerializer.Serialize(message.StatesList, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

            return stream;
        }
    }
}
