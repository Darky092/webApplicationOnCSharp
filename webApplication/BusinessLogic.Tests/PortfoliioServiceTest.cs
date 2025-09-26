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
    public class PortfolioServiceTest
    {
        private readonly PortfolioService service;
        private readonly Mock<IPortfolioRepository> portfolioRepositoryMoq;
        private readonly Mock<IPortfolioValidator> portfolioValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public PortfolioServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            portfolioRepositoryMoq = new Mock<IPortfolioRepository>();
            portfolioValidatorMoq = new Mock<IPortfolioValidator>();

            repositoryWrapperMoq.Setup(x => x.portfolio)
                .Returns(portfolioRepositoryMoq.Object);

            service = new PortfolioService(repositoryWrapperMoq.Object, portfolioValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            portfolioRepositoryMoq.Verify(x => x.Create(It.IsAny<portfolio>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectPortfolios()
        {
            return new List<object []>
            {
                new object [] { new portfolio { userid = 0, achievement = "Award" } },
                new object [] { new portfolio { userid = 1, achievement = "" } },
                new object [] { new portfolio { userid = 0, achievement = "" } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectPortfolios))]
        public async Task CreateAsyncPortfolioShouldNotCreate(portfolio model)
        {
            var validationResult = new ValidationResult();

            if (model.userid <= 0)
                validationResult.Errors.Add(new ValidationFailure("userid", "User ID is required"));
            if (string.IsNullOrEmpty(model.achievement))
                validationResult.Errors.Add(new ValidationFailure("achievement", "Achievement is required"));

            portfolioValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            portfolioRepositoryMoq.Verify(x => x.Create(It.IsAny<portfolio>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewPortfolioShouldCreate()
        {
            var newPortfolio = new portfolio
            {
                userid = 5,
                achievement = "First Place",
                addedat = DateTime.Now
            };

            portfolioValidatorMoq.Setup(x => x.ValidateAsync(newPortfolio))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newPortfolio);

            portfolioRepositoryMoq.Verify(x => x.Create(It.IsAny<portfolio>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenPortfolioIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenPortfolioNotFound_ThrowsKeyNotFoundException()
        {
            portfolioRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()))
                .ReturnsAsync(new List<portfolio>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found portfolios with portfolioId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenPortfolioFound_ReturnsPortfolio()
        {
            var expected = new portfolio
            {
                userid = 42,
                achievement = "Gold Medal",
                addedat = DateTime.Now
            };

            portfolioRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()))
                .ReturnsAsync(new List<portfolio> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.userid);
            Assert.Equal("Gold Medal", result.achievement);
            portfolioRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllPortfolios()
        {
            var mockPortfolios = new List<portfolio>
            {
                new portfolio { userid = 1, achievement = "First", addedat = DateTime.Now },
                new portfolio { userid = 2, achievement = "Second", addedat = DateTime.Now }
            };

            portfolioRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockPortfolios);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("First", result [0].achievement);
            Assert.Equal("Second", result [1].achievement);
            portfolioRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenUserIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0, "Award"));
        }

        [Fact]
        public async Task Delete_WhenAchievementIsNull_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(1, null));
        }

        [Fact]
        public async Task Delete_WhenPortfolioNotFound_ThrowsKeyNotFoundException()
        {
            portfolioRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()))
                .ReturnsAsync(new List<portfolio>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999, "Award"));
            Assert.Contains("Portfolio for userId: 999 and achievement: Award not found", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultiplePortfoliosFound_ThrowsInvalidOperationException()
        {
            var portfolios = new List<portfolio>
            {
                new portfolio { userid = 5, achievement = "Award" },
                new portfolio { userid = 5, achievement = "Award" }
            };

            portfolioRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()))
                .ReturnsAsync(portfolios);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5, "Award"));
            Assert.Contains("Found more then one objects with userId: 5 and achievement: Award", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenPortfolioFound_DeletesAndSaves()
        {
            var portfolioToDelete = new portfolio
            {
                userid = 777,
                achievement = "Certificate",
                addedat = DateTime.Now
            };

            portfolioRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<portfolio, bool>>>()))
                .ReturnsAsync(new List<portfolio> { portfolioToDelete });

            await service.Delete(777, "Certificate");

            portfolioRepositoryMoq.Verify(x => x.Delete(It.IsAny<portfolio>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}