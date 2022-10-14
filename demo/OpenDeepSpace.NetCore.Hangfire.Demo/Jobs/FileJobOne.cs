using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDeepSpace.NetCore.Hangfire.Demo
{

    public class FileJobOne : IAsyncBackgroundJob<FileJobOneArgs>
    {
        public async Task ExecuteAsync(FileJobOneArgs args)
        {
            var bytes = args.data;
        }
    }

    public class FileJobOneArgs
    { 
        public byte[] data;
    }
}
