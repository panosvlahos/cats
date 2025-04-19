using Application.Commands;
using Cats.Controllers;
using Entities.Models;
using Interfaces.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cats.Tests
{
   

    public class CatsControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMediator> _mediatorMock;
        private readonly CatsController _controller;

        public CatsControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mediatorMock = new Mock<IMediator>();
            _controller = new CatsController(_unitOfWorkMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Fetch_ReturnsJobId()
        {
            // Arrange
            var jobId = "12345";
            _mediatorMock.Setup(m => m.Send(It.IsAny<FetchCatsCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(jobId);

            // Act
            var result = await _controller.Fetch();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal(jobId, value["jobId"]);
        }

        [Fact]
        public async Task GetCatById_ReturnsCat_WhenFound()
        {
            var catId = "abc123";
            var cat = new Cat { CatId = catId };
            _unitOfWorkMock.Setup(u => u.CatRepository.GetCatByIdAsync(catId)).ReturnsAsync(cat);

            var result = await _controller.GetCatById(catId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cat, okResult.Value);
        }

        [Fact]
        public async Task GetCatById_ReturnsNotFound_WhenNotFound()
        {
            _unitOfWorkMock.Setup(u => u.CatRepository.GetCatByIdAsync("nope")).ReturnsAsync((Cat)null);

            var result = await _controller.GetCatById("nope");

            Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData("playful")]
        [InlineData("friendly")]
        public async Task GetCatsByTag_ReturnsCats(string tag)
        {
            var cats = new List<Cat> { new Cat { CatId = "1" } };
            _unitOfWorkMock.Setup(u => u.CatRepository.GetCatsByTagAsync(tag)).ReturnsAsync(cats);

            var result = await _controller.GetCatsByTag(tag);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cats, okResult.Value);
        }

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 5)]
        public async Task GetCats_ReturnsPagedCats(int page, int pageSize)
        {
            var cats = new List<Cat> { new Cat { CatId = "1" } };
            _unitOfWorkMock.Setup(u => u.CatRepository.GetCatsAsync(page, pageSize)).ReturnsAsync(cats);

            var result = await _controller.GetCats(page, pageSize);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(cats, okResult.Value);
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        public async Task GetCats_InvalidPaging_ReturnsBadRequest(int page, int pageSize)
        {
            var result = await _controller.GetCats(page, pageSize);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

}
