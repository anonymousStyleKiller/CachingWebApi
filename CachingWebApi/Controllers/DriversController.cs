using CachingWebApi.Data;
using CachingWebApi.Interfaces;
using CachingWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CachingWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ICacheService _cacheService;
    private readonly AppDbContext _context;

    public DriversController(ILogger<WeatherForecastController> logger, ICacheService cacheService, AppDbContext context)
    {
        _logger = logger;
        _cacheService = cacheService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
       var cacheData =  _cacheService.GetData<Driver>("drivers");
       if (cacheData != null && cacheData.Any()) return Ok(cacheData);

       cacheData = await _context.Drivers.ToListAsync();
       var expiryTime = DateTimeOffset.Now.AddSeconds(30);
       _cacheService.SetData<IEnumerable<Driver>>("drivers", cacheData, expiryTime);
       return Ok(cacheData);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Driver value)
    {
        var addedObj = await _context.Drivers.AddAsync(value);
        var expiryTime = DateTimeOffset.Now.AddSeconds(30);
        _cacheService.SetData<Driver>($"driver{value.Id}", addedObj.Entity, expiryTime);
        await _context.SaveChangesAsync();
        return Ok(addedObj.Entity);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var exist = await _context.Drivers.FirstOrDefaultAsync(x => x.Id == id);
        if (exist == null) return NotFound();
        _context.Remove(exist);
        _cacheService.RemoveData($"driver{id}");
        await _context.SaveChangesAsync();
        return NoContent();
    }

}