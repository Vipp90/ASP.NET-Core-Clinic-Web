using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_Web.Models
{
    public class Hour
    {
        public DateTime Time { get; set; }
        public String hour { get; set; }
        public Hour()

        {


        }

        public void AddHours(double h)
        {
            Time.AddHours(h);
        }
      //  public Hour AddMinutes(double m)
       // {
       //   return  Time.AddMinutes(m);
      //  }
        public String returnHour (){
            this.hour = this.Time.Hour.ToString() + this.Time.Minute.ToString();
            
            return hour;
        }
    }

    
    
    
}
