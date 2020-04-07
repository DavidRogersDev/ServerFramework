namespace KesselRun.Business.DataTransferObjects
{
    public class RegisterUserPayloadDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
