using System.Text.Json;
using System.Text.Json.Serialization;

namespace NvidiaPersonasJapanDataVirtualSurvey.Core.Converters;

public class NullableUnixTimestampConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }
        if (reader.TokenType == JsonTokenType.Number)
        {
            long unixTime = reader.GetInt64();
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
        }
        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteNumberValue(((DateTimeOffset)value.Value).ToUnixTimeSeconds());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
