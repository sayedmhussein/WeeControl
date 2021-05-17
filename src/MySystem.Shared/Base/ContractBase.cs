using System;
using System.ComponentModel.DataAnnotations;

namespace Sayed.MySystem.Shared.Base
{
    public class ContractBase
    {
        public Guid? ParentId { get; set; }
        
        [Required]
        [StringLength(10)]
        public string ContractNo { get; set; }

        [Required]
        public int ContractType { get; set; }

        [Required]
        [StringLength(255)]
        public string ContractName { get; set; }

        public DateTime CreationTs { get; set; }

        public Guid SalesId { get; set; }

        public Guid OfficeId { get; set; }

        public DateTime? ExpireTs { get; set; }
    }
}
