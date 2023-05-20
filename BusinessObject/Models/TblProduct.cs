using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblProduct
    {
        public string Id { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Type { get; set; } = null!;
        public string? Image { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual TblCategory Category { get; set; } = null!;
    }
}
