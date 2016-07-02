namespace Norma.Models
{
    internal interface INetworkCaptureRequestAware
    {
        void OnRequestHandling(NetworkEventArgs e);
    }

    internal interface INetworkCaptureResponseAware
    {
        void OnResponseHandling(NetworkEventArgs e);
    }
}