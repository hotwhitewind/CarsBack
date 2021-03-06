using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCars.Abstraction;
using TestCars.Domain;
using TestCars.Model;

namespace TestCars
{
    public class CarsCRUDService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CarsCRUDService> _logger;

        public CarsCRUDService(IUnitOfWork unitOfWork, ILogger<CarsCRUDService> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateOrUpdateCar(Car carEdt)
        {
            _logger.LogDebug("CarsCRUDService.CreateOrUpdateCar: {Id}", carEdt.Id);
            Car car;
            if(carEdt.Id == Guid.Empty)
            {
                car = new Car(carEdt.Brand, carEdt.Model, carEdt.Color, carEdt.Equipment, carEdt.Price);
                await _unitOfWork.AddAsync(car);
            }
            else
            {
                var existingCar = _unitOfWork.Query<Car>().SingleOrDefault(x => x.Id == carEdt.Id);
                car = existingCar ?? throw new DomainException($"Trying to update not existing car: {carEdt.Id}");                
            }

            CarFill(carEdt, car);
            await _unitOfWork.CommitAsync();

            return car.Id;
        }

        public async Task Delete(Guid carGuidId)
        {
            var existingCar = _unitOfWork.Query<Car>().SingleOrDefault(x => x.Id == carGuidId);
            if(existingCar == null)
                throw new DomainException($"Trying to delete not existing car: {carGuidId}");
            _unitOfWork.Remove(existingCar);
            await _unitOfWork.CommitAsync();
        }

        public Car GetCar(Guid carGuidId)
        {
            var existingCar = _unitOfWork.Query<Car>().SingleOrDefault(x => x.Id == carGuidId);
            if (existingCar == null)
                throw new DomainException($"Trying to get not existing car: {carGuidId}");
            return existingCar;
        }

        public List<Car> GetAllCars()
        {
            return _unitOfWork.Query<Car>().ToList();
        }

        public void CarFill(Car editCar, Car model)
        {
            if (!String.IsNullOrEmpty(editCar.Brand))
                model.Brand = editCar.Brand;
            if (!String.IsNullOrEmpty(editCar.Color))
                model.Color = editCar.Color;
            if (!String.IsNullOrEmpty(editCar.Equipment))
                model.Equipment = editCar.Equipment;
            if (!String.IsNullOrEmpty(editCar.Model))
                model.Model = editCar.Model;
            if (editCar.Price != null)
            {
                if (editCar.Price < 0)
                    model.Price = 0;
                else
                    model.Price = editCar.Price;
            }
        }
    }
}
