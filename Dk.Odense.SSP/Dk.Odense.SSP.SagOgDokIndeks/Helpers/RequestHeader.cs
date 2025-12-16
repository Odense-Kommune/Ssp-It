using System.ServiceModel.Channels;
using System.Xml;

namespace Dk.Odense.SSP.SagOgDokIndeks.Helpers
{
    public class RequestHeader : MessageHeader
    {
        private string uuid;
        public override string Name => "RequestHeader";
        public override string Namespace => "http://kombit.dk/xml/schemas/RequestHeader/1/";

        public RequestHeader(string uuid)
        {
            this.uuid = uuid;
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteElementString("TransactionUUID", uuid);
        }
    }
}