using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RazorSample.Pages.Clans.Models
{
    public record Clan(string Tag, string Name, string Type, string Description, long BadgeId,
        long ClanScore,
        long ClanWarTrophies,
        Location Location,
        long RequiredTrophies,
        long DonationsPerWeek,
        string ClanChestStatus,
        int ClanChestLevel,
        int ClanChestMaxLevel,
        int Members,
        IEnumerable<ClanMember> MemberList);

    public record Location(long Id, string Name, bool IsCountry, string CountryCode);

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

    public record Arena(long Id, string Name);

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
