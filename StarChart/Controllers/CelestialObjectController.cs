using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (celestialObject != null)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(n => n.Name == name).ToList();

            if (celestialObjects.Any())
            {
                foreach (var celestialObject in celestialObjects)
                {
                    celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
                }
                return Ok(celestialObjects);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(celestialObjects);

        }

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {

            _context.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var existingItem = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);

            if (existingItem != null)
            {
                existingItem.Name = celestialObject.Name;
                existingItem.Id = celestialObject.Id;
                existingItem.OrbitedObjectId = celestialObject.OrbitedObjectId;
                _context.Update(existingItem);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }


        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(e => e.Id == id);
            if (celestialObject != null)
            {
                celestialObject.Name = name;
                _context.Update(celestialObject);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id).ToList();
            if (celestialObjects.Any())
            {
                _context.RemoveRange(celestialObjects);
                _context.SaveChanges();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }


    }
}