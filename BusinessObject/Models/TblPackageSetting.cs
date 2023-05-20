using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblPackageSetting
    {
        public string PackageId { get; set; } = null!;
        public string CategoryId { get; set; } = null!;
        public int Quantity { get; set; }

        public virtual TblCategory Category { get; set; } = null!;
        public virtual TblPackage Package { get; set; } = null!;
    }
}
