using System;
namespace AuthSystem.core.Interfaces.Email
{
    public interface IEmailService
    {
        void Send(string to, string subject, string html, string from = null);
    }
}

