using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strona.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the Konto_Pacjenta class
    public class Patient_account : IdentityUser
    {

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string Surname { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string Pesel { get; set; }

        
    }
}
