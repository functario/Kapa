namespace HomeAutomation.Actors.Homes;

public class Light : Device
{
    public Light(Guid id, string name, string model)
        : base(id, name, model)
    {
        Id = id;
        Name = name;
        Model = model;
    }

    public bool IsOn { get; set; }
}
