using System.Net;

namespace Retryrr.Application.Utils;

public static class HttpListenerExtensions
{
    public static IAsyncResult BeginGetContext(this HttpListener listener, AsyncCallback? callback) =>
        listener.BeginGetContext(callback, null);
}