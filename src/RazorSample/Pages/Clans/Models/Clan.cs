using System.Collections.Generic;

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
        IEnumerable<object> MemberList);

    public record Location(long Id, string Name, bool IsCountry, string CountryCode);
}
