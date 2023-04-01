using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace LabSqlParser.Lexer;
static class Lexer {
	static readonly Regex lexemeRx = new(
		"""
		(?<spaces>[\t\n\r ]+)|
		(?<identifier>[a-zA-Z_][a-zA-Z0-9_]*)|
		(?<number>[0-9]+)|
		(?<punctuator>[()+\/])
		""", RegexOptions.IgnorePatternWhitespace);
	static IEnumerable<Match> GetMatches(Regex rx, string input) {
		var match = rx.Match(input);
		while (match.Success) {
			yield return match;
			match = match.NextMatch();
		}
	}
	public static IEnumerable<Token> GetTokens(string input) {
		var lastPosition = 0;
		foreach (var m in GetMatches(lexemeRx, input)) {
			if (m.Index != lastPosition) {
				throw new LexerException($"Нераспознанный токен после подстроки \"{input[..lastPosition]}\": {input[lastPosition..m.Index]}");
			}
			if (m.Groups["spaces"].Success) {
				yield return new Token(TokenType.Spaces, m.Value);
			}
			else if (m.Groups["identifier"].Success) {
				yield return new Token(TokenType.Identifier, m.Value);
			}
			else if (m.Groups["number"].Success) {
				yield return new Token(TokenType.Number, m.Value);
			}
			else if (m.Groups["punctuator"].Success) {
				yield return new Token(TokenType.Punctuator, m.Value);
			}
			else {
				throw new LexerException("Получена некорректная строка");
			}
			lastPosition += m.Length;
		}
	}
}
