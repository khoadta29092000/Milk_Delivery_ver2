using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblAccounts = new HashSet<TblAccount>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<TblAccount> TblAccounts { get; set; }
    }
}
