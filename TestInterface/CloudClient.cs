using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestInterface
{
    public class CloudClient : Addovation.Cloud.Apps.AddoResources.Client.Portable.CloudClient
    {
        #region Properties

        public override string PlatformName => "iOS";
        public override string PlatformVersion => "9.1";
        public override string AppVersion => "2.0.1";
        public override string DeviceId { get; } = Guid.NewGuid().ToString();
        public override string AppId => "TestClient";

        #endregion

        protected override void OnError(string error, Exception ex)
        {
            throw ex;
        }
    }
}
