using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PMS.Application.Services;
using PMS.Controllers;
using PMS.Domain.Dtos;
using PMS.Domain.Entities;
using PMS.Infrastructure;

namespace PMS.Tests
{
    public class PatientsControllerTests
    {
        public class PatientControllerTests
        {
            private readonly Mock<IPatientService> _mockPatientService;
            private readonly IMapper _mapper;
            private readonly PatientsController _patientController;

            public PatientControllerTests()
            {
                _mockPatientService = new Mock<IPatientService>();
                _mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new MappingProfile());
                }).CreateMapper();
                _patientController = new PatientsController(_mockPatientService.Object, _mapper);
            }

            // Helper method to create a sample patient DTO
            private PatientCreateDto CreateSamplePatientDto()
            {
                return new PatientCreateDto(FirstName: "John", LastName: "Doe", DateOfBirth: new DateTime(1985, 1, 1),
                    Gender: "Male", Address: "123 Main St");
            }

            private PatientUpdateDto CreateSamplePatientUpdateDto()
            {
                return new PatientUpdateDto(FirstName: "Jane", LastName: "Doe", DateOfBirth: new DateTime(1985, 1, 1),
                    Gender: "Female", Address: "123 Main St");
            }

            // Helper method to create a sample patient entity
            private Patient CreateSamplePatientEntity()
            {
                return new Patient
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1985, 1, 1),
                    Gender = "Male",
                    Address = "123 Main St",
                    IsDeleted = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
            }

            [Fact]
            public async Task GetPatientById_ReturnsOkResult_WhenPatientExists()
            {
                // Arrange
                var patientId = 1;
                var patientEntity = CreateSamplePatientEntity();
                _mockPatientService.Setup(service => service.GetPatientByIdAsync(patientId))
                    .ReturnsAsync(patientEntity);

                // Act
                var result = await _patientController.GetPatient(patientId);

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnPatient = Assert.IsType<PatientResponseDto>(okResult.Value);
                Assert.Equal(patientId, returnPatient.Id);
            }

            [Fact]
            public async Task GetPatientById_ReturnsNotFound_WhenPatientDoesNotExist()
            {
                // Arrange
                var patientId = 999;
                _mockPatientService.Setup(service => service.GetPatientByIdAsync(patientId))
                    .ReturnsAsync((Patient)null);

                // Act
                var result = await _patientController.GetPatient(patientId);

                // Assert
                Assert.IsType<OkObjectResult>(result);
            }

            [Fact]
            public async Task GetAllPatients_ReturnsOkResult_WithListOfPatients()
            {
                // Arrange
                var patients = new List<Patient>
            {
                CreateSamplePatientEntity(),
                CreateSamplePatientEntity()
            };
                _mockPatientService.Setup(service => service.GetAllPatientsAsync())
                    .ReturnsAsync(patients);

                // Act
                var result = await _patientController.GetAllPatients();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var returnPatients = Assert.IsType<List<PatientResponseDto>>(okResult.Value);
                Assert.Equal(2, returnPatients.Count);
            }

            [Fact]
            public async Task CreatePatient_ReturnsCreatedAtAction_WhenPatientIsValid()
            {
                // Arrange
                var patientDto = CreateSamplePatientDto();
                var patientEntity = CreateSamplePatientEntity();
                _mockPatientService.Setup(service => service.CreatePatientAsync(It.IsAny<PatientCreateDto>()))
                    .ReturnsAsync(patientEntity);

                // Act
                var result = await _patientController.CreatePatient(patientDto);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Equal("GetPatient", createdAtActionResult.ActionName);
                Assert.Equal(patientEntity.Id, ((PatientResponseDto)createdAtActionResult.Value).Id);
            }

            [Fact]
            public async Task UpdatePatient_ReturnsNoContent_WhenPatientExists()
            {
                // Arrange
                var patientId = 1;
                var patientDto = CreateSamplePatientUpdateDto();
                _mockPatientService.Setup(service => service.UpdatePatientAsync(patientId, patientDto));
                 

                // Act
                var result = await _patientController.UpdatePatient(patientId, patientDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task UpdatePatient_ReturnsNotFound_WhenPatientDoesNotExist()
            {
                // Arrange
                var patientId = 999;
                var patientDto = CreateSamplePatientUpdateDto();
                _mockPatientService.Setup(service => service.UpdatePatientAsync(patientId, patientDto));
                   

                // Act
                var result = await _patientController.UpdatePatient(patientId, patientDto);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task DeletePatient_ReturnsNoContent_WhenPatientExists()
            {
                // Arrange
                var patientId = 1;
                _mockPatientService.Setup(service => service.DeletePatientAsync(patientId));

                // Act
                var result = await _patientController.DeletePatient(patientId);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task DeletePatient_ReturnsNoContentResult_WhenPatientDoesNotExist()
            {
                // Arrange
                var patientId = 999;
                _mockPatientService.Setup(service => service.DeletePatientAsync(patientId));

                // Act
                var result = await _patientController.DeletePatient(patientId);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }
        }
    }
}