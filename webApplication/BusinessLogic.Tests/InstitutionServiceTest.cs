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
    public class InstitutionServiceTest
    {
        private readonly InstitutionService service;
        private readonly Mock<IInstitutionRepository> institutionRepositoryMoq;
        private readonly Mock<IInstitutionValidator> institutionValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly Mock<ICityRepository> cityRepositoryMoq;

        public InstitutionServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            institutionRepositoryMoq = new Mock<IInstitutionRepository>();
            institutionValidatorMoq = new Mock<IInstitutionValidator>();
            cityRepositoryMoq = new Mock<ICityRepository>();

            repositoryWrapperMoq.Setup(x => x.institution).Returns(institutionRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.city).Returns(cityRepositoryMoq.Object);

            service = new InstitutionService(repositoryWrapperMoq.Object, institutionValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            institutionRepositoryMoq.Verify(x => x.Create(It.IsAny<institution>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectInstitutions()
        {
            return new List<object []>
            {
                new object [] { new institution { institutionname = "", street = "Main St" } },
                new object [] { new institution { institutionname = "School", street = "" } },
                new object [] { new institution { institutionname = "", street = "" } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectInstitutions))]
        public async Task CreateAsyncInstitutionShouldNotCreate(institution model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.institutionname))
                validationResult.Errors.Add(new ValidationFailure("institutionname", "Institution name is required"));
            if (string.IsNullOrEmpty(model.street))
                validationResult.Errors.Add(new ValidationFailure("street", "Street is required"));

            institutionValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            institutionRepositoryMoq.Verify(x => x.Create(It.IsAny<institution>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewInstitutionShouldCreate()
        {
            var newInstitution = new institution
            {
                institutionname = "Tech School",
                street = "123 Main St",
                phone = "555-1234",
                website = "https://techschool.com",
                cityid = 5
            };

            institutionValidatorMoq.Setup(x => x.ValidateAsync(newInstitution))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newInstitution);

            institutionRepositoryMoq.Verify(x => x.Create(It.IsAny<institution>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            institutionRepositoryMoq.Verify(x => x.Update(It.IsAny<institution>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectInstitutions))]
        public async Task UpdateAsyncInstitutionShouldNotUpdate(institution model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.institutionname))
                validationResult.Errors.Add(new ValidationFailure("institutionname", "Institution name is required"));
            if (string.IsNullOrEmpty(model.street))
                validationResult.Errors.Add(new ValidationFailure("street", "Street is required"));

            institutionValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            institutionRepositoryMoq.Verify(x => x.Update(It.IsAny<institution>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewInstitutionShouldUpdate()
        {
            var updateInstitution = new institution
            {
                institutionid = 10,
                institutionname = "Updated School",
                street = "456 New St",
                phone = "555-5678",
                website = "https://updatedschool.com",
                cityid = 8
            };

            var existingInstitution = new institution
            {
                institutionid = 10,
                institutionname = "Old School",
                street = "789 Old St",
                phone = "555-0000",
                website = "https://oldschool.com",
                cityid = 5
            };

            institutionRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution> { existingInstitution });

            institutionValidatorMoq.Setup(x => x.ValidateAsync(updateInstitution))
                .ReturnsAsync(new ValidationResult());

            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city> { new city { cityid = 8 } });

            await service.Update(updateInstitution);

            institutionRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<institution, bool>>>()), Times.Once);
            cityRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            institutionValidatorMoq.Verify(x => x.ValidateAsync(updateInstitution), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncInstitution_WhenCityNotFound_ThrowsKeyNotFoundException()
        {
            var updateInstitution = new institution
            {
                institutionid = 10,
                institutionname = "Updated School",
                street = "456 New St",
                cityid = 999
            };

            var existingInstitution = new institution
            {
                institutionid = 10,
                institutionname = "Old School",
                street = "789 Old St"
            };

            institutionRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution> { existingInstitution });

            institutionValidatorMoq.Setup(x => x.ValidateAsync(updateInstitution))
                .ReturnsAsync(new ValidationResult());

            cityRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<city, bool>>>()))
                .ReturnsAsync(new List<city>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateInstitution));
            Assert.Contains("Did not found city with cityId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenInstitutionIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenInstitutionNotFound_ThrowsKeyNotFoundException()
        {
            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found institution with institutionId: 999 ", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenInstitutionFound_ReturnsInstitution()
        {
            var expected = new institution
            {
                institutionid = 42,
                institutionname = "University",
                street = "Campus Ave",
                cityid = 10
            };

            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.institutionid);
            Assert.Equal("University", result.institutionname);
            institutionRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllInstitutions()
        {
            var mockInstitutions = new List<institution>
            {
                new institution { institutionid = 1, institutionname = "School 1", street = "St 1" },
                new institution { institutionid = 2, institutionname = "School 2", street = "St 2" }
            };

            institutionRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockInstitutions);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("School 1", result [0].institutionname);
            Assert.Equal("School 2", result [1].institutionname);
            institutionRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenInstitutionIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenInstitutionNotFound_ThrowsKeyNotFoundException()
        {
            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found institution with institutionId: 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleInstitutionsFound_ThrowsInvalidOperationException()
        {
            var institutions = new List<institution>
            {
                new institution { institutionid = 5, institutionname = "Test" },
                new institution { institutionid = 5, institutionname = "Test2" }
            };

            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(institutions);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("Found more then one institution", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenInstitutionFound_DeletesAndSaves()
        {
            var institutionToDelete = new institution
            {
                institutionid = 777,
                institutionname = "ToDelete",
                street = "Nowhere"
            };

            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution> { institutionToDelete });

            await service.Delete(777);

            institutionRepositoryMoq.Verify(x => x.Delete(It.IsAny<institution>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}