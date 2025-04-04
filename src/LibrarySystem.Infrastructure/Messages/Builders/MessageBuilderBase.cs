﻿using System.Net.Mail;
using LibrarySystem.Application.Core.Extensions;
using LibrarySystem.Domain.Exceptions.Common;
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace LibrarySystem.Infrastructure.Messages.Builders;

public abstract class MessageBuilderBase
{
    private readonly string _sender;

    protected MessageBuilderBase(IConfiguration configuration)
    {
        _sender = configuration.GetSection("SMTP:Username").Value ?? throw new NullReferenceException();
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
        var filePath = $"{DirectoryExtensions.GetProjectSourceDirectory()}/LibrarySystem.Infrastructure/Messages/Html/{fileName}";

        if (!File.Exists(filePath))
        {
            throw new HtmlTemplateMissingException(filePath);
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