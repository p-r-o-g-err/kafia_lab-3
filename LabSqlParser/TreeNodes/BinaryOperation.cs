sealed record BinaryOperation(
	IExpression Left,
	BinaryOperationType Operator,
	IExpression Right
	) : IExpression {
	public string ToFormattedString() {
		return $"{Left.ToFormattedString()} {OperatorToString()} {Right.ToFormattedString()}";
	}
	public string OperatorToString() {
		return Operator switch {
			BinaryOperationType.Or => "OR",
			BinaryOperationType.And => "AND",
			BinaryOperationType.Add => "+",
			BinaryOperationType.Division => "/",
			_ => throw new System.NotImplementedException($"Для оператора \"{Operator}\" преобразование в строку не определено"),
		};
	}
}
