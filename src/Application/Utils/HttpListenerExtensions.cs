using System.Net;

namespace Retryrr.Application.Utils;

public static class HttpListenerExtensions
{
    public static async Task BeginGetContextAsync(
        this HttpListener listener, 
        AsyncCallback? callback, 
        CancellationToken cancellationToken) =>
        await Task.Factory
            .FromAsync(listener.BeginGetContext(callback, null), _ => { })
            .HandleCancellation(cancellationToken);
}