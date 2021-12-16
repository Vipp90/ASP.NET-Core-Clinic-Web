using Clinic_Web.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Clinic_Web.Models
{
    public class Patient
    {

        public long PatientId { get; set; }

        [DisplayName("Imię")]
        public string Name { get; set; }
        [DisplayName("Nazwisko")]
        public string Surname { get; set; }
        public string Pesel { get; set; }

        public string Fullname
        {
            get { return Name + " " + Surname; }
        }



        public virtual ICollection<Visit> Scheduled_visits { get; set; }
        public Patient()

        {


        }

        public String toString2()
        {

            String pom1 = "\n" + this.Name + " " + this.Surname + "\n";
            return pom1;

        }

        public override bool Equals(Object ob)
        {
            String Name = Surname;
            String Name2 = ((Patient)ob).Surname;
            String Pesel1 = Pesel;
            String Pesel2 = ((Patient)ob).Pesel;
            bool a = Name.Equals(Name2);

            if (Pesel2 != "0")
            {

                bool b = Pesel1.Equals(Pesel2);
                bool c = false;
                if (a && b == true)
                {
                    c = true;
                }
                return c;
            }

            else return a;
        }
    }
}

