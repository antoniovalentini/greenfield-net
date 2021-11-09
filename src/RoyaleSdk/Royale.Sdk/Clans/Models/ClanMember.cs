using System;
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
}
