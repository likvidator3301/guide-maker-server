namespace GuideMaker.Repository.Filters
{
    public interface IFilter
    {
        FilterType Type { get; }
    }

    public enum FilterType
    {
        Comparison,
        Logical
    }
}
