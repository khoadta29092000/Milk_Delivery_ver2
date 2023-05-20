using BusinessObject.Models;
using DataAccess.DTO.Station;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SubcriptionMilk.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IRepositoryAccount repositoryAccount;
        private readonly IRepositoryStation repositoryStation;
        private readonly IConfiguration configuration;
        public StationController(IRepositoryAccount _repositoryAccount, IRepositoryStation _repositoryStation, IConfiguration configuration)
        {
            repositoryAccount = _repositoryAccount;
            this.configuration = configuration;
            repositoryStation = _repositoryStation;
        }
        [HttpGet]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> GetAll(string? search, bool? isDeleted, int page, int pageSize)
        {

            try
            {
                var StationList = await repositoryStation.SearchByName(search, isDeleted, page, pageSize);
                var Count = StationList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = StationList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("GetAccountByStation")]
        public async Task<ActionResult> GetAccountByStationId(string stationId, int page, int pageSize)
        {
            try
            {
                var AccountList = await repositoryStation.GetAccountsByStation(stationId, page, pageSize);
                var Count = AccountList.Count();
                return Ok(new { StatusCode = 200, Message = "Load successful", data = AccountList, Count });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpGet("GetOrderByStation")]
        public async Task<ActionResult> GetOrderByStationId(string stationId, int page, int pageSize)
        {
            try
            {
                var OrderList = await repositoryStation.GetOrdersByStation(stationId, page, pageSize);
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
                var Result = await repositoryStation.GetStationById(id);
                return Ok(new { StatusCode = 200, Message = "Load successful", data = Result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPost]
        //[Authorize(Roles = "1")]
        public async Task<IActionResult> Create(PostStationDTO station)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                string formatDate = currentDate.ToString("ddMMyyyy");
                var StationList = await repositoryStation.GetStation();
                int count = StationList.Count(item => item.Id.StartsWith("STA" + currentDate.ToString("ddMMyyyy")));
                var id = "STA" + formatDate + (count + 1);
                var newStaion = new TblStation
                {
                  Id = id,
                  IsDeleted = false,
                  StationAddress = station.StationAddress,
                  StationName = station.StationName,
                  StationDescription = station.StationDescription,  
                };        
                await repositoryStation.AddTblStation(newStaion);
                return Ok(new { StatusCode = 200, Message = "Add successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }
        }
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> update(string id, PutStationDTO station)
        {
            if (id != station.Id)
            {
                return BadRequest();
            }
            try
            {
                var oldStation = await repositoryStation.GetStationById(id);
                var newStation = new TblStation
                {
                    IsDeleted = false,
                    Id = id,
                    StationAddress = station.StationAddress,
                    StationDescription = station.StationDescription,
                    StationName = station.StationName,
                };
                await repositoryStation.UpdateTblStation(newStation);
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
                TblStation station = await repositoryStation.GetStationById(id);
                if (station == null)
                {
                    return Ok(new { StatusCode = 400, Message = "Id not Exists" });
                }
                else
                {
                    await repositoryStation.UpdateActive(id, station.IsDeleted);
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
                await repositoryStation.DeleteTblStation(id);
                return Ok(new { StatusCode = 200, Message = "Delete successful" });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, Message = ex.Message });
            }


        }
    }
}
