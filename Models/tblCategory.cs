using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebGiaoDichViecLam.Models
{
    public class tblCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int iCategoryID { get; set; }

        [Required]
        public string sCategoryName { get; set; }
    }
}
