using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BSshop.Data;
using BSshop.Models;

namespace BSshop.Services
{
    public class ReservationCheck
    {
        ApplicationDbContext _dBContext;

        public Guid RemoveGuid { get; set; }

        public ReservationCheck(ApplicationDbContext dBContext)
        {
            _dBContext = dBContext;
        }
        public bool isPosible(Reservation reservation)
        {
            bool possible = true;

            if ((reservation.ToMinutes - reservation.FromMinutes) >= 0)
                foreach (var _reservation in _dBContext.Reservations.Where(x => (x.Date.Date == reservation.Date.Date && x.Id != RemoveGuid)))
                {
                    if (reservation.From < _reservation.From)
                    {
                        Console.WriteLine("1");
                        if (reservation.To <= _reservation.From) possible = true;
                        else return possible = false;
                    }
                    else
                    {
                        Console.WriteLine("2");
                        if (reservation.From >= _reservation.To) possible = true;
                        else return possible = false;
                    }

                    Console.WriteLine(possible);
                }

            return possible;
        }
    }
}
