using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebGiaoDichViecLam.Models
{
    public class tblJob
    {
        [Key]
        public int iJobID { get; set; }

        public int iCompanyID { get; set; }


        public int iCategoryID { get; set; }


        public string sNameJob {  get; set; }


        public float fSalaryJob { get; set; }


        public string sTypeJob { get; set; }


        public DateTime dPostedDate { get; set; }


        public DateTime dExpiryDate { get; set; }


        public string sDescriptionJob { get; set; }



        public virtual tblCategory TblCategory { get; set; } = null!;

        public virtual tblCompany TblCompany { get; set; } = null!;

    }
}
