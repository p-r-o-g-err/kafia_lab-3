using System;
namespace LabSqlParser.Lexer;
public class LexerException : Exception {
	public LexerException(string? message) : base(message) { }
}
