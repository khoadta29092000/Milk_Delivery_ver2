using BusinessObject.Models;
using DataAccess.DTO.SubscriptionDay;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SubcriptionMilk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionDayController : ControllerBase
    {
        private readonly IRepositorySubscriptionDay repositorySubscriptionDay;
        private readonly IConfiguration configuration;
        public SubscriptionDayController(IRepositorySubscriptionDay _repositorySubscriptionDay, IConfiguration configuration)
        {
            repositorySubscriptionDay = _repositorySubscriptionDay;
            this.configuration = configuration;
        }
        [HttpGet]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAll(int page, int pageSize)
        {

            try
            {
                var SubscriptionDayList = await repositorySubscriptionDay.SearchByName(page, pageSize);
                var Count = SubscriptionDayList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = SubscriptionDayList, Count });
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
                var Result = await repositorySubscriptionDay.GetSubscriptionDayById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(PostSubscriptionDayDTO SubscriptionDay)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string formatDate = currentDate.ToString("ddMMyyyy");
                var SubscriptionDayList = await repositorySubscriptionDay.GetSubscriptionDay();
                int count = SubscriptionDayList.Count(item => item.Id.StartsWith("SCD" + currentDate.ToString("ddMMyyyy")));
                var id = "SCD" + formatDate + (count + 1);
                var newStaion = new TblSubscriptionDay
                {
                    Id = id,
                    Description = SubscriptionDay.Description,
                    DiscountPercentage = SubscriptionDay.DiscountPercentage,
                    Title = SubscriptionDay.Title
                };
                await repositorySubscriptionDay.AddTblSubscriptionDay(newStaion);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> update(string id, PutSubscriptionDayDTO SubscriptionDay)
        {
            if (id != SubscriptionDay.Id)
            {
                return BadRequest();
            }
            try
            {
                var oldSubscriptionDay = await repositorySubscriptionDay.GetSubscriptionDayById(id);
                var newSubscriptionDay = new TblSubscriptionDay
                {
                    Id = id,
                    Description = SubscriptionDay.Description,
                    DiscountPercentage = SubscriptionDay.DiscountPercentage,
                    Title = SubscriptionDay.Title
                };
                await repositorySubscriptionDay.UpdateTblSubscriptionDay(newSubscriptionDay);
                return Ok(new { StatusCode = 200, Message = "Update successful" });
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
                await repositorySubscriptionDay.DeleteTblSubscriptionDay(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
