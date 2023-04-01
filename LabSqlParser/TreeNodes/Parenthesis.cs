sealed record Parenthesis(
	IExpression Expression
) : IExpression {
	public string ToFormattedString() {
		return $"({Expression.ToFormattedString()})";
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitParenthesis(this);
	}
}
