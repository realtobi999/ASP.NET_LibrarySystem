using System.Net.Mail;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Exceptions;
using LibrarySystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace LibrarySystem.Infrastructure.Messages;

public abstract class MessageBuilderBase
{
    private readonly IConfiguration _configuration;
    private readonly string _sender;

    public MessageBuilderBase(IConfiguration configuration)
    {
        _configuration = configuration;
        _sender = _configuration.GetSection("SMTP:Username").Value ?? throw new NullReferenceException();
    }

    protected MailMessage BuildBaseMessage(string toEmail)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_sender),
            IsBodyHtml = true
        };
        message.To.Add(toEmail);

        return message;
    }

    protected static string AttachHtml(string fileName, object model)
    {
        var filePath = string.Format("{0}/LibrarySystem.Infrastructure/Messages/Html/{1}", DirectoryExtensions.GetProjectSourceDirectory(), fileName);

        if (!File.Exists(filePath))
        {
            throw new HtmlTemplateNotFoundException(filePath);
        }

        var html = File.ReadAllText(filePath);

        var engine = new RazorLightEngineBuilder()
                      .UseFileSystemProject(Path.GetDirectoryName(filePath))
                      .UseMemoryCachingProvider()
                      .Build();

        var body = engine.CompileRenderStringAsync(Guid.NewGuid().ToString(), html, model).Result;

        return body;
    }
}
