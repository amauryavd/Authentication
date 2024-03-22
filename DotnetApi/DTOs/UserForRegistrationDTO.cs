namespace DotnetApi.DTOs
{
    public class UserForRegistrationDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }

        public UserForRegistrationDTO()
        {
            if (Email == null)
            {
                Email = string.Empty;
            }
            if (Password == null)
            {
                Password = string.Empty;
            }
            if (PasswordConfirm == null)
            {
                PasswordConfirm = string.Empty;
            }
            if (FirstName == null)
            {
                FirstName = "";
            }
            if (LastName == null)
            {
                LastName = "";
            }
            if (Gender == null)
            {
                Gender = "";
            }
        }

    }
}
