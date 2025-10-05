namespace Kapa.Abstractions;

public interface IValueInfo
{
    public Kinds Kinds { get; }
    public string TypeFullPath { get; }
    public bool IsGeneric { get; }
    public ICollection<IValueInfo> GenericArguments { get; }
}
