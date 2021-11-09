using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Royale.Sdk.Clans.Models
{
    public class DateTimeConverterRoyale : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (typeToConvert != typeof(DateTime))
            {
                throw new Exception($"Expected type of '{typeof(DateTime)}' but found '{typeToConvert.Name}'");
            }

            var s = reader.GetString() ?? throw new InvalidOperationException();
            return DateTime.ParseExact(s, "yyyyMMddTHHmmss.fffZ", CultureInfo.InvariantCulture);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
