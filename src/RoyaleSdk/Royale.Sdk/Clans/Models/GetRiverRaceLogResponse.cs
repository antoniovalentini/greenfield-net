using System;
using System.Text.Json.Serialization;

namespace Royale.Sdk.Clans.Models
{
    public record GetRiverRaceLogResponse(RiverRace[] Items);

    public record RiverRace(
        int SeasonId,
        int SectionIndex,
        [property: JsonConverter(typeof(DateTimeConverterRoyale))] DateTime CreatedDate,
        Standing[] Standings);

    public record Standing(
        int Rank,
        int TrophyChange,
        RiverClan Clan);
}
