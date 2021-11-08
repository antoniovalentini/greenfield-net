namespace Royale.Sdk.Players.Models
{
    public record GetUpcomingChestsResponse(Chest[] Items);

    public record Chest(int Index, string Name);
}
