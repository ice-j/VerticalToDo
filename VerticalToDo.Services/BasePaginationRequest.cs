namespace VerticalToDo.Services
{
    public abstract class BasePaginationRequest<TResponse> : BaseRequest<TResponse>
		where TResponse : BasePaginationResponse
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		internal int Skip => PageNumber > 1 ? (PageNumber - 1) * PageSize : 0;
	}
}
