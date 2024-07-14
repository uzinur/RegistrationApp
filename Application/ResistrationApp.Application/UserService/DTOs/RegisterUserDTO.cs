namespace ResistrationApp.Application.UserService.DTOs
{
    public class RegisterUserDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public int ProvinceId { get; set; }
        public bool IsAgree { get; set; }
    }
}