using AutoMap.Base;


namespace Schema.Response
{
    public class UserResponse:BaseResponse
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string IBAN { get; set; }


    }
}
