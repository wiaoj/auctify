using Wiaoj.Corvus.Abstractions;

namespace Auctify.Libraries.Shared.Contracts;
public sealed record NewBidPlacedEvent(Guid AuctionId, decimal NewPrice) : IEvent;