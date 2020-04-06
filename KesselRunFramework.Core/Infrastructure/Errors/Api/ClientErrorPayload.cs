namespace KesselRunFramework.Core.Infrastructure.Errors.Api
{
    /// <summary>
    /// An response returned to the client
    /// </summary>
    public class ClientErrorPayload
    {
        public string Message { get; set; }
        public bool IsError { get; set; }
        public bool IsValidationFail { get; set; }
        public string Detail { get; set; }
        public object Data { get; set; }
    }
}
