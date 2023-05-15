using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SubcriptionMilk.DTO
{
    public partial class ChangePasswordDTO
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
