using System.Collections.Generic;

namespace training_api.Contacts.Responses
{
    public class AuthenticationFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
