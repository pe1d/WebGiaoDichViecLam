using System.ComponentModel.DataAnnotations;

namespace WebGiaoDichViecLam.Models
{
    public class tblProfileUser
    {
        [Key]
        public int iProfileID { get; set; }

        public int FK_iAccountID { get; set; }

        public string sFullName { get; set; }

        public string sPhoneNumber { get; set; } = null!;

        public string sAddress { get; set; } = null!;

        public string sSkills {  get; set; } = null!;

        public string sExperience { get; set; } = null!;

        public string sEducation { get; set; } = null!;

        public string sCV { get; set; } = null!;

        public string sPhoto { get; set; } = null!;


    }
}
