using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Royale.Sdk.Clans.Models
{
    public record ClanMember(
        string Tag,
        string Name,
        string Role,
        [property: JsonConverter(typeof(DateTimeConverterRoyale))] DateTime LastSeen,
        int ExpLevel,
        int Trophies,
        Arena Arena,
        int ClanRank,
        int PreviousClanRank,
        int Donations,
        int DonationsReceived,
        int ClanChestPoints);

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
