using PractoFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PractoFinal.Controllers
{
    public class HomeController : Controller
    {
        PractoEntities3 Practo = new PractoEntities3();
        private static List<Display> result;
        public ActionResult Index()
        {
            
            return View();
        }
        public List<Location> GetLocationList()
        {
            List<Location> getlocation = Practo.Locations.ToList();
            return getlocation;
        }
        public List<Specilization> GetSpecList()
        {
            List<Specilization> getSpecList = Practo.Specilizations.ToList();
            return getSpecList;
        }

        public ActionResult Filter(string Location, string Specilization)
        {
            result = (from d in Practo.Doctors
                      join l in Practo.Locations on d.Loc_id equals l.Loc_id
                      join s in Practo.Specilizations
                      on d.Spec_id equals s.Spec_id
                      select new Display
                      {
                          Doc_id = d.Doc_id,
                          Doc_name = d.Doc_name,
                          Doc_exp = d.Doc_exp,
                          Spec_name = s.Spec_name,
                          Loc_name = l.Loc_name,
                          Doc_fees = d.Doc_fees
                      }).ToList();
            result = result.Where(x => x.Loc_name == Location && x.Spec_name == Specilization).ToList();
            return View(result);
        }

        public ActionResult orderby()
        {
            var A = result.OrderBy(x => x.Doc_fees);
            return View("Filter", A);
        }
        public ActionResult DescOrderby()
        {
            var A = result.OrderByDescending(x => x.Doc_fees);

            return View("Filter", A);

        }
        public ActionResult Book(int? id)
        {
            TempData["id"] = id;
            ViewBag.message = id;
            var da = Convert.ToDateTime(TempData["AppointmentDate"]);
            var inner = from c in Practo.Appps
                        where c.AppointmentDate == da.Date
                        select c.TimeSlotID;
            var outer = (from p in Practo.TimeSlots
                         where !inner.Contains(p.TimeSlotID)
                         select p).ToList();
            outer.Insert(0, new TimeSlot { TimeSlotID = 0, Time = "------select TimeSlot------" });
            ViewBag.query = outer;
            return View();
        }
        [HttpPost]
        public ActionResult Book(Appp data)
        {
            TempData["PatientName"] = data.PatientName;
            TempData["Patientcity"] = data.PatientCity;
            TempData["AppointmentDate"] = data.AppointmentDate;
            TempData["MobileNo"] = data.MobileNo;
            TempData["time"] = data.TimeSlotID;

            var date = Convert.ToDateTime(TempData["AppointmentDate"]);
            var time = Convert.ToInt32(TempData["time"]);
            var id = Convert.ToInt32(TempData["id"]);

            bool exists = Practo.Appps.Any(x => x.AppointmentDate == date.Date &&
            x.TimeSlotID == time && x.Doc_id == id);
            if (exists)
            {
                TempData["status"] = "For that  Date selected TimeSlot is not available";
                return RedirectToAction("Book");

            }
            else
            {
                Practo.Appps.Add(data);
                Practo.SaveChanges();
                return RedirectToAction("Status");
            }

           

        }
        public ActionResult Status()
        {
            var item = (from a in Practo.Locations
                          join
                           b in Practo.Doctors on a.Loc_id equals b.Loc_id into table1
                          from b in table1.DefaultIfEmpty()
                          join c in Practo.Specilizations on b.Spec_id equals c.Spec_id into table2
                          from c in table2.DefaultIfEmpty()
                          join d in Practo.Appps on b.Doc_id equals d.Doc_id into table3
                          from d in table3.DefaultIfEmpty()
                          join e in Practo.TimeSlots on d.TimeSlotID equals e.TimeSlotID
                          select new AppVM
                          {
                              AppointmentID = d.AppointmentID,
                              PatientName = d.PatientName,
                              PatientCity = d.PatientCity,
                              MobileNo = d.MobileNo,
                              AppointmentDate=d.AppointmentDate,
                              Gender=d.Gender,
                              Doc_fees = b.Doc_fees,
                              Doc_exp = b.Doc_exp,
                              Doc_name = b.Doc_name,
                              Loc_name = a.Loc_name,
                              Spec_name = c.Spec_name,
                              Time = e.Time
                          }).ToList();

            var PatientName = Convert.ToString(TempData["PatientName"]);
            var PatientCity = Convert.ToString(TempData["Patientcity"]);
            var apdate = Convert.ToDateTime(TempData["AppointmentDate"]);
            var MobileNo = Convert.ToString(TempData["MobileNo"]);

            item = item.Where(x => x.MobileNo == MobileNo && x.PatientName == PatientName
                                    && x.PatientCity == PatientCity).ToList();

            return View(item);
        }
    }
}