namespace DotnetApi.DTOs
{
    public class UserForLoginConfirmationDTO
    {
        public byte[] Passwordhash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserForLoginConfirmationDTO()
        {
            if (Passwordhash == null)
            {
                Passwordhash = new byte[0];
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = new byte[0];
            }
        }
    }
}
