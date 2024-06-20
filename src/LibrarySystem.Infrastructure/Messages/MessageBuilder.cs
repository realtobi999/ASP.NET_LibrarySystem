using System.Net.Mail;
using LibrarySystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using RazorEngine.Templating;

namespace LibrarySystem.Infrastructure.Messages;

public class MessageBuilder : IMessageBuilder
{
    private readonly IConfiguration _configuration;
    private readonly string _sender;

    public MessageBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
        _sender = _configuration.GetSection("SMTP:Username").Value ?? throw new NullReferenceException();
    }

    private MailMessage BuildBaseMessage(string toEmail)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_sender),
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        return message;
    }

    private void AttachHtml(MailMessage message, string fileName, object model)
    {
        using var stream = GetType().Assembly.GetManifestResourceStream(fileName) ?? throw new InvalidOperationException();
        using var reader = new StreamReader(stream);

        var html = reader.ReadToEnd();
        var engine = RazorEngineService.Create();
        var body = engine.RunCompile(html, Guid.NewGuid().ToString(), typeof(object), model);

        message.Body = body;
    }
}
