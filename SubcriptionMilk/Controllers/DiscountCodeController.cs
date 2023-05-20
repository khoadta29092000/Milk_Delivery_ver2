using BusinessObject.Models;
using DataAccess.DTO.DiscountCode;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SubcriptionMilk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountCodeController : ControllerBase
    {
        private readonly IRepositoryAccount repositoryAccount;
        private readonly IRepositoryDiscountCode repositoryDiscountCode;
        private readonly IConfiguration configuration;
        public DiscountCodeController(IRepositoryAccount _repositoryAccount, IRepositoryDiscountCode _repositoryDiscountCode, IConfiguration configuration)
        {
            repositoryAccount = _repositoryAccount;
            this.configuration = configuration;
            repositoryDiscountCode = _repositoryDiscountCode;
        }

        private string GenerateRandomCode(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(chars.Length);
                stringBuilder.Append(chars[randomIndex]);
            }

            return stringBuilder.ToString();
        }

        [HttpGet]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAll(int minPoint, int maxPoint, bool? isDeleted, int page, int pageSize)
        {

            try
            {
                var DiscountCodeList = await repositoryDiscountCode.SearchByName(minPoint, maxPoint, isDeleted, page, pageSize);
                var Count = DiscountCodeList.Count();
                if (minPoint != 0 || maxPoint != 0)
                {
                    if (maxPoint < minPoint)
                    {
                        return StatusCode(409, new { StatusCode = 409, Message = "maxPoint must be greater than minPoint" });
                    }
                    else
                    {
                        return Ok(new { StatusCode = 200, Message = "Load successful", data = DiscountCodeList, Count });
                    }
                }
              
                return Ok(new { StatusCode = 200, Message = "Load successful", data = DiscountCodeList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("GetDiscountCodeOfAccount")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetDiscountCodeOfAccount(int page, int pageSize)
        {

            try
            {
                var DiscountCodeOfAccountList = await repositoryDiscountCode.GetDiscountCodeOfAccount(null, null, page, pageSize);
                var Count = DiscountCodeOfAccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = DiscountCodeOfAccountList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("account/{accountId}/DiscountCode/{discountCodeId}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllDiscountCodeOfAccount(string? accountId, string? discountCodeId)
        {

            try
            {
                var DiscountCodeOfAccountList = await repositoryDiscountCode.GetDiscountCodeOfAccount(accountId, discountCodeId, 0, 0);
                var Count = DiscountCodeOfAccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = DiscountCodeOfAccountList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpGet("AccountBuyCode")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllAccountBuyCode( string discountCodeId, int page, int pageSize)
        {

            try
            {
                var AccountBuyCodeList = await repositoryDiscountCode.GetAllAccountBuyDiscountCode(discountCodeId, page, pageSize);
                var Count = AccountBuyCodeList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = AccountBuyCodeList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("CodeOfAccount")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAllCodeofAccount(string accountId, int page, int pageSize)
        {

            try
            {
                var CodeOfAccountList = await repositoryDiscountCode.GetAllDiscountCodeBuyAccount(accountId, page, pageSize);
                var Count = CodeOfAccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = CodeOfAccountList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("GetOrderByDiscountCode")]
        public async Task<ActionResult> GetOrderByDiscountCodeId(string DiscountCodeId, int page, int pageSize)
        {
            try
            {
                var OrderList = await repositoryDiscountCode.GetOrdersByDiscountCode(DiscountCodeId, page, pageSize);
                var Count = OrderList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = OrderList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                var Result = await repositoryDiscountCode.GetDiscountCodeById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(PostDiscountCodeDTO DiscountCode)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string formatDate = currentDate.ToString("ddMMyyyy");
                var DiscountCodeList = await repositoryDiscountCode.GetDiscountCode();
                int count = DiscountCodeList.Count(item => item.Id.StartsWith("DCC" + currentDate.ToString("ddMMyyyy")));
                var id = "DCC" + formatDate + (count + 1);
                var newStaion = new TblDiscountCode
                {
                    Id = id,
                    IsDeleted = false,
                    Points = DiscountCode.Points,
                    CodeDescription = DiscountCode.CodeDescription,
                    DiscountPercentage = DiscountCode.DiscountPercentage,
                };
                await repositoryDiscountCode.AddTblDiscountCode(newStaion);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost("DiscountOfAccount")]
        [Authorize]
        public async Task<IActionResult> CreateByCusomer(PostDiscountOfAccountDTO DiscountCode)
        {
            try
            {
                if (DiscountCode.Quantity == 0)
                {
                    return Ok(new { StatusCode = 409, Message = "Quantity must be greater than 0" });
                }
                string userId = User.FindFirst("Id")?.Value;
                DateTime beginTime = DateTime.Now;
                TblAccount account = await repositoryAccount.GetProfile(userId);
                TblDiscountCode discountCodeById = await repositoryDiscountCode.GetDiscountCodeById(DiscountCode.DiscountCodeId);
                int enoughBuy = account.TblMemberPoint.MemberPoints - (DiscountCode.Quantity * discountCodeById.Points);
                if (enoughBuy < 0)
                {
                    return Ok(new { StatusCode = 409, Message = "Not enought Points" });
                }
                var newDiscountOfAccount = new TblDiscountOfAccount
                {
                    AccountId = userId,
                    DiscountCodeId = discountCodeById.Id,
                    BeginTime = beginTime,
                    EndTime = beginTime.AddMonths(1),
                    Code = GenerateRandomCode(8),
                    Quantity = DiscountCode.Quantity,
                };
                await repositoryDiscountCode.AddTblDiscountCodeOfAccount(newDiscountOfAccount);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> update(string id, PutDiscountCodeDTO DiscountCode)
        {
            if (id != DiscountCode.Id)
            {
                return BadRequest();
            }
            try
            {
                var oldDiscountCode = await repositoryDiscountCode.GetDiscountCodeById(id);
                var newDiscountCode = new TblDiscountCode
                {
                    IsDeleted = false,
                    Id = id,
                    CodeDescription = DiscountCode.CodeDescription,
                    DiscountPercentage = DiscountCode.DiscountPercentage,
                    Points = DiscountCode.Points
                };
                await repositoryDiscountCode.UpdateTblDiscountCode(newDiscountCode);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("UpdateActive")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> UpdateActive(string id)
        {
            try
            {
                TblDiscountCode DiscountCode = await repositoryDiscountCode.GetDiscountCodeById(id);
                if (DiscountCode == null)
                {
                    return Ok(new { StatusCode = 400, Message = "Id not Exists" });
                }
                else
                {
                    await repositoryDiscountCode.UpdateActive(id, DiscountCode.IsDeleted);
                    return Ok(new { StatusCode = 200, Message = "Update Active successful" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await repositoryDiscountCode.DeleteTblDiscountCode(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
