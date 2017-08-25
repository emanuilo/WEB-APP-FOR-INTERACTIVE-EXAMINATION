using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WebRole1.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace WebRole1.Hubs
{
    public class PushHub : Hub
    {
        public void Send(string idK)
        {
            int idKan = Convert.ToInt32(idK);
            using (baza db = new baza())
            {
                var klon = db.Klons.Where(p => p.IdKan == idKan).OrderByDescending(p => p.IdKlo).FirstOrDefault();
                var ponudjeni = klon.KlonPonudjeniOdgs.ToString();
                Clients.Group("Channel " + idKan).addNewQuestion(klon.Naslov, klon.Tekst, ponudjeni);
                //Clients.All.addNewQuestion(klon.Naslov, klon.Tekst, ponudjeni);
            }
        }

        public void JoinGroup(string idKor)
        {
            using (baza db = new baza())
            {
                int userId = Convert.ToInt32(idKor);
                Korisnik korisnik = db.Korisniks.Find(userId);
                if (korisnik.Uloga != "student")
                    return;

                var pratis = db.Pratis.Where(p => p.IdKor == korisnik.IdKor).ToList();

                foreach (var item in pratis)
                {
                    Groups.Add(Context.ConnectionId, "Channel " + item.IdKan);
                }
            }
        }
    }
}