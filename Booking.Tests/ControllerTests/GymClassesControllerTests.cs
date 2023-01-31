using AutoMapper;
using Booking.Core.Entities;
using Booking.Core.Repositories;
using Booking.Core.ViewModels;
using Booking.Data;
using Booking.Tests.Helpers;
using Booking.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Booking.Tests.ControllerTests
{
    public class GymClassesControllerTests
    {
        private GymClassesController controller;
        private Mock<IGymClassRepository> mockRepo;

        public GymClassesControllerTests()
        {
            mockRepo = new Mock<IGymClassRepository>();
            var mockUoW = new Mock<IUnitOfWork>();
            mockUoW.Setup(u => u.GymClassRepository).Returns(mockRepo.Object);

            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.ConstructServicesUsing(c => new AttendingResolver(Mock.Of<IHttpContextAccessor>()));
                cfg.AddProfile<MapperProfile>();
            }));

            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new UserManager<ApplicationUser>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            controller = new GymClassesController(mockUoW.Object, userManager, mapper);
        }


        [Fact]
        public async void Index_NotAuthenticated_ShouldReturnIndexViewModel()
        {
            //Arrange
            controller.SetUserIsAuthenticated(false);
            mockRepo.Setup(c => c.GetAsync()).ReturnsAsync(new List<GymClass>());

            //Act
            var actual = await controller.Index(new IndexViewModel()) as ViewResult;

            //Assert
            Assert.IsType<IndexViewModel>(actual?.Model);

        }
    }
}