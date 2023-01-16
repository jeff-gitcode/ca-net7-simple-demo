namespace Application.Interface.SPI
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}