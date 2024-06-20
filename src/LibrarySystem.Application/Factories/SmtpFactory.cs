using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Application.Factories;

public class SmtpFactory
{
    public static SmtpClient CreateInstance(IConfiguration configuration)
    {
        var host = configuration.GetSection("SMTP:Host").Value ?? throw new NullReferenceException();
        var port = int.Parse(configuration.GetSection("SMTP:Port").Value ?? throw new NullReferenceException());

        var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false
        };

        var username = configuration.GetSection("SMTP:Username").Value ?? throw new NullReferenceException();
        var password = configuration.GetSection("SMTP:Password").Value ?? throw new NullReferenceException();

        client.Credentials = new NetworkCredential(username, password);

        return client;
    }
}
