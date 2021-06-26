using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSshop.Data;
using BSshop.Models;
using Microsoft.AspNetCore.Identity;
using BSshop.Services;
using Microsoft.AspNetCore.Authorization;

namespace BSshop.Controllers
{
    public class ReservationController : Controller
    {
        ApplicationDbContext _dBContext;
        public ReservationController(ApplicationDbContext dBContext)
        {
            _dBContext = dBContext;
        }

        public IActionResult Index(string id)
        {
            DateTime dateTime;
            if (id == "" || id == null) dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            else try { dateTime = new DateTime(int.Parse(id[0..4]), int.Parse(id[4..6]), int.Parse(id[6..8])); }
                catch (Exception) { dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day); }


            DateTime monday = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(-(int)dateTime.DayOfWeek + 1);
            DateTime sunday = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day).AddDays(-(int)dateTime.DayOfWeek + 7);

            ViewBag.Reservations = _dBContext.Reservations.Where(x => x.Date >= monday && x.Date <= sunday).ToList();
            return View();
        }

        public IActionResult Add()
        {
            ViewBag.Save = true;
            return View();
        }
        public IActionResult Edit(Guid Id)
        {
            Console.WriteLine(Id);
            ViewBag.Reservation = _dBContext.Reservations.FirstOrDefault(x => x.Id == Id);
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(Guid resId, string id)
        {
            var reservarionDelete = _dBContext.Reservations.FirstOrDefault(x=> x.Id == resId);
            _dBContext.Remove(reservarionDelete);
            _dBContext.SaveChanges();
            return Index(id);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Add(DateTime Date, DateTime From, DateTime To, int PeopleNumber) {
            ViewBag.Save = true;

            Reservation reservation = new Reservation()
            {
                Date = Date,
                From = From,
                To = To,
                PeopleNumber = PeopleNumber,
                FromMinutes = From.Minute + From.Hour * 60,
                ToMinutes = To.Minute + To.Hour * 60,
                Email = User.Identity.Name,
            };

            ReservationCheck reservationCheck = new ReservationCheck(_dBContext);


            if (reservationCheck.isPosible(reservation))
            {
                _dBContext.Reservations.Add(reservation);
                _dBContext.SaveChanges();
                Console.WriteLine("save");
                return RedirectToAction("Index");
            }
            else
            {
                Console.WriteLine("not save");
                ViewBag.Save = false;
            }
            return View();
        }


        [HttpPost]
        [Authorize]
        public IActionResult Edit(DateTime Date, DateTime From, DateTime To, int PeopleNumber, Guid Id)
        {
            ViewBag.Save = true;

            Reservation reservation = new Reservation()
            {
                Id = Id,
                Date = Date,
                From = From,
                To = To,
                PeopleNumber = PeopleNumber,
                FromMinutes = From.Minute + From.Hour * 60,
                ToMinutes = To.Minute + To.Hour * 60,
                Email = User.Identity.Name,
            };
            Console.WriteLine(Id);

            ReservationCheck reservationCheck = new ReservationCheck(_dBContext);
            reservationCheck.RemoveGuid = Id;


            if (reservationCheck.isPosible(reservation))
            {
                try
                {
                    _dBContext.Reservations.Remove(_dBContext.Reservations.FirstOrDefault(x => x.Id == Id));
                    _dBContext.Reservations.Add(reservation);
                    _dBContext.SaveChanges();
                    Console.WriteLine("save");
                }
                catch (Exception)
                {

                }
            }
            else
            {
                Console.WriteLine("not save");
                return Edit(Id);
            }
            return Redirect($"/Reservation/Index/");
        }
    }
}
