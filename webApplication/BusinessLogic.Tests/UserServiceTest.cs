using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Services;
using Domain.Interfaces;
using Domain.Models;
using Moq;
using Validators.Interefaces;
using FluentValidation.Results;


namespace BusinessLogic.Tests
{
    public class UserServiceTest
    {
        private readonly UserService service;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IUserValidator> userValidatorMoq;
        public UserServiceTest()
        {
            var repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            userRepositoryMoq = new Mock<IUserRepository>();
            userValidatorMoq = new Mock<IUserValidator>();
            
            

            repositoryWrapperMoq.Setup(x => x.user)
                .Returns(userRepositoryMoq.Object);

           

            service = new UserService(repositoryWrapperMoq.Object, userValidatorMoq.Object);

            
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
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "", role = "asd", email = "asd",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "", passwordhash = "asd", role = "asd", email = "asd",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd",telephonnumber = "", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asd", passwordhash = "asd", role = "asd", email = "asd",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 2, name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 5, name = "asd", surname = "asd", avatar = "asg", passwordhash = "", role = "asd", email = "",telephonnumber = "asd", patronymic = "asd" } },
                new object [] { new user { userid = 4, name = "asd", surname = "asd", avatar = "asg", passwordhash = "asd", role = "asd", email = "",telephonnumber = "asd", patronymic = "asd" } },

            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]
        public async Task CreateAsyncUserShouldNotCreateUser(user user)
        {
            // Arrange 
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

            // Act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(user));

            // Assert
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
            //act
            await service.Create(newUser);
            userValidatorMoq.Setup(x => x.ValidateAsync(newUser))
                    .ReturnsAsync(new ValidationResult());
            //assert
            userRepositoryMoq.Verify(x => x.Create(It.IsAny<user>()), Times.Once);
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
                email = "asd",
                telephonnumber = "asd",
                patronymic = "asd",
                isactive = true,
                createdat = DateTime.Now

            };
            //act
            await service.Update(updateUser);
            userValidatorMoq.Setup(x => x.ValidateAsync(updateUser))
            .ReturnsAsync(new ValidationResult());
            //assert
            userRepositoryMoq.Verify(x => x.Update(It.IsAny<user>()), Times.Once);
        }
        [Theory]
        [MemberData(nameof(GetIncorrectUsers))]

        public async Task UpdateAsyncUserShouldNotUpdateUser(user user)
        {
            // Arrange 
            var validationResult = new ValidationResult();
            if (user.userid == 0)
                validationResult.Errors.Add(new ValidationFailure("userId", "Inccorect userId"));

            if (string.IsNullOrEmpty(user.name))
                validationResult.Errors.Add(new ValidationFailure("name", "Name is required"));

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

            //act
            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(user));
            //assert
            userRepositoryMoq.Verify(x => x.Update(It.IsAny<user>()), Times.Never);
        }

    }
}
