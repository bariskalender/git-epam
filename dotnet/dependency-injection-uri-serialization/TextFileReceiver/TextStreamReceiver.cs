using System.Collections.Generic;
using System.IO;
using DataReceiving;
using Microsoft.Extensions.Logging;

namespace TextFileReceiver
{
    public class TextStreamReceiver : IDataReceiver
    {
        private readonly string path;

        public TextStreamReceiver(string path)
        {
            this.path = path;
        }

        // mevcut constructor kalsın
        public TextStreamReceiver(string path, ILogger<TextStreamReceiver> logger)
        {
            this.path = path;
        }

        public IEnumerable<string> Receive()
        {
            return File.Exists(this.path)
                ? File.ReadAllLines(this.path)
                : new List<string>();
        }
    }
}
