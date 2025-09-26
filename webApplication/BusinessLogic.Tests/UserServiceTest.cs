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
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IUserValidator> userValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;

        public UserServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();
            userValidatorMoq = new Mock<IUserValidator>();

            repositoryWrapperMoq.Setup(x => x.user)
                .Returns(userRepositoryMoq.Object);

            service = new UserService(repositoryWrapperMoq.Object, userValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullUSerShouldTrowNullArgumentExeption()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectUsers()
        {
            return new List<object []>
            {
                new object [] { new user { name = "", surname = "asd", avatar = "asd", passwordhash = "asd", role = "Student", email = "asd", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "", role = "asd", email = "asd", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "", passwordhash = "asd", role = "asd", email = "asd", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd", telephonnumber = "", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 2, name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 5, name = "asd", surname = "asd", avatar = "asg", passwordhash = "", role = "asd", email = "", telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 4, name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "", telephonnumber = "asd", patronymic = "asd" } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncUserShouldNotCreateUser(user user)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(user.name))
                validationResult.Errors.Add(new ValidationFailure("name", "Name is required"));
            if (string.IsNullOrEmpty(user.surname))
                validationResult.Errors.Add(new ValidationFailure("surname", "Surname is required"));
            if (string.IsNullOrEmpty(user.patronymic))
                validationResult.Errors.Add(new ValidationFailure("patronymic", "Patronymic is required"));
            if (string.IsNullOrEmpty(user.email))
                validationResult.Errors.Add(new ValidationFailure("email", "Email is required"));
            if (string.IsNullOrEmpty(user.passwordhash))
                validationResult.Errors.Add(new ValidationFailure("passwordhash", "Password hash is required"));
            if (string.IsNullOrEmpty(user.role) || !new [] { "Student", "Teacher", "Admin" }.Contains(user.role))
                validationResult.Errors.Add(new ValidationFailure("role", "Role must be one of: Student, Teacher, Admin"));
            if (string.IsNullOrEmpty(user.avatar))
                validationResult.Errors.Add(new ValidationFailure("avatar", "Avatar is required"));
            if (string.IsNullOrEmpty(user.telephonnumber))
                validationResult.Errors.Add(new ValidationFailure("telephonnumber", "Telephone number is required"));

            userValidatorMoq.Setup(x => x.ValidateAsync(user))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(user));
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewUserShouldCreateNewUser()
        {
            var newUser = new user
            {
                name = "asdsdf",
                surname = "asdsdf",
                avatar = "asdsdf",
                passwordhash = "asd",
                role = "Student",
                email = "jjdkfie.kkdk@gmail.com",
                telephonnumber = "89033673829",
                patronymic = "asdsdf"
            };

            userValidatorMoq.Setup(x => x.ValidateAsync(newUser))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newUser);

            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncUserShouldUpdateUser()
        {
            var updateUser = new user
            {
                userid = 4,
                name = "asd",
                surname = "asd",
                avatar = "asd",
                passwordhash = "asd",
                role = "Student",
                email = "asdasd@gmail.com",
                telephonnumber = "89034673467",
                patronymic = "asd",
                isactive = true,
                createdat = DateTime.Now
            };

            var existingUser = new user
            {
                userid = 4,
                name = "oldname",
                surname = "oldsurname",
                avatar = "old",
                passwordhash = "oldhash",
                role = "Student",
                email = "old@gmail.com",
                telephonnumber = "89000000000",
                patronymic = "oldpat",
                isactive = true,
                createdat = DateTime.Now
            };

            userRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { existingUser });

            userValidatorMoq.Setup(x => x.ValidateAsync(updateUser))
                .ReturnsAsync(new ValidationResult());

            await service.Update(updateUser);

            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            userRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
            userValidatorMoq.Verify(x => x.ValidateAsync(updateUser), Times.Once);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task UpdateAsyncUserShouldNotUpdateUser(user user)
        {
            var validationResult = new ValidationResult();

            if (user.userid == 0)
                validationResult.Errors.Add(new ValidationFailure("userId", "Inccorect userId"));
            if (string.IsNullOrEmpty(user.name))
                validationResult.Errors.Add(new ValidationFailure("name", "Name is required"));
            if (string.IsNullOrEmpty(user.surname))
                validationResult.Errors.Add(new ValidationFailure("surname", "Surname is required"));
            if (string.IsNullOrEmpty(user.patronymic))
                validationResult.Errors.Add(new ValidationFailure("patronymic", "Patronymic is required"));
            if (string.IsNullOrEmpty(user.email))
                validationResult.Errors.Add(new ValidationFailure("email", "Email is required"));
            if (string.IsNullOrEmpty(user.passwordhash))
                validationResult.Errors.Add(new ValidationFailure("passwordhash", "Password hash is required"));
            if (string.IsNullOrEmpty(user.role) || !new [] { "Student", "Teacher", "Admin" }.Contains(user.role))
                validationResult.Errors.Add(new ValidationFailure("role", "Role must be one of: Student, Teacher, Admin"));
            if (string.IsNullOrEmpty(user.avatar))
                validationResult.Errors.Add(new ValidationFailure("avatar", "Avatar is required"));
            if (string.IsNullOrEmpty(user.telephonnumber))
                validationResult.Errors.Add(new ValidationFailure("telephonnumber", "Telephone number is required"));

            userValidatorMoq.Setup(x => x.ValidateAsync(user))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(user));

            repositoryWrapperMoq.Verify(x => x.Save(), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsync_WhenUserIdIsZero_ShouldStillUpdateUser()
        {
            var updateUser = new user
            {
                userid = 0,
                name = "updatedname",
                surname = "updatedsurname",
                avatar = "updatedavatar",
                passwordhash = "newhash",
                role = "Student",
                email = "updated@example.com",
                telephonnumber = "89012345678",
                patronymic = "updatedpatronymic",
                isactive = true,
                createdat = DateTime.Now
            };

            var existingUser = new user
            {
                userid = 10,
                name = "oldname",
                surname = "oldsurname",
                avatar = "old",
                passwordhash = "oldhash",
                role = "Student",
                email = "old@example.com",
                telephonnumber = "89000000000",
                patronymic = "oldpat",
                isactive = true,
                createdat = DateTime.Now
            };

            userRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { existingUser });

            userValidatorMoq.Setup(x => x.ValidateAsync(updateUser))
                .ReturnsAsync(new ValidationResult());

            await service.Update(updateUser);

            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            userRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
            userValidatorMoq.Verify(x => x.ValidateAsync(updateUser), Times.Once);
        }

        [Fact]
        public async Task GetById_WhenIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenUserNotFound_ThrowsKeyNotFoundException()
        {
            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("User with ID 999 not found.", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenUserFound_ReturnsUser()
        {
            var expectedUser = new user
            {
                userid = 42,
                name = "Alice",
                email = "alice@example.com"
            };

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { expectedUser });

            var result = await service.GetById(42);

            Assert.Equal(42, result.userid);
            Assert.Equal("Alice", result.name);
            Assert.Equal("alice@example.com", result.email);
            userRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllUsers()
        {
            var mockUsers = new List<user>
            {
                new user { userid = 1, name = "John" },
                new user { userid = 2, name = "Jane" }
            };

            userRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockUsers);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("John", result [0].name);
            Assert.Equal("Jane", result [1].name);
            userRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(-1));
        }

        [Fact]
        public async Task Delete_WhenUserNotFound_ThrowsKeyNotFoundException()
        {
            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("User with ID 999 not found.", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleUsersFound_ThrowsInvalidOperationException()
        {
            var users = new List<user>
            {
                new user { userid = 5, name = "User1" },
                new user { userid = 5, name = "User2" }
            };

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(users);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("Finded more then one user with ID: 5", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenUserFound_DeletesAndSaves()
        {
            var userToDelete = new user { userid = 777, name = "Bob" };

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { userToDelete });

            await service.Delete(777);

            userRepositoryMoq.Verify(x => x.Delete(It.IsAny<user>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}