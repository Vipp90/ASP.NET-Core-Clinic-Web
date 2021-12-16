using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Clinic_Web.Models.Models
{
    public class Visit
    {
        
        
        public long VisitId { get; set; }
        [DisplayName("Pacjent")]
        public virtual long PatientId { get; set; }
        [DisplayName("Pacjent")]
        public virtual Patient patient { get; set; }
        [DisplayName("Lekarz")]
        public virtual long DoctorId { get; set; }
        [DisplayName("Lekarz")]
        public virtual Doctor doctor { get; set; }
        [DisplayName("Data")]
        [Required]
        
        [DataType(DataType.Date)]
        public DateTime date { get; set; }

        [DisplayName("Godzina")]
        [Required]
        public string hour { get; set; }
        public Visit()

        {


        }
        //  public List<String> get_free_hours() {
        //   return   this.doctor.get_free_visit_day(this.date);

        //   }

        /*
        public override bool Equals(Object ob)
        {

            Patient patient = this.patient;
            Patient patient2 = ((Visit)ob).patient;
            Doctor doctor = this.doctor;
            Doctor doctor2 = ((Visit)ob).doctor;
            DateTime date = this.date;
            DateTime date2 = ((Visit)ob).date;

            bool a = patient == (patient2);
            bool b = doctor == (doctor2);
            bool c = date.Equals(date2);
            bool d = false;

            if (a && b && c == true)
            {
                d = true;
            }


            return d;
        }
        */
    }
}

