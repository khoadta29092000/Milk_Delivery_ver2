using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO.DiscountCode
{
    public partial class PostDiscountOfAccountDTO
    {
        public string DiscountCodeId { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
