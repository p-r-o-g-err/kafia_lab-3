sealed record Insert(
	Identifier TableName,
	IExpression Row,
	IExpression Limit
	) : INode {
	public string ToFormattedString() {
		var str = $"INSERT INTO {TableName.ToFormattedString()} VALUES ({Row.ToFormattedString()})";
		str += Limit == null ? "" : $" LIMIT {Limit.ToFormattedString()}";
		return str;
	}
}
