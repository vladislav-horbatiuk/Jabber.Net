using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Jabber.Net.Config.Resources;
using RazorEngine;

namespace Jabber.Net.Config
{
    public class JabberConfigTransformer
    {
        public static string ToAppConfig(JabberConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            if (config.Validate().Any())
                throw new ArgumentException(
                    ConfigurationResource.JabberConfigTransformer_ToAppConfig_Configuration_not_valid, "config",
                    new Exception(string.Join(Environment.NewLine, config.Validate())));

            //Run transform
            using (
                var reader =
                    new StreamReader(
                        typeof(JabberConfigTransformer).Assembly.GetManifestResourceStream(
                            "Jabber.Net.Config.AppConfig.cshtml")))
            {
                string result = Razor.Parse(reader.ReadToEnd(), config); //Transform coded model to template
                return result;
            }
        }

    }
}