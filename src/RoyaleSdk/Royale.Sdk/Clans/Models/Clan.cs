using System.Collections.Generic;

namespace Royale.Sdk.Clans.Models
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
}
