using HomeAutomation.Prototypes.Homes;
using HomeAutomation.Prototypes.Identifications;

namespace HomeAutomation.Prototypes;

public interface IUser
{
    public Home? Home { get; set; }
    public Identification? Identification { get; set; }
}
