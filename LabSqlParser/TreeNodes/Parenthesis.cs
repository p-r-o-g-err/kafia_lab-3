sealed record Parenthesis(
	IExpression Expression
) : IExpression {
	public string ToFormattedString() {
		return $"({Expression.ToFormattedString()})";
	}
}
