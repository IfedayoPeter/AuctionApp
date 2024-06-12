using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuctionApp.Data;

public static class JsonHelper
{
    public static string SerializeWithReferenceHandling(object obj)
    {
        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(obj, options);
    }
}