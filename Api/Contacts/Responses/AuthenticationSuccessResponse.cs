namespace Api.Contacts.Responses
{
    public class AuthenticationSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
