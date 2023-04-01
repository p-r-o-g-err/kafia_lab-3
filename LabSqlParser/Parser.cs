using System;
using System.Collections.Generic;
using System.Linq;
sealed class Parser {
	readonly IReadOnlyList<Token> tokens;
	int pos;
	public Parser(IReadOnlyList<Token> tokens) {
		this.tokens = tokens;
	}
	#region common
	Exception MakeError(string message) {
		throw new InvalidOperationException($"{message} в {pos}");
	}
	void ReadNextToken() {
		pos++;
	}
	Token CurrentToken => tokens[pos];
	void Expect(string s) {
		if (CurrentToken.Lexeme != s) {
			throw MakeError($"Ожидалось \"{s}\", получено \"{CurrentToken.Lexeme}\"");
		}
		ReadNextToken();
	}
	void Expect(TokenType expectedType) {
		if (CurrentToken.Type != expectedType) {
			throw MakeError($"Ожидался тип токена \"{expectedType}\", получен \"{CurrentToken.Type}\"");
		}
	}
	bool SkipIf(string s) {
		if (CurrentToken.Lexeme == s) {
			ReadNextToken();
			return true;
		}
		return false;
	}
	void ExpectEof() {
		if (CurrentToken.Type != TokenType.EndOfFile) {
			throw MakeError($"Ожидался конец файла, получен {CurrentToken}");
		}
	}
	#endregion
	public static List<Token> PrepareTokens(IEnumerable<Token> tokens) {
		return tokens.Where(token => token.Type != TokenType.Spaces).Append(new Token(TokenType.EndOfFile, "")).ToList();
	}
	public static Insert Parse(IEnumerable<Token> tokens) {
		var tokenList = PrepareTokens(tokens);
		var parser = new Parser(tokenList);
		var insert = parser.ParseInsert();
		parser.ExpectEof();
		return insert;
	}
	Insert ParseInsert() {
		Expect("INSERT");
		Expect("INTO");
		var tableName = ParseIdentifier();
		Expect("VALUES");
		Expect("(");
		var row = ParseExpression();
		Expect(")");
		var limitIsAll = false;
		IExpression? limit = null;
		if (SkipIf("LIMIT")) {
			if (SkipIf("ALL")) {
				limitIsAll = true;
			}
			else {
				limit = ParseExpression();
			}
		}
		return new Insert(tableName, row, limitIsAll, limit);
	}
	IExpression ParseExpression() {
		var left = ParseConditionalAnd();
		while (SkipIf("OR")) {
			var right = ParseConditionalAnd();
			left = new BinaryOperation(left, BinaryOperationType.Or, right);
		}
		return left;
	}
	IExpression ParseConditionalAnd() {
		var left = ParseAdditive();
		while (SkipIf("AND")) {
			var right = ParseAdditive();
			left = new BinaryOperation(left, BinaryOperationType.And, right);
		}
		return left;
	}
	IExpression ParseAdditive() {
		var left = ParseMultiplicative();
		while (SkipIf("+")) {
			var right = ParseMultiplicative();
			left = new BinaryOperation(left, BinaryOperationType.Add, right);
		}
		return left;
	}
	IExpression ParseMultiplicative() {
		var left = ParsePrimary();
		while (SkipIf("/")) {
			var right = ParsePrimary();
			left = new BinaryOperation(left, BinaryOperationType.Division, right);
		}
		return left;
	}
	IExpression ParsePrimary() {
		if (CurrentToken.Type == TokenType.Number) {
			return ParseNumber();
		}
		if (SkipIf("(")) {
			var expression = ParseExpression();
			Expect(")");
			var parenthesis = new Parenthesis(expression);
			return parenthesis;
		}
		throw MakeError("Ожидалось число или выражение в скобках.");
	}
	Identifier ParseIdentifier() {
		Expect(TokenType.Identifier);
		var identifier = new Identifier(CurrentToken.Lexeme);
		ReadNextToken();
		return identifier;
	}
	Number ParseNumber() {
		Expect(TokenType.Number);
		var number = new Number(CurrentToken.Lexeme);
		ReadNextToken();
		return number;
	}
}
