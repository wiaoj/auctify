namespace Auctify.BiddingService.WebAPI.Features.PlaceBid; 
public sealed record PlaceBidRequest(decimal Amount); 
public sealed record PlaceBidResponse(string AuctionId, decimal NewPrice, string Message);