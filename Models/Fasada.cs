using System;
using System.Collections.Generic;
using System.Linq;

namespace Clinic_Web.Models.Models
{
    public class Fasada
    {
        private Database_controller _context;

        public Fasada(Database_controller context)
        {
            _context = context;
        }
        private List<Patient> patients = new List<Patient>();

        public List<Patient> Patients { get => patients; set => patients = value; }



        public void updatedata()
        {

            patients = _context.Patients.ToList();

        }



        public string Add_patient(Patient patient)
        {
            Patient Patient = new Patient();

            Patient.Name = patient.Name;
            Patient.Surname = patient.Surname;
            Patient.Pesel = patient.Pesel;

            String if_is = addpacjent(Patient);
            if (!"Is".Equals(if_is))
            {

                _context.Patients.Add(Patient);
                _context.SaveChanges();
                return "Dodano pacjenta";
            }
            else
            { return "Ten pacjent już istnieje"; }
        }

        public String addpacjent(Patient val)
        {
            bool if_is = patients.Contains(val);
            if (if_is == true)
            {
                return ("Is");
            }
            else
            {
                patients.Add(val);
                return null;
            }
        }


    }
}
