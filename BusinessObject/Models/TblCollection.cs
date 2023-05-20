using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblCollection
    {
        public int Id { get; set; }
        public string? OrderDetailId { get; set; }
        public string ProductId { get; set; } = null!;

        public virtual TblOrderDetail? OrderDetail { get; set; }
    }
}
