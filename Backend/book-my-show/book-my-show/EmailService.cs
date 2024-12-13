using MailKit.Net.Smtp;
using MimeKit;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _port = 587;
    private readonly string _email = "vaishnavimene1234@gmail.com";
    private readonly string _password = "9511688250";

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Vaishnavi", _email));
        emailMessage.To.Add(new MailboxAddress("yaravakavyareddy39@gmail.com", toEmail));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("plain") { Text = message };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_email, _password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}
