using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Jabber.Net.Config.Resources;

namespace Jabber.Net.Config
{
    public interface IJabberConfigItemValidate
    {
        IEnumerable<string> Validate();
    }

    [Serializable]
    [DataContract]
    public class JabberConfig : IJabberConfigItemValidate
    {
        [DataMember]
        public string Domain { get; set; }

        [DataMember]
        public string IpOrHostName { get; set; }

        public bool MucEnabled { get { return !string.IsNullOrEmpty(MucAddress); } }

        [DataMember]
        public string MucAddress { get; set; }

        [DataMember]
        public TcpListenerConfiguration TcpListener { get; set; }

        [DataMember]
        public BoshListenerConfiguration BoshListener { get; set; }

        [DataMember]
        public ComponentListenerConfiguration ComponentListener { get; set; }

        [DataMember]
        public string UsersConnectionStringName { get; set; }
        
        [DataMember]
        public string ElementsConnectionStringName { get; set; }

        #region IJabberConfigItemValidate Members

        public IEnumerable<string> Validate()
        {
            var listValidationErrors = new List<string>();
            listValidationErrors.AddRange(ValidateConfig());
            if (TcpListener != null)
                listValidationErrors.AddRange(TcpListener.Validate());
            if (ComponentListener != null)
                listValidationErrors.AddRange(ComponentListener.Validate());
            if (BoshListener != null)
                listValidationErrors.AddRange(BoshListener.Validate());
            return listValidationErrors;
        }

        #endregion

        /// <summary>
        /// Validates config and returns errors if any
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> ValidateConfig()
        {
            if (string.IsNullOrEmpty(Domain))
                yield return "Specify domain";
            if (MucEnabled && string.IsNullOrEmpty(MucAddress))
                yield return "Specify muc address";
            IPAddress parsed;
            if (!IPAddress.TryParse(IpOrHostName, out parsed))
                yield return "Malformed ip address";

            if (TcpListener == null && BoshListener == null && ComponentListener == null)
            {
                yield return "All listeners are not set. Configure any of them";
            }
        }
    }

    [Serializable]
    [DataContract]
    public class ListenerConfiguration : IJabberConfigItemValidate
    {
        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        #region IJabberConfigItemValidate Members

        public virtual IEnumerable<string> Validate()
        {
            if (Port < 0 || Port > 65535)
                yield return ConfigurationResource.Error_Wrong_Port;
        }

        #endregion
    }

    [Serializable]
    [DataContract]
    public class ComponentListenerConfiguration : ListenerConfiguration
    {
        [DataMember]
        public string Secret { get; set; }

        public override IEnumerable<string> Validate()
        {
            return base.Validate().Union(ValidateComponent());
        }

        private IEnumerable<string> ValidateComponent()
        {
            if (string.IsNullOrEmpty(Secret))
                yield return "Component listner should specify secret";
        }
    }

    [Serializable]
    [DataContract]
    public class BoshListenerConfiguration : ListenerConfiguration
    {
        [DataMember]
        public string Bind { get; set; }

        public override IEnumerable<string> Validate()
        {
            return base.Validate().Union(ValidateBosh());
        }

        private IEnumerable<string> ValidateBosh()
        {
            if (string.IsNullOrEmpty(Bind) || !Uri.IsWellFormedUriString(Bind, UriKind.Relative))
                yield return "Bosh bind should be url path";
        }
    }

    [Serializable]
    [DataContract]
    public class TcpListenerConfiguration : ListenerConfiguration
    {
        [DataMember]
        public bool EnableTls { get; set; }

        [DataMember]
        public string CertificatePath { get; set; }

        public override IEnumerable<string> Validate()
        {
            return base.Validate().Union(ValidateTcp());
        }

        private IEnumerable<string> ValidateTcp()
        {
            if (EnableTls)
            {
                //Check certificate
                if (!string.IsNullOrEmpty(CertificatePath))
                {
                    if (File.Exists(CertificatePath))
                    {
                        string certificateMessageError = string.Empty;
                        try
                        {
                            //Try load
                            var x509 = new X509Certificate2();
                            //Create X509Certificate2 object from .cer file.
                            byte[] rawData = File.ReadAllBytes(CertificatePath);
                            x509.Import(rawData);
                            if (!x509.HasPrivateKey)
                                certificateMessageError = "Certificate should contain a private key";
                        }
                        catch (Exception e)
                        {
                            certificateMessageError = e.Message;
                        }
                        if (!string.IsNullOrEmpty(certificateMessageError))
                            yield return certificateMessageError;
                    }
                }
                else
                {
                    yield return "Can't enable TLS without certificate specified";
                }
            }
        }
    }
}