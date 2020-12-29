namespace GuideMaker.Repository.Filters
{
    public class ComparisonFilter: IFilter
    {
        public string FieldName { get; }
        public string Value { get; }
        public ComparisonOperator Operator { get; }
        public FilterType Type { get; }

        public ComparisonFilter(string fieldName, string value, ComparisonOperator @operator)
        {
            FieldName = fieldName;
            Value = value;
            Operator = @operator;
            Type = FilterType.Comparison;
        }

        
    }
}
