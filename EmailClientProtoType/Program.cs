using System;
using System.Net.Mail;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace EmailClientProtoType
{
	class Program
	{
		static void Main(string[] args)
		{
			using var client = new ImapClient();

			// Serverdaten des Email-Postfachs
			client.Connect("IMAP-Postfach", PORT, false);

			// Benutzername und Passwort
			client.Authenticate("Benutzername", "Passwort");

			// The Inbox folder is always available on all IMAP servers...
			var inbox = client.Inbox;
			inbox.Open(FolderAccess.ReadOnly);

			Console.WriteLine("Total messages: {0}", inbox.Count);
			Console.WriteLine("Recent messages: {0}", inbox.Recent);

			for (int i = 0; i < inbox.Count; i++)
			{
				var message = inbox.GetMessage(i);
				Console.WriteLine("Subject: {0}", message.Subject);
			}

			client.Disconnect(true);
		}
	}
}
