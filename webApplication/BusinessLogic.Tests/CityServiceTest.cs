using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using FluentValidation.Results;
using Moq;
using Validators.Interefaces;

namespace BusinessLogic.Tests
{
    public class CityServiceTest
    {
        private readonly CityService service;
        private readonly Mock<ICityRepository> cityRepositoryMoq;
        private readonly Mock<ICityValidator> cityValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public CityServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            cityRepositoryMoq = new Mock<ICityRepository>();
            cityValidatorMoq = new Mock<ICityValidator>();

            repositoryWrapperMoq.Setup(x => x.city)
                .Returns(cityRepositoryMoq.Object);

            service = new CityService(repositoryWrapperMoq.Object, cityValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            cityRepositoryMoq.Verify(x => x.Create(It.IsAny<city>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectCities()
        {
            return new List<object []>
            {
                new object [] { new city { cityname = "" } },
                new object [] { new city { cityname = null } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectCities))]
        public async Task CreateAsyncCityShouldNotCreate(city model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.cityname))
                validationResult.Errors.Add(new ValidationFailure("cityname", "City name is required"));

            cityValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            cityRepositoryMoq.Verify(x => x.Create(It.IsAny<city>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewCityShouldCreate()
        {
            var newCity = new city
            {
                cityname = "Moscow",
                postalcode = "101000",
                country = "Russia"
            };

            cityValidatorMoq.Setup(x => x.ValidateAsync(newCity))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newCity);

            cityRepositoryMoq.Verify(x => x.Create(It.IsAny<city>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            cityRepositoryMoq.Verify(x => x.Update(It.IsAny<city>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectCities))]
        public async Task UpdateAsyncCityShouldNotUpdate(city model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.cityname))
                validationResult.Errors.Add(new ValidationFailure("cityname", "City name is required"));

            cityValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            cityRepositoryMoq.Verify(x => x.Update(It.IsAny<city>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewCityShouldUpdate()
        {
            var updateCity = new city
            {
                cityid = 10,
                cityname = "Updated Moscow",
                postalcode = "101001",
                country = "Russian Federation"
            };

            var existingCity = new city
            {
                cityid = 10,
                cityname = "Moscow",
                postalcode = "101000",
                country = "Russia"
            };

            cityRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city> { existingCity });

            cityValidatorMoq.Setup(x => x.ValidateAsync(updateCity))
                .ReturnsAsync(new ValidationResult());

            await service.Update(updateCity);

            cityRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<city, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            cityValidatorMoq.Verify(x => x.ValidateAsync(updateCity), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenCityIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenCityNotFound_ThrowsKeyNotFoundException()
        {
            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found cities with id 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenCityFound_ReturnsCity()
        {
            var expected = new city
            {
                cityid = 42,
                cityname = "Berlin",
                postalcode = "10115",
                country = "Germany"
            };

            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.cityid);
            Assert.Equal("Berlin", result.cityname);
            cityRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllCities()
        {
            var mockCities = new List<city>
            {
                new city { cityid = 1, cityname = "Moscow", postalcode = "101000" },
                new city { cityid = 2, cityname = "London", postalcode = "SW1A 1AA" }
            };

            cityRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockCities);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("Moscow", result [0].cityname);
            Assert.Equal("London", result [1].cityname);
            cityRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenCityIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenCityNotFound_ThrowsKeyNotFoundException()
        {
            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found cities with id 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleCitiesFound_ThrowsInvalidOperationException()
        {
            var cities = new List<city>
            {
                new city { cityid = 5, cityname = "Test" },
                new city { cityid = 5, cityname = "Test2" }
            };

            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(cities);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("found more then one city", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenCityFound_DeletesAndSaves()
        {
            var cityToDelete = new city
            {
                cityid = 777,
                cityname = "ToDelete",
                postalcode = "00000"
            };

            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city> { cityToDelete });

            await service.Delete(777);

            cityRepositoryMoq.Verify(x => x.Delete(It.IsAny<city>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}