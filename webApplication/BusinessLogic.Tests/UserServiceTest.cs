using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;

namespace BusinessLogic.Tests
{
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        public UserServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();

            repositoryWrapperMoq.Setup(x => x.user)
                .Returns(userRepositoryMoq.Object);
            service = new UserService(repositoryWrapperMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullUSerShouldTrowNullArgumentExeption()
        {
            //act
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));

            //assert
            Assert.IsType<ArgumentNullException>(ex);
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectUsers()
        {
            return new List<object []>
            {
                new object [] { new user { name = "", surname = "asd", avatar = "asd", passwordhash = "asd", role = "Student", email = "asd",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "", role = "asd", email = "asd",telephonnumber = "asd", patronymic = "asd" } }
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncUserShouldNotCreateUser(user user)
        {
            //arrange
            var newUser = user;

            //act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(newUser));
            //assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }
        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newUser = new user
            {
                name = "asd",
                surname = "asd",
                avatar = "asd",
                passwordhash = "asd",
                role = "Student",
                email = "asd",
                telephonnumber = "asd",
                patronymic = "asd"
            };
            //act
            await service.Create(newUser);

            //assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Once);
        }

    }
}
