using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Xml.Serialization;

namespace SecretSatan
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<Person> persons;
            try
            {
                var personLines = File.ReadAllLines("persons.txt");
                persons = personLines.Select(x =>
                {
                    var pair = x.Split(':');
                    var res = new Person
                    {
                        Email = pair[1],
                        Name = pair[0]
                    };
                    return res;
                }).ToList();
            }
            catch 
            {
                Console.WriteLine($"Something is wrong with persons {_sadSmiles[_rnd.Next(_sadSmiles.Length)]}");
                Console.ReadLine();
                return;
            }
            if (persons.Count < 3)
            {
                Console.WriteLine($"There must be 3 or more persons {_sadSmiles[_rnd.Next(_sadSmiles.Length)]}");
                Console.ReadLine();
                return;
            }

            var lines = File.ReadAllLines("message.txt");
            if (lines.Length <= 1)
            {
                Console.WriteLine($"The message is empty {_sadSmiles[_rnd.Next(_sadSmiles.Length)]}");
                Console.ReadLine();
                return;
            }
            var subject = lines[0];
            var textPattern = string.Join(Environment.NewLine, lines.Except(new[] {lines[0]}));
            var dict = CreateDictionary(persons);
            while (dict.Any(x => x.Key.Equals(x.Value)))
            {
                //Console.WriteLine("Oh no, I failed to arrange them!");
                dict = CreateDictionary(persons);
            }
            Console.WriteLine($"Persons shuffled... {_happySmiles[_rnd.Next(_happySmiles.Length)]}");

            // Saving the pairs, just in case
            var list = dict.Select(x => new PersonPair() {Sender = x.Key, Receiver = x.Value}).ToList();
            XmlSerializer s = new XmlSerializer(list.GetType());
            var now = DateTime.Now;
            var filename =
                $"pairs{now.Year}{now.Month:D2}{now.Month:D2}-{now.Hour:D2}{now.Minute:D2}{now.Second:D2}.xml";
            using (var stream = File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename)))
            {
                s.Serialize(stream, list);
            }


            foreach (KeyValuePair<Person, Person> pair in dict)
            {
                using (var msg = new MailMessage())
                {
                    msg.To.Add(pair.Key.Email);
                    msg.Subject = subject;
                    msg.Body = textPattern.Replace("%name%", pair.Value.Name);
                    Send(msg);
                }
            }
            Console.WriteLine();
            Console.WriteLine("That's All Folks!");
            Console.WriteLine("Press any key to party hard...");
            Console.ReadLine();
        }

        private static readonly Random _rnd = new Random();
        private static readonly string[] _happySmiles = { ":)", ":]", ";)", ";]", ";}", ":}", ":D", "^_^" };
        private static readonly string[] _sadSmiles = {":(", ":[", ";(", ";[", ":{", ";{", ":C", ":,("};

        private static Dictionary<Person, Person> CreateDictionary(IList<Person> persons)
        {
            var res = new Dictionary<Person, Person>();
            
            while (!res.Any() || res.Any(x => x.Key.Equals(x.Value)))
            {
                res.Clear();
                var senders = persons.ToList();
                var receivers = persons.ToList();

                while (senders.Count > 1)
                {
                    var sender = senders[_rnd.Next(senders.Count)];
                    var receiver = receivers.Where(x => !Equals(x, sender)).ElementAt(_rnd.Next(receivers.Count - 1));
                    res.Add(sender, receiver);
                    senders.Remove(sender);
                    receivers.Remove(receiver);
                }
                res.Add(senders.Single(), receivers.Single());
            }
            return res;
        }


        private static void Send(MailMessage message)
        {
            if (string.IsNullOrEmpty(message.From?.Address))
            {
                message.From = new MailAddress(AppSettingsHelper.SystemEmailAddress,
                    AppSettingsHelper.SystemEmailSenderName);
            }
            using (SmtpClient smtp = new SmtpClient(AppSettingsHelper.EmailSmtpUrl, AppSettingsHelper.EmailSmtpPort))
            {
                smtp.Credentials = new NetworkCredential(AppSettingsHelper.EmailLogin, AppSettingsHelper.EmailPassword);
                smtp.EnableSsl = AppSettingsHelper.SmtpSslEnabled;

                try
                {
                    //smtp.Send(message);
                    Console.WriteLine($"Sent to {message.To.First().Address} {_happySmiles[_rnd.Next(_happySmiles.Length)]}");
                }
                catch
                {
                    Console.WriteLine($"Failed send to {message.To.First().Address} {_sadSmiles[_rnd.Next(_sadSmiles.Length)]}." +
                                      $" You must tell him everything yourself(see the pairs*.xml)");
                }
            }
        }
    }
}
