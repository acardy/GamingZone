using GamingZone.Data;
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
        private IApplicationDbContext m_databaseContext;

        public HomeController(IApplicationDbContext appContext)
        {
            m_databaseContext = appContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Games()
        {
            return View(m_databaseContext.Lobbies);
        }

        [Authorize]
        public ActionResult Lobby(string lobbyId)
        {
            var lobby = m_databaseContext.Lobbies.FirstOrDefault(l => l.Id == lobbyId);
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