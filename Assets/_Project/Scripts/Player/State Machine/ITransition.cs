namespace ThePatient
{
    public interface ITransition
    {
        IState TargetState { get; }
        IPredicate Predicate { get; }
    }
}
