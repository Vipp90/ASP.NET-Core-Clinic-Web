using Clinic_Web.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Web.Models
{
    public class Doctor
    {
        public long DoctorId { get; set; }
        [DisplayName("Imię")]
        public string Name { get; set; }
        [DisplayName("Nazwisko")]
        public string Surname { get; set; }
        [DisplayName("Specjalizacja")]
        public string Specialization { get; set; }

        public string Fullname
        {
            get { return Name + " " + Surname; }
        }



        public virtual IEnumerable<Visit> Scheduled_visits { get; set; }


        public String toString2()
        {

            String pom1 = "\n" + this.Name + " " + this.Surname + "\n";
            return pom1;

        }

        public override bool Equals(Object ob)
        {
            String Name = Surname;
            String Name2 = ((Doctor)ob).Surname;
            string Specialization1 = Specialization;
            string Specialization2 = ((Doctor)ob).Specialization;
            bool a = Name.Equals(Name2);
            if (Specialization2 != "")
            {

                bool b = Specialization1.Equals(Specialization2);
                bool c = false;
                if (a && b == true)
                {
                    c = true;
                }
                return c;
            }

            else return a;
        }

        public List<String[]> get_visit_day(DateTime data)

        {
            List<String[]> list = new List<String[]>();

            if (Scheduled_visits == null)
                return null;

            else
                foreach (Visit next in Scheduled_visits)
                {
                    if (next.date.DayOfYear.Equals(data.DayOfYear))
                    {
                        String[] visit = new String[2];
                        visit[0] = next.patient.Name.ToString() + " " + next.patient.Surname.ToString();
                        visit[1] = next.date.ToString();
                        list.Add(visit);
                    }
                }
            return list;
        }

        public List<DateTime> get_free_visit_day_example()

        {

            DateTime data = new DateTime();

            List<String> list = new List<String>();
            List<DateTime> hours = new List<DateTime>();
            DateTime hour = new DateTime();
            hour = hour.AddHours(9).AddMinutes(00);


            for (int j = 0; j <= 16; j++)
            {
                /// hours.Add(hour.ToShortTimeString());
                hours.Add(hour);
                hour = hour.AddMinutes(30);
            }

            return hours;

        }

        public List<DateTime> get_free_visit_day(DateTime data)

        {
            DateTime today = DateTime.Today;
            List<String> list = new List<String>();
            //var parsedDate = DateTime.Parse(data);
            TimeSpan todaytime = new TimeSpan();
            todaytime = DateTime.UtcNow.TimeOfDay;
                

            List<DateTime> hours = new List<DateTime>();
            String hourr;
            DateTime hour = new DateTime();
            

            if (today.DayOfYear == data.DayOfYear)
            {
                int h = todaytime.Hours +1;
                if (h < 14)
                {
                    hour = hour.AddHours(h + 2);

                }
                else { return null; }

            }
            else { hour = hour.AddHours(9).AddMinutes(00); }
            if (Scheduled_visits == null)
            {
                do
                {
                    // hours.Add(hour.ToShortTimeString());
                    hours.Add(hour);
                    hour = hour.AddMinutes(30);
                } while (hour.Hour != 17);

                return hours;
            }



            else
                foreach (Visit next in Scheduled_visits)
                {
                    if (next.date.DayOfYear.Equals(data.DayOfYear))
                    {
                        String visit = "";
                        visit = next.patient.toString2() + next.hour.ToString();
                        list.Add(visit);
                    }
                }
            do

            {
                String result = "";
                DateTime selected_time;

                int n = 0;
                do
                {
                    for (int i = 0; i < list.Count; i++)

                    {
                        hourr = hour.TimeOfDay.ToString();
                        hourr = hourr.Substring(0, hourr.Length - 3);
                        if (list[i].Contains(hourr))
                        {
                            result = "Is";
                        }
                        n++;
                    }
                } while (n < list.Count);


                if (result != "Is")
                {
                    selected_time = hour;

                    hours.Add(selected_time);
                }
                hour = hour.AddMinutes(30);
            } while (hour.Hour != 17);
            return hours;
        }
        public String visitsString()

        {

            String visit = "";
            foreach (Visit next in Scheduled_visits)
            {
                visit = next.patient.Name + next.patient.Surname;
            }
            return visit;
        }

    }
}







