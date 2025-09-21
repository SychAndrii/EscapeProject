using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EscapeProject.Task
{
    public class StrictDateTimeConverter : JsonConverter<DateTime?>
    {
        private const string Format = "yyyy-MM-dd'T'HH:mm:ss";

        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            string? str = reader.GetString();
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            // Strict parsing
            return DateTime.TryParseExact(str, Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result)
                ? (DateTime?)result
                : throw new JsonException($"Invalid date format. Expected exactly '{Format}', got '{str}'.");
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(Format, CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
