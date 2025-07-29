using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace Auctify.WebApp.Services;
public sealed class AuctionHubService : IAsyncDisposable {
    private HubConnection? _hubConnection;
    private readonly NavigationManager _navigationManager;
    public event Action<string, decimal>? OnNewBidReceived;

    public AuctionHubService(NavigationManager navigationManager) {
        _navigationManager = navigationManager;
    }

    public async Task StartConnectionAsync(string auctionId) {
        var hubUrl = _navigationManager.ToAbsoluteUri("/auctionhub");

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, decimal>("NewBidReceived", (receivedAuctionId, newPrice) => {
            if (receivedAuctionId == auctionId) {
                OnNewBidReceived?.Invoke(receivedAuctionId, newPrice);
            }
        });

        await _hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync() {
        if (_hubConnection is not null) {
            await _hubConnection.DisposeAsync();
        }
    }
}