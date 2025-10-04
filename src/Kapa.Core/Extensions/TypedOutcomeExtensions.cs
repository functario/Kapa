//using Kapa.Abstractions.Outcomes;
//using Kapa.Core.Outcomes;

//namespace Kapa.Core.Extensions;

//public static class TypedOutcomeExtensions
//{
//    public static TypedOutcome<T> ToTypedOutcome<T>(
//        this T value,
//        string source,
//        OutcomeStatus status
//    )
//    {
//        new(source, status, value);
//    }

//    public static TypedOutcome<T> ToOkTypedOutcome<T>(this T value, string source)
//    {
//        new(source, OutcomeStatus.Ok, value);
//    }

//    public static TypedOutcome<T> ToFailTypedOutcome<T>(this T value, string source)
//    {
//        new(source, OutcomeStatus.Fail, value);
//    }
//}
