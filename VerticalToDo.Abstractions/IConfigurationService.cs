namespace VerticalToDo.Abstractions
{
    public interface IConfigurationService
    {
        string ConnectionString { get; }
        string AuthTokenLifetimeInSeconds { get; }
    }
}
