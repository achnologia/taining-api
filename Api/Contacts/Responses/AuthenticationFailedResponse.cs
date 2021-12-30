using System.Collections.Generic;

namespace Api.Contacts.Responses
{
    public class AuthenticationFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
