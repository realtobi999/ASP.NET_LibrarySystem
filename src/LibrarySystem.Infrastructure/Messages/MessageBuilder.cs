using System.Net.Mail;
using LibrarySystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

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
        var message = new MailMessage();

        message.From = new MailAddress(_sender);
        message.To.Add(toEmail);
        message.IsBodyHtml = true;

        return message;
    }
}
