using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblSlotShip
    {
        public TblSlotShip()
        {
            TblOrderDetails = new HashSet<TblOrderDetail>();
        }

        public string Id { get; set; } = null!;
        public string SlotTitle { get; set; } = null!;
        public string? SlotDescription { get; set; }

        public virtual ICollection<TblOrderDetail> TblOrderDetails { get; set; }
    }
}
