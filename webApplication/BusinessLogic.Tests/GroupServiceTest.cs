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
    public class GroupServiceTest
    {
        private readonly GroupService service;
        private readonly Mock<IGroupReposiitory> groupRepositoryMoq;
        private readonly Mock<IGroupValidator> groupValidatorMoq;
        private readonly Mock<IRepositoryWrapper> repositoryWrapperMoq;
        private readonly Mock<IUserRepository> userRepositoryMoq;
        private readonly Mock<IInstitutionRepository> institutionRepositoryMoq;

        public GroupServiceTest()
        {
            repositoryWrapperMoq = new Mock<IRepositoryWrapper>();
            groupRepositoryMoq = new Mock<IGroupReposiitory>();
            groupValidatorMoq = new Mock<IGroupValidator>();
            userRepositoryMoq = new Mock<IUserRepository>();
            institutionRepositoryMoq = new Mock<IInstitutionRepository>();

            repositoryWrapperMoq.Setup(x => x.group).Returns(groupRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.user).Returns(userRepositoryMoq.Object);
            repositoryWrapperMoq.Setup(x => x.institution).Returns(institutionRepositoryMoq.Object);

            service = new GroupService(repositoryWrapperMoq.Object, groupValidatorMoq.Object);
        }

        [Fact]
        public async Task CreateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Create(null));
            Assert.IsType<ArgumentNullException>(ex);
            groupRepositoryMoq.Verify(x => x.Create(It.IsAny<group>()), Times.Never);
        }

        public static IEnumerable<object []> GetIncorrectGroups()
        {
            return new List<object []>
            {
                new object [] { new group { groupname = "", curatorid = 5, institutionid = 10 } },
                new object [] { new group { groupname = "Group A", curatorid = 0, institutionid = 10 } },
                new object [] { new group { groupname = "Group A", curatorid = 5, institutionid = 0 } },
                new object [] { new group { groupname = "", curatorid = 0, institutionid = 0 } },
            };
        }

        [Theory]
        [MemberData(nameof(GetIncorrectGroups))]
        public async Task CreateAsyncGroupShouldNotCreate(group model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.groupname))
                validationResult.Errors.Add(new ValidationFailure("groupname", "Group name is required"));
            if (model.curatorid <= 0)
                validationResult.Errors.Add(new ValidationFailure("curatorid", "Curator ID is required and must be greater than 0"));
            if (model.institutionid <= 0)
                validationResult.Errors.Add(new ValidationFailure("institutionid", "Institution ID is required and must be greater than 0"));

            groupValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Create(model));

            groupRepositoryMoq.Verify(x => x.Create(It.IsAny<group>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task CreateAsyncNewGroupShouldCreate()
        {
            var newGroup = new group
            {
                groupname = "CS101",
                curatorid = 5,
                institutionid = 10,
                course = 1,
                specialty = "Computer Science"
            };

            groupValidatorMoq.Setup(x => x.ValidateAsync(newGroup))
                .ReturnsAsync(new ValidationResult());

            await service.Create(newGroup);

            groupRepositoryMoq.Verify(x => x.Create(It.IsAny<group>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NullModelShouldThrowArgumentNullException()
        {
            var ex = await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Update(null));
            Assert.IsType<ArgumentNullException>(ex);
            groupRepositoryMoq.Verify(x => x.Update(It.IsAny<group>()), Times.Never);
        }

        [Theory]
        [MemberData(nameof(GetIncorrectGroups))]
        public async Task UpdateAsyncGroupShouldNotUpdate(group model)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrEmpty(model.groupname))
                validationResult.Errors.Add(new ValidationFailure("groupname", "Group name is required"));
            if (model.curatorid <= 0)
                validationResult.Errors.Add(new ValidationFailure("curatorid", "Curator ID is required and must be greater than 0"));
            if (model.institutionid <= 0)
                validationResult.Errors.Add(new ValidationFailure("institutionid", "Institution ID is required and must be greater than 0"));

            groupValidatorMoq.Setup(x => x.ValidateAsync(model))
                .ReturnsAsync(validationResult);

            var ex = await Assert.ThrowsAnyAsync<ArgumentException>(() => service.Update(model));

            groupRepositoryMoq.Verify(x => x.Update(It.IsAny<group>()), Times.Never);
            Assert.IsType<ArgumentException>(ex);
        }

        [Fact]
        public async Task UpdateAsyncNewGroupShouldUpdate()
        {
            var updateGroup = new group
            {
                groupid = 10,
                groupname = "Updated CS101",
                curatorid = 8,
                institutionid = 15,
                course = 2,
                specialty = "Updated CS"
            };

            var existingGroup = new group
            {
                groupid = 10,
                groupname = "Old CS101",
                curatorid = 5,
                institutionid = 10,
                course = 1,
                specialty = "Old CS"
            };

            groupRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group> { existingGroup });

            groupValidatorMoq.Setup(x => x.ValidateAsync(updateGroup))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { new user { userid = 8 } });

            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution> { new institution { institutionid = 15 } });

            await service.Update(updateGroup);

            groupRepositoryMoq.Verify(x => x.FindByConditionTraking(It.IsAny<Expression<Func<group, bool>>>()), Times.Once);
            userRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()), Times.Once);
            institutionRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
            groupValidatorMoq.Verify(x => x.ValidateAsync(updateGroup), Times.Once);
        }

        [Fact]
        public async Task UpdateAsyncGroup_WhenCuratorNotFound_ThrowsKeyNotFoundException()
        {
            var updateGroup = new group
            {
                groupid = 10,
                groupname = "Updated CS101",
                curatorid = 999,
                institutionid = 15
            };

            var existingGroup = new group
            {
                groupid = 10,
                groupname = "Old CS101",
                curatorid = 5,
                institutionid = 10
            };

            groupRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group> { existingGroup });

            groupValidatorMoq.Setup(x => x.ValidateAsync(updateGroup))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateGroup));
            Assert.Contains("Did not found user with userId: 999", ex.Message);
        }

        [Fact]
        public async Task UpdateAsyncGroup_WhenInstitutionNotFound_ThrowsKeyNotFoundException()
        {
            var updateGroup = new group
            {
                groupid = 10,
                groupname = "Updated CS101",
                curatorid = 8,
                institutionid = 999
            };

            var existingGroup = new group
            {
                groupid = 10,
                groupname = "Old CS101",
                curatorid = 5,
                institutionid = 10
            };

            groupRepositoryMoq
                .Setup(x => x.FindByConditionTraking(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group> { existingGroup });

            groupValidatorMoq.Setup(x => x.ValidateAsync(updateGroup))
                .ReturnsAsync(new ValidationResult());

            userRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<user, bool>>>()))
                .ReturnsAsync(new List<user> { new user { userid = 8 } });

            institutionRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<institution, bool>>>()))
                .ReturnsAsync(new List<institution>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Update(updateGroup));
            Assert.Contains("Did not found institution with institutionId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenGroupIdIsZeroOrNegative_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(0));
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.GetById(-1));
        }

        [Fact]
        public async Task GetById_WhenGroupNotFound_ThrowsKeyNotFoundException()
        {
            groupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.GetById(999));
            Assert.Contains("Did not found groups with groupId: 999", ex.Message);
        }

        [Fact]
        public async Task GetById_WhenGroupFound_ReturnsGroup()
        {
            var expected = new group
            {
                groupid = 42,
                groupname = "Math 101",
                curatorid = 5,
                institutionid = 10,
                course = 1,
                specialty = "Mathematics"
            };

            groupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group> { expected });

            var result = await service.GetById(42);

            Assert.Equal(42, result.groupid);
            Assert.Equal("Math 101", result.groupname);
            groupRepositoryMoq.Verify(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsAllGroups()
        {
            var mockGroups = new List<group>
            {
                new group { groupid = 1, groupname = "CS101", curatorid = 1, institutionid = 1 },
                new group { groupid = 2, groupname = "Math101", curatorid = 2, institutionid = 2 }
            };

            groupRepositoryMoq
                .Setup(x => x.FindAll())
                .ReturnsAsync(mockGroups);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Equal("CS101", result [0].groupname);
            Assert.Equal("Math101", result [1].groupname);
            groupRepositoryMoq.Verify(x => x.FindAll(), Times.Once);
        }

        [Fact]
        public async Task Delete_WhenGroupIdIsZero_ThrowsArgumentNullException()
        {
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => service.Delete(0));
        }

        [Fact]
        public async Task Delete_WhenGroupNotFound_ThrowsKeyNotFoundException()
        {
            groupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group>());

            var ex = await Assert.ThrowsAnyAsync<KeyNotFoundException>(() => service.Delete(999));
            Assert.Contains("Did not found groups with groupId: 999", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenMultipleGroupsFound_ThrowsInvalidOperationException()
        {
            var groups = new List<group>
            {
                new group { groupid = 5, groupname = "Test" },
                new group { groupid = 5, groupname = "Test2" }
            };

            groupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(groups);

            var ex = await Assert.ThrowsAnyAsync<InvalidOperationException>(() => service.Delete(5));
            Assert.Contains("found more then one group", ex.Message);
        }

        [Fact]
        public async Task Delete_WhenGroupFound_DeletesAndSaves()
        {
            var groupToDelete = new group
            {
                groupid = 777,
                groupname = "ToDelete",
                curatorid = 888,
                institutionid = 999
            };

            groupRepositoryMoq
                .Setup(x => x.FindByCondition(It.IsAny<Expression<Func<group, bool>>>()))
                .ReturnsAsync(new List<group> { groupToDelete });

            await service.Delete(777);

            groupRepositoryMoq.Verify(x => x.Delete(It.IsAny<group>()), Times.Once);
            repositoryWrapperMoq.Verify(x => x.Save(), Times.Once);
        }
    }
}