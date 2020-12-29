namespace GuideMaker.Repository.Filters
{
    public class LogicalFilter: IFilter
    {
        public IFilter[] Filters { get; }
        public LogicalOperator Operator { get; }
        public FilterType Type { get; }

        public LogicalFilter(LogicalOperator @operator, params IFilter[] filters)
        {
            Filters = filters;
            Operator = @operator;
            Type = FilterType.Logical;
        }
    }
}
