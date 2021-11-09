using System;
using System.Text.Json.Serialization;

namespace Royale.Sdk.Clans.Models
{
    public record Clan(
        string Tag,
        string Name,
        string Type,
        string Description,
        long BadgeId,
        long ClanScore,
        long ClanWarTrophies,
        Location Location,
        long RequiredTrophies,
        long DonationsPerWeek,
        string ClanChestStatus,
        int ClanChestLevel,
        int ClanChestMaxLevel,
        int Members,
        ClanMember[] MemberList);

    public record RiverClan(
        string Tag,
        string Name,
        long BadgeId,
        int Fame,
        int RepairPoints,
        [property: JsonConverter(typeof(DateTimeConverterRoyale))] DateTime FinishTime,
        int PeriodPoints,
        int ClanScore,
        RiverParticipant[] Participants);

    public record RiverParticipant(
        string Tag,
        string Name,
        int Fame,
        int RepairPoints,
        int BoatAttacks,
        int DecksUsed,
        int DecksUsedToday);
}
