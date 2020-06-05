namespace VerticalToDo.Services
{
    public abstract class BasePaginationResponse : BaseResponse
    {
        public int Count { get; set; }
    }
}
