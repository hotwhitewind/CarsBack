using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestCars.Model;

namespace TestCars.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : Controller
    {
        private readonly CarsCRUDService _carsCRUDService;

        public CarsController(CarsCRUDService carsCRUDService)
        {
            _carsCRUDService = carsCRUDService;
        }

        [HttpPost]
        [Route("createOrUpdateCar")]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrUpdateBuyer(Car buyer)
        {
            var id = await _carsCRUDService.CreateOrUpdateCar(buyer);
            return Ok(id);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _carsCRUDService.Delete(id);
            return new OkResult();
        }

        [HttpGet]
        [Route("cars")]
        [ProducesResponseType(typeof(List<Car>), (int)HttpStatusCode.OK)]
        public IActionResult Cars()
        {
            var ret = _carsCRUDService.GetAllCars();
            return Ok(ret);
        }

        [HttpGet]
        [Route("car")]
        [ProducesResponseType(typeof(Car), (int)HttpStatusCode.OK)]
        public IActionResult Car(Guid id)
        {
            var ret = _carsCRUDService.GetCar(id);
            return Ok(ret);
        }
    }
}
