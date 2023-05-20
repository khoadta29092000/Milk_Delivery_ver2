using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblStatusByOrder
    {
        public TblStatusByOrder()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
        }

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
