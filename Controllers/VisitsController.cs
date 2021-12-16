using Clinic_Web.Models;
using Clinic_Web.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Strona.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_Web.Controllers
{
   [Authorize(Roles = "Admin, Patient")]
    public class VisitsController : Controller
    {
        private readonly Database_controller _context;
        private readonly UserManager<Patient_account> _userManager;
        public string pesel;

        

        public VisitsController(Database_controller context, UserManager<Patient_account> userManager)
        {
            _context = context;
            _userManager = userManager;

           
        }

        // GET: Visits


        public async Task<IActionResult> Index()
        {

        var user = await _userManager.GetUserAsync(User);
           DateTime today = DateTime.Today;
            TimeSpan todaytime = new TimeSpan();
            TimeSpan onehour = TimeSpan.FromHours(1);
            todaytime = DateTime.UtcNow.TimeOfDay;
            todaytime.Add(onehour);
            pesel = user.Pesel;
            var database_controller = _context.Visits.Where(c => c.patient.Pesel == user.Pesel && c.date.DayOfYear> today.DayOfYear || c.date.Year> today.Year).Include(v => v.doctor).Include(v => v.patient);
            var list = _context.Visits.Where(c => c.patient.Pesel == user.Pesel && c.date.DayOfYear == today.DayOfYear && c.date.Hour > todaytime.Hours).Include(v => v.doctor).Include(v => v.patient);
            var list1 = _context.Visits.Where(c => c.patient.Pesel == user.Pesel && c.date.DayOfYear == today.DayOfYear && c.date.Hour == todaytime.Hours && c.date.Minute > todaytime.Minutes).Include(v => v.doctor).Include(v => v.patient);
            var sum = database_controller.Concat(list).Concat(list1);
            var result= sum.OrderByDescending(c => c.date.DayOfYear);
            return View(await result.ToListAsync());
        }

        public async Task<IActionResult> Index_previous()
        {

            var user = await _userManager.GetUserAsync(User);
            DateTime today = DateTime.Today;
            TimeSpan todaytime = new TimeSpan();
            TimeSpan onehour = TimeSpan.FromHours(1);
            todaytime = DateTime.UtcNow.TimeOfDay;
            todaytime.Add(onehour);
            pesel = user.Pesel;
            var database_controller = _context.Visits.Where(c => c.patient.Pesel == user.Pesel && c.date.DayOfYear < today.DayOfYear).Include(v => v.doctor).Include(v => v.patient);
            var list = _context.Visits.Where(c => c.patient.Pesel == user.Pesel && c.date.DayOfYear == today.DayOfYear && c.date.Hour < todaytime.Hours).Include(v => v.doctor).Include(v => v.patient);
            var sum = database_controller.Concat(list);
            var result = sum.OrderBy(c => c.date.DayOfYear);
            return View(await result.ToListAsync());
        }

    
        // GET: Visits/Create
        public async Task<IActionResult> Create()
        {


            var user = await _userManager.GetUserAsync(User);
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Fullname");

            ViewData["PatientId"] = new SelectList(_context.Patients.Where(c => c.Pesel == user.Pesel), "PatientId", "Fullname");

           

            return View();
        }

        [HttpGet]
        public List<DateTime> Updatedata(long x, DateTime data)
        {
            DateTime todaydata = DateTime.Today;
           
            var items = new List<DateTime>();
            if (data.DayOfYear < todaydata.DayOfYear && data.Year == todaydata.Year)
            { return null; }
            Doctor doctor = new Doctor();
            doctor = _context.Doctors.Find(x); ;
            Visit visit = new Visit();
            visit.doctor = doctor;

            //  var result = _context.Visits.ToList().Where(y => y.DoctorId == x);
            //  doctor.Scheduled_visits = result;
            items = doctor.get_free_visit_day(data);
            // items = doctor.get_free_visit_day(data);
            return items;
        }

        // GET: Stock

        public ActionResult get_visits(int a)

        {

            Doctor doctor1 = new Doctor();
            doctor1 = _context.Doctors.Find(a);

            var items = new List<string>() { };



            // items = doctor1.get_free_visit_day_example();
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Surname");

            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Surname");
            ViewData["visit"] = new SelectList(items);
            return View();

        }

        // POST: Visits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VisitId,PatientId,DoctorId,date,hour")] Visit visit)
        {
            if (ModelState.IsValid)
            {
                Doctor doctor = _context.Doctors.Find(visit.DoctorId);

                // doctor.get_free_visit_day(visit.date);
                // List<string> lista =  doctor.get_free_visit_day(visit.date);

                //  visit.doctor.Scheduled_visits.ToList().Add(visit);
                //visit.patient.Scheduled_visits.ToList().Add(visit);
                string[] time = visit.hour.Split(':');
                int hour = Int32.Parse(time[0]);
                int min = Int32.Parse(time[1]);
                DateTime t = new DateTime();
                t = visit.date.AddHours(hour).AddMinutes(min);
                visit.date = t;
               
                _context.Add(visit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));


            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", visit.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", visit.PatientId);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits.FindAsync(id);
            if (visit == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "Fullname");
            ViewData["PatientId"] = new SelectList(_context.Patients.Where(c => c.Pesel == visit.patient.Pesel), "PatientId", "Fullname");
            return View(visit);
        }
        

        // POST: Visits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("VisitId,PatientId,DoctorId,date,hour")] Visit visit)
        {
            if (id != visit.VisitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(visit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VisitExists(visit.VisitId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           // ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorId", "DoctorId", visit.DoctorId);
           // ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "PatientId", visit.PatientId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Visits/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visit = await _context.Visits
                .Include(v => v.doctor)
                .Include(v => v.patient)
                .FirstOrDefaultAsync(m => m.VisitId == id);
            if (visit == null)
            {
                return NotFound();
            }

            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var visit = await _context.Visits.FindAsync(id);
            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VisitExists(long id)
        {
            return _context.Visits.Any(e => e.VisitId == id);
        }
    }
}
