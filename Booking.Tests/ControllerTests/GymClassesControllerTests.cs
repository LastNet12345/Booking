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
            var userManager = new UserManager<ApplicationUser>(mockUserStore.Object, null!, null!, null!, null!, null!, null!, null!, null!);

            controller = new GymClassesController(mockUoW.Object, userManager, mapper);
        }


        [Fact]
        public async Task Index_NotAuthenticated_ShouldReturnIndexViewModel()
        {
            //Arrange
            controller.SetUserIsAuthenticated(false);
            mockRepo.Setup(c => c.GetAsync()).ReturnsAsync(new List<GymClass>());

            //Act
            var actual = await controller.Index(new IndexViewModel()) as ViewResult;

            //Assert
            Assert.IsType<IndexViewModel>(actual?.Model);

        }

        [Fact]
        public async Task Index_AuthenticatedUser_WithShowHistoryIsFalse_ShouldReturnIndexViewModelAndExpectedClasses()
        {
            //Arrange
            controller.SetUserIsAuthenticated(true);
            var gymClasses = GetGymClasses();
            var expected = gymClasses.Count();
            mockRepo.Setup(c => c.GetWithAttendinAsync()).ReturnsAsync(gymClasses);

            var vm = new IndexViewModel { ShowHistory = false };

            //Act
            var actual = (await controller.Index(vm) as ViewResult)?.Model as IndexViewModel;

            //Assert
            Assert.IsType<IndexViewModel>(actual);
            Assert.Equal(expected, actual?.GymClasses.Count());

        }

        [Fact]
        public void Create_Get_ReturnsDefaultView_ShouldReturnNull()
        {
            controller.SetAjaxRequest(false);

            var actual = controller.Create() as ViewResult;

            Assert.IsType<ViewResult>(actual);
            Assert.Null(actual?.ViewName);
        }
        
        
        [Fact]
        public void Create_Get_WithAjax_ReturnsPartialView_ShouldReturnCreatePartial()
        {
            controller.SetAjaxRequest(true);
            const string correctPartialViewName = "CreatePartial";

            var actual = controller.Create() as PartialViewResult;

            Assert.IsType<PartialViewResult>(actual);
            Assert.Equal(correctPartialViewName, actual?.ViewName);
        }


            private IEnumerable<GymClass> GetGymClasses()
        {
            return new List<GymClass>
            {
                new GymClass
                {
                    Id = 1,
                    Name = "Spinning",
                    Description = "Easy",
                    StartTime = DateTime.Now.AddDays(3),
                    Duration = new TimeSpan(0,60,0)

                },
                new GymClass
                {
                    Id = 2,
                    Name = "Body Pump",
                    Description = "Hard",
                    StartTime = DateTime.Now.AddDays(23),
                    Duration = new TimeSpan(0,60,0)
                },
                new GymClass
                {
                    Id = 3,
                    Name = "Core",
                    Description = "Hard",
                    StartTime = DateTime.Now.AddDays(-2),
                    Duration = new TimeSpan(0,60,0)
                }
            };
        }
    }
}