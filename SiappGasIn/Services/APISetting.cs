using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SiappGasIn.Services
{

    public class APISettingOptions
    {
        public string getToken { get; set; }
        public string pipeCalculator { get; set; }

    }
}
