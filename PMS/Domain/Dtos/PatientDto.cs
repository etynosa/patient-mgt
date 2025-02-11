namespace PMS.Domain.Dtos
{
    public class PatientDto
    {
    }

    public record PatientCreateDto(string FirstName, string LastName, DateTime DateOfBirth, string Gender, string Address);

    public record PatientUpdateDto(string FirstName, string LastName, DateTime DateOfBirth, string Gender, string Address);

    public record PatientResponseDto(int Id, string FirstName, string LastName, DateTime DateOfBirth, string Gender, string Address);
}
