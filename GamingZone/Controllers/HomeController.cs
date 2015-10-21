using GamingZone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GamingZone.Controllers
{
    public class HomeController : Controller
    {
        // Simulates a database with a lobby table
        private readonly List<Lobby> m_lobbies = new List<Lobby>
        {
            new Lobby() { Id = "1", Name = "Fifteen Two", Tables = 250, SeatsPerTable = 2 },
            new Lobby() { Id = "2", Name = "Peg Pals", Tables = 250, SeatsPerTable = 2 }
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Games()
        {
            return View(m_lobbies);
        }

        public ActionResult Lobby(string lobbyId)
        {
            var lobby = m_lobbies.FirstOrDefault(l => l.Id == lobbyId);
            if (lobby == null)
                return HttpNotFound();

            return View(lobby);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}