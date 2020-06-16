using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtterTravels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace OtterTravels.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context {get; set;}
        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.Otters = _context.Otters.ToList();
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(Otter newbie)
        {
            if(ModelState.IsValid)
            {
                if(_context.Otters.Any( o => o.Name == newbie.Name))
                {
                    ModelState.AddModelError("Name","Choose anotter name!!!");
                    ViewBag.Otters = _context.Otters.ToList();
                    return View("Index");
                }
                else
                {
                    _context.Otters.Add(newbie);
                    _context.SaveChanges();
                    HttpContext.Session.SetInt32("OtterId",newbie.OtterId);
                    return Redirect($"/dashboard/{newbie.OtterId}");
                }
            }
            else
            {
                ViewBag.Otters = _context.Otters.ToList();
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(int OtterId)
        {
            HttpContext.Session.SetInt32("OtterId",OtterId);
            return Redirect($"/dashboard/{OtterId}");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet("vacation/new")]
        public IActionResult AddVacation()
        {
            return View();
        }

        [HttpPost("/create/vacation")]
        public IActionResult CreateVacation(Vacation newVac)
        {
            if(ModelState.IsValid)
            {
                int? otterId = HttpContext.Session.GetInt32("OtterId");
                List<Vacation> myVacays = _context.Otters
                    .Include(o => o.PlannedVacations)
                    .FirstOrDefault(o => o.OtterId == (int) otterId)
                    .PlannedVacations.ToList();
                if(IsAlreadyBooked(newVac, myVacays))
                {
                    ModelState.AddModelError("StartDate", "You are already on a vacation at this time");
                    return View("AddVacation");
                }
                newVac.OtterId = (int)otterId;
                _context.Vacations.Add(newVac);
                _context.SaveChanges();

                return Redirect($"/dashboard/{otterId}");
            }
            else
            {
                return View("AddVacation");
            }
        }

        public static bool IsAlreadyBooked(Vacation newVac, List<Vacation> vacations)
        {
            DateTime testStart = newVac.StartDate;
            DateTime testEnd = newVac.StartDate.AddDays(newVac.NumberDays);
            foreach(var v in vacations)
            {
                DateTime start = v.StartDate;
                DateTime end = v.StartDate.AddDays(v.NumberDays);
                // no part of the testStart or testEnd can overlap this block of time
                if(start <= testEnd && testStart <= end)
                {
                    return true;
                }
            }
            return false;
        }

        [HttpGet("dashboard/{otterId}")]
        public IActionResult Dashboard (int otterId)
        {   
            if(HttpContext.Session.GetInt32("OtterId") == null)
            {
                return RedirectToAction("Logout");
            }
            Otter otterInDb = _context.Otters
                .Include( o => o.PlannedVacations )
                .FirstOrDefault( o => o.OtterId == otterId);
            ViewBag.myVacays = otterInDb.PlannedVacations
                .OrderBy(v => v.StartDate).ToList();
            return View(otterInDb);
        }
    }
}
