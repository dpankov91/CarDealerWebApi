using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarShop.Core.ApplicationService.Services;
using CarShop.Core.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarShopWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }


        // GET: api/<CarController>
        [HttpGet]
        public ActionResult<IEnumerable<Car>> Get()
        {
            try
            {
                if (_carService.GetAllCars() != null)
                {
                    return Ok(_carService.GetAllCars());
                }
                return NotFound();
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Error when trying to get a list of cars");
            }
        }

        // GET api/<CarController>/5
        [HttpGet("{id}")]
        public ActionResult<Car> Get(int id)
        {
            try
            {
                if (id < 1 && id.Equals(null))
                {
                    return StatusCode(500, "ID should be 1 or more");
                }
                else if (_carService.GetCarById(id) == null)
                {
                    return NotFound();
                }
                return _carService.GetCarById(id);
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Error when trying to get a car by ID");
            }

        }

        // POST api/<CarController>
        [HttpPost]
        public ActionResult<Car>Post([FromBody] Car car)
        {
            try
            {
                _carService.Create(car);
                return StatusCode(201, "Yes Sir! Car is created.");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Error when trying to create a car");
            }
        }

        // PUT api/<CarController>/5
        [HttpPut("{id}")]
        public ActionResult<Car> Put(int id, [FromBody] Car car)
        {
            try
            {
                if(car.Id != id || id < 1)
                {
                    return BadRequest("ID error! Please check ID");
                }
                _carService.Update(car);
                return StatusCode(200, "Yeah, car is updated");
            }
            catch (System.Exception)
            {
                return StatusCode(500, "Error when trying to update car");  
            }
        }

        // DELETE api/<CarController>/5
        [HttpDelete("{id}")]
        public ActionResult<Car> Delete(int id)
        {
            try
            {
                if(id < 1)
                {
                    return BadRequest("ID Error! Please check ID");
                }
                var carToDelete = _carService.Delete(id);
                if (carToDelete == null)
                {
                    return NotFound();
                }
                return Accepted(carToDelete);
            }
            catch(System.Exception)
            {
                return StatusCode(500, "Error when deleting car");
            }
        }
    }
}
