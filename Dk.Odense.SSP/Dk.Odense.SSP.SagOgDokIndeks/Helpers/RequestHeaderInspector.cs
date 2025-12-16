using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Dk.Odense.SSP.SagOgDokIndeks.Helpers
{
    public class RequestHeaderInspector : IClientMessageInspector
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
            ;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            request.Headers.Add(new RequestHeader(IdUtil.GenerateUuid()));

            return null;
        }
    }
}
