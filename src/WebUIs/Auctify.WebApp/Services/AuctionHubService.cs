using Microsoft.AspNetCore.SignalR.Client;

namespace Auctify.WebApp.Services;
public sealed class AuctionHubService : IAsyncDisposable {
    private HubConnection? _hubConnection;
    public event Action<string, decimal>? OnNewBidReceived;


    public async Task StartConnectionAsync(string auctionId) {
        string hubUrl = "https://localhost:7164/auctionhub";

        this._hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .WithAutomaticReconnect()
            .Build();

        this._hubConnection.On<string, decimal>("NewBidReceived", (receivedAuctionId, newPrice) => {
            if (receivedAuctionId == auctionId) {
                OnNewBidReceived?.Invoke(receivedAuctionId, newPrice);
            }
        });

        await this._hubConnection.StartAsync();
    }

    public async ValueTask DisposeAsync() {
        if (this._hubConnection is not null) {
            await this._hubConnection.DisposeAsync();
        }
    }
}