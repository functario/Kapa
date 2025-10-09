namespace HomeAutomation.Actors;

public interface IUser
{
    public Home? Home { get; set; }
    public Identification? Identification { get; set; }
}
