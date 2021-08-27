using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            using (var client = new ImapClient())
            {
                using (var cancel = new CancellationTokenSource())
                {
                    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
                    client.Connect("imap.gmail.com", 993, true, cancel.Token);
                    client.AuthenticationMechanisms.Remove("XOAUTH");
                    client.Authenticate("mailID", "pass", cancel.Token);
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly, cancel.Token);
                    List<string> m = new List<string>();
                    for (int i = 0; i < 50; i++)
                    {
                        var message = inbox.GetMessage(i, cancel.Token);
                        m.Add(message.Subject);
                    }
                    client.Disconnect(true, cancel.Token);
                    ViewBag.MailCount = m.Count();
                    ViewBag.MailList = m;

                }
            }

            return View();
        }

    }
}