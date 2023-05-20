using System;
using System.Collections.Generic;

namespace BusinessObject.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblPackageSettings = new HashSet<TblPackageSetting>();
            TblProducts = new HashSet<TblProduct>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool? IsDeleted { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<TblPackageSetting> TblPackageSettings { get; set; }
        public virtual ICollection<TblProduct> TblProducts { get; set; }
    }
}
