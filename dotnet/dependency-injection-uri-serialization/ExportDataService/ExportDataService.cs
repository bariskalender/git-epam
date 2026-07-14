using System.Linq;
using Conversion;
using DataReceiving;
using Serialization;

namespace ExportDataService
{
    public class ExportDataService<T>
    {
        private readonly IDataReceiver receiver;
        private readonly IDataSerializer<T> serializer;
        private readonly IConverter<T> converter;

        public ExportDataService(
            IDataReceiver receiver,
            IDataSerializer<T> serializer,
            IConverter<T> converter)
        {
            this.receiver = receiver;
            this.serializer = serializer;
            this.converter = converter;
        }

        public void Run()
        {
            var result = this.receiver
                .Receive()
                .Select(item => this.converter.Convert(item))
                .Where(item => item is not null)
                .Select(item => item!)
                .ToList();

            this.serializer.Serialize(result);
        }
    }
}
