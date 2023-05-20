using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblOrderDetail
    {
        public TblOrderDetail()
        {
            TblCollections = new HashSet<TblCollection>();
            TblOrders = new HashSet<TblOrder>();
        }

        public string Id { get; set; } = null!;
        public string? SlotShipId { get; set; }
        public DateTime DateOrder { get; set; }
        public int StatusId { get; set; }

        public virtual TblSlotShip? SlotShip { get; set; }
        public virtual TblStatusByOrder Status { get; set; } = null!;
        public virtual ICollection<TblCollection> TblCollections { get; set; }
        public virtual ICollection<TblOrder> TblOrders { get; set; }
    }
}
