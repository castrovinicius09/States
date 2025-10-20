using System.Text.Json;
using Application.DTOs.States.Messages;

namespace Application.Mapping
{
    internal static class StatesMapping
    {
        internal static string MapMessageToJson(this StatesMessage message)
        {
            return JsonSerializer.Serialize(message.StatesList, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
    }
}
