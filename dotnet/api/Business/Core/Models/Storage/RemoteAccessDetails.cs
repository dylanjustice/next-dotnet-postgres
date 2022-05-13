using AndcultureCode.CSharp.Core.Interfaces.Providers.Storage;

namespace DylanJustice.Demo.Business.Core.Models.Storage
{
    public class RemoteAccessDetails : IRemoteAccessDetails
    {
        public string Url { get; set; }
    }
}