namespace Kapa.Abstractions;

/// <summary>
/// The supported kind of types by <see cref="Kapa"/>.
/// </summary>
public interface IKinds
{
    public IKinds StringKind { get; }
    public IKinds NumberKind { get; }
    public IKinds IntegerKind { get; }
    public IKinds ObjectKind { get; }
    public IKinds ArrayKind { get; }
    public IKinds BooleanKind { get; }
}
