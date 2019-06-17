using System.Collections.Generic;
using AdminCore.DAL.Models.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminCore.DAL.Models
{
    [Table("system_user")]
    public class SystemUser : ISoftDeletable
    {
        [Key]
        [Column("system_user_id")]
        public int SystemUserId { get; set; }
    }
}
