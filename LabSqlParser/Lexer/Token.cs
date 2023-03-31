sealed record Token(
	TokenType Type,
	string Lexeme
) {
	public override string ToString() {
		return $"{Type,10} -> \"{Lexeme}\"";
	}
}
