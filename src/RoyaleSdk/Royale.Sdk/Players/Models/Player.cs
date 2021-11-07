using Royale.Sdk.Clans.Models;

namespace Royale.Sdk.Players.Models
{
    public record Player(
        string Tag,
        string Name,
        int ExpLevel,
        int Trophies,
        int BestTrophies,
        int Wins,
        int Losses,
        int BattleCount,
        int ThreeCrownWins,
        int ChallengeCardsWon,
        int ChallengeMaxWins,
        int TournamentCardsWon,
        int TournamentBattleCount,
        string Role,
        int Donations,
        int DonationsReceived,
        int TotalDonations,
        int WarDayWins,
        int ClanCardsCollected,
        Clan Clan,
        Arena Arena);
}
