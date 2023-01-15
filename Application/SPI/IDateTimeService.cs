namespace Application.SPI
{
    public interface IDateTimeService
    {
        DateTime UtcNow { get; }
    }
}