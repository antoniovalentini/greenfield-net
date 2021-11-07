using System.Collections.Generic;
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
        Arena Arena,
        IEnumerable<Card> CurrentDeck);

    public record Card(
        string Name,
        int Id,
        int Level,
        int StarLevel,
        int MaxLevel,
        int Count,
        IconUrls IconUrls);

    public record IconUrls(string Medium);
}
