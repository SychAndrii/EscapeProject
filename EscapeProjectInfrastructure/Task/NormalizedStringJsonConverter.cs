using System.Text.Json;
using System.Text.Json.Serialization;
using BaseDomain;

namespace EscapeProjectInfrastructure.Task
{
    public class NormalizedStringJsonConverter : JsonConverter<NormalizedString>
    {
        public override NormalizedString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            string? str = reader.GetString();
            return str == null ? throw new JsonException("Expected a string for NormalizedString.") : new NormalizedString(str);
        }

        public override void Write(Utf8JsonWriter writer, NormalizedString value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
