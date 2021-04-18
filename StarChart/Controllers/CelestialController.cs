using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialController(ApplicationDbContext context)
        {
            _context = context;
        }        
    }
}