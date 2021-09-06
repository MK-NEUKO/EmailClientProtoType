using System;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace EmailClientProtoType
{
	class Program
	{
		static void Main(string[] args)
		{
			var imapServer = "imaps.server.de";
			var imapPort = 007;

			var smtpServer = "smtps.server.de";
			var smtpPort = 007;

			Console.Write("Email...: ");
			string email = Console.ReadLine();
			Console.Write("Passwort: ");
			string password = Console.ReadLine();
			Console.WriteLine();

			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("MK-NEUKO-EmailClientProtoType", "email@adresse.de"));
			message.To.Add(new MailboxAddress("Alias", "email@adresse.de"));
			message.Subject = "+++Test+++ EmailClientProtoType +++Test+++";
			message.Body = new TextPart("plain")
			{
				Text = @"Hallo Empfänger,

				Die Email wurde vom EmailClientProtoType versendet.

				--ProtoType"
			};

			var smtpClient = new SmtpClient();

			try
			{
				SendTestEmail(smtpClient, message, smtpServer, smtpPort, email, password);
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				smtpClient.Disconnect(true);
				smtpClient.Dispose();
			}


			var imapClient = new ImapClient();

			try
			{
				RetrieveInbox(imapClient, imapServer, imapPort, email, password);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
			finally
			{
				imapClient.Disconnect(true);
				imapClient.Dispose();
			}					
		}

		private static void SendTestEmail(SmtpClient client, MimeMessage message, string server, int port, string email, string password)
		{			
			client.Connect(server, port, true);

			// Note: only needed if the SMTP server requires authentication
			client.Authenticate(email, password);
			client.Send(message);
		}

		private static void RetrieveInbox(ImapClient client, string server, int port, string email, string password)
		{			
			client.Connect(server, port, true);			
			client.Authenticate(email, password);
			
			var inbox = client.Inbox;
			inbox.Open(FolderAccess.ReadOnly);

			Console.WriteLine("Total messages: {0}", inbox.Count);
			Console.WriteLine("Recent messages: {0}", inbox.Recent);

			for (int i = 0; i < inbox.Count; i++)
			{
				var message = inbox.GetMessage(i);
				Console.WriteLine("Subject: {0}", message.Subject);
			}
		}
	}
}
