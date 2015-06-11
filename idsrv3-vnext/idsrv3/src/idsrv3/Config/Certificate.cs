using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Hosting;

namespace idsrv3.Config
{
    static class Certificate
    {
        public static X509Certificate2 Get(IHostingEnvironment environment)
        {
            var certFile = environment.WebRootPath + "\\idsrv3test.pfx";

            return new X509Certificate2(certFile, "idsrv3test");
        }

    }
}
