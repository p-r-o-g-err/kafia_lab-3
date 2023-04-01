sealed record Insert(
	Identifier TableName,
	IExpression Row,
	bool LimitIsAll,
	IExpression? Limit
) : INode {
	public string ToFormattedString() {
		var insert = $"INSERT INTO {TableName.ToFormattedString()} VALUES ({Row.ToFormattedString()})";
		if (LimitIsAll) {
			insert += " LIMIT ALL";
		}
		else if (Limit != null) {
			insert += $" LIMIT {Limit.ToFormattedString()}";
		}
		return insert;
	}
	public void AcceptVisitor(INodeVisitor visitor) {
		visitor.VisitInsert(this);
	}
}
