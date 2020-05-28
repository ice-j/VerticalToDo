namespace VerticalToDo.Core.Features.Accounts
{
    public class Account : BaseEntity
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
