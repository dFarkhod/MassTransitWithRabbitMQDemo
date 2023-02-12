using MassTransit;
using MassTransitDemo.Common;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace MassTransitDemo.Publisher.Extensions;

internal static class ServiceCollectionExtensions
{
    private static RabbitMqSettings _rabbitMqSettings = null;
    private static X509Certificate2 _x509Certificate2 = null;

   
    internal static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration config)
    {
        var rabbitMqSection = config.GetSection("RabbitMqSettings");
        var _rabbitMqSettings = rabbitMqSection.Get<RabbitMqSettings>();
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(_rabbitMqSettings.Host, _rabbitMqSettings.VirtualHost, host =>
                {
                    host.Username(_rabbitMqSettings.Username);
                    host.Password(_rabbitMqSettings.Password);

                    if (_rabbitMqSettings.UseSSL)
                    {
                        LoadCert();
                        host.UseSsl(ssl =>
                        {
                            ssl.ServerName = Dns.GetHostName();
                            ssl.AllowPolicyErrors(SslPolicyErrors.RemoteCertificateNameMismatch);
                            ssl.Certificate = _x509Certificate2;
                            ssl.Protocol = SslProtocols.Tls12;
                            ssl.CertificateSelectionCallback = CertificateSelectionCallback;
                        });
                    }
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddHostedService<MessagePublisher>();
        return services;
    }

    private static void LoadCert()
    {
        var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
        store.Open(OpenFlags.ReadOnly);

        try
        {
            X509Certificate2Collection certificatesInStore = store.Certificates;
            _x509Certificate2 = certificatesInStore.OfType<X509Certificate2>()
                            .FirstOrDefault(cert => cert.Thumbprint?.ToLower() == _rabbitMqSettings.SSLThumbprint?.ToLower());
        }
        finally
        {
            store.Close();
        }
    }
    private static X509Certificate CertificateSelectionCallback(object sender, string targethost, X509CertificateCollection localcertificates, X509Certificate remotecertificate, string[] acceptableissuers)
    {
        var serverCertificate = localcertificates.OfType<X509Certificate2>()
                                .FirstOrDefault(cert => cert.Thumbprint.ToLower() == _rabbitMqSettings.SSLThumbprint.ToLower());

        return serverCertificate ?? throw new Exception("Wrong certificate");
    }
}
