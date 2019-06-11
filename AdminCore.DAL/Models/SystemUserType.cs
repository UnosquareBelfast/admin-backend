using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("system_user_type")]
    public class SystemUserType
    {
        [Key]
        [Column("system_user_type_id")]
        public int SystemUserTypeId { get; set; }

        [Column("description")]
        public string Description { get; set; }
    }
}
