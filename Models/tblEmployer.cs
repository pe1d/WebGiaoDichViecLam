using System.ComponentModel.DataAnnotations;

namespace WebGiaoDichViecLam.Models
{
    public class tblEmployer
    {
        [Key]
        public int iEmployerID { get; set; }

        public int FK_iAccountID { get; set; }


        public int FK_iCompanyID { get; set; }


        public string sFullName { get; set; }


        public string sAddress { get; set; } = null!;


        public string sPhoneNumber { get; set; } = null!;


        public virtual tblCompany tblCompany { get; set; } = null!;



    }
}
