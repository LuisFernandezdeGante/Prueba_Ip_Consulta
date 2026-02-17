using Ip_Api.Data;
using Ip_Api.Models;
using Ip_Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ip_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IpController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IpService _service;

        public IpController(AppDbContext context, IpService service)
        {
            _context = context;
            _service = service;
        }

        

        [HttpGet("{ip}")]
        public async Task<IActionResult> ConsultIp(string ip)
        {
            var exists = await _context.IpRecords
                .FirstOrDefaultAsync(x => x.Ip == ip);



            if (exists != null)
                
                return Conflict(new { message = "La IP ya está registrada." });

            var json = await _service.GetIpInfo(ip);
            var root = json.RootElement;

            //return Ok(json);


            var record = new IpRecord
            {
                Ip = ip,
                Company = root.GetProperty("company").GetProperty("name").GetString(),
                Country = root.GetProperty("location").GetProperty("country").GetProperty("name").GetString(),
                City = root.GetProperty("location").GetProperty("city").GetString(),
                Latitude = root.GetProperty("location").GetProperty("latitude").GetDouble(),
                Longitude = root.GetProperty("location").GetProperty("longitude").GetDouble(),
                Lenguaje = root.GetProperty("location").GetProperty("language").GetProperty("name").GetString(),
                TimeZone = root.GetProperty("time_zone").GetProperty("name").GetString()
            };

            _context.IpRecords.Add(record);
            await _context.SaveChangesAsync();

            return Ok(record);
        }

        [HttpGet]
       
        public async Task<IActionResult> getall()
        {
            return Ok(await _context.IpRecords.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.IpRecords.FindAsync(id);
            if (record == null) return NotFound();

            _context.IpRecords.Remove(record);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetHistory()
        {
            var records = await _context.IpRecords
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(records);
        }


    }
}