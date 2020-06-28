using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Terminal.Collector.HMI.Core
{
    public class ConfigManager
    {
        public static IConfiguration Configuration { get; set; }

        static ConfigManager()
        {
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource
                {
                    Path = "appsettings.json",
                    ReloadOnChange = true
                }).Build();
        }
    }
}
