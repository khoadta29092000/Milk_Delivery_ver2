using System;
using System.Collections.Generic;

namespace SubcriptionMilk.DTO
{
    public partial class VerificationCodeDTO
    {
        public string AccountID { get; set; } = null!;
        public string Code { get; set; } = null!;

    }
}
