using FluentValidation;
using rp_challenge.Application.DTOs;
using rp_challenge.Domain.Entities;
using rp_challenge.Domain.Exception;
using rp_challenge.Domain.Repositories;

namespace rp_challenge.Application.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;
        private readonly IValidator<CreateCarDTO> _createValidator;
        private readonly IValidator<UpdateCarDTO> _updateValidator;

        public CarService(
            ICarRepository carRepository,
            IValidator<CreateCarDTO> createValidator,
            IValidator<UpdateCarDTO> updateValidator)
        {
            _carRepository = carRepository;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<CarDTO?> GetByIdAsync(int id)
        {
            var car = await _carRepository.GetByIdAsync(id);
            return car == null ? null : MapToDto(car);
        }

        public async Task<IEnumerable<CarDTO>> GetAllAsync()
        {
            var cars = await _carRepository.GetAllAsync();
            return cars.Select(MapToDto);
        }

        public async Task<CarDTO> CreateAsync(CreateCarDTO createCarDTO)
        {
            var validationResult = await _createValidator.ValidateAsync(createCarDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Check if Model already exists
            var existingCar = await _carRepository.GetByModelAsync(createCarDTO.Model);
            if (existingCar != null)
            {
                throw new CarAlreadyExistsException(createCarDTO.Model);
            }

            var car = Car.Create(
                createCarDTO.Brand,
                createCarDTO.Model,
                createCarDTO.Price);

            var carId = await _carRepository.CreateAsync(car);
            var createdCar = await _carRepository.GetByIdAsync(carId);

            return MapToDto(createdCar!);
        }

        public async Task<CarDTO> UpdateAsync(int id, UpdateCarDTO updateCarDTO)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateCarDTO);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var car = await _carRepository.GetByIdAsync(id);
            if (car == null)
            {
                throw new CarNotFoundException(id);
            }

            // Check if Model is being changed and if new Model already exists
            if (car.Model != updateCarDTO.Model)
            {
                var existingCar = await _carRepository.GetByModelAsync(updateCarDTO.Model);
                if (existingCar != null)
                {
                    throw new CarAlreadyExistsException(updateCarDTO.Model);
                }
            }

            car.Update(
                updateCarDTO.Brand,
                updateCarDTO.Model,
                updateCarDTO.Price);

            await _carRepository.UpdateAsync(car);

            var updatedCar = await _carRepository.GetByIdAsync(id);
            return MapToDto(updatedCar!);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _carRepository.ExistsAsync(id);
            if (!exists)
            {
                throw new CarNotFoundException(id);
            }

            return await _carRepository.DeleteAsync(id);
        }

        private static CarDTO MapToDto(Car car)
        {
            return new CarDTO
            {
                Id = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Price = car.Price,
                Created = car.Created,
                Updated = car.Updated
            };
        }
    }
}
