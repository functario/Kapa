namespace HomeAutomation.Prototypes.Homes;

public interface IDevice
{
    Guid Id { get; }
    string Model { get; }
    string Name { get; }
}
