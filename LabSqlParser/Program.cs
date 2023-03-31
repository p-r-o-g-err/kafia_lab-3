using System;
using System.Collections.Generic;
using System.Linq;
namespace LabSqlParser;
static class Program {
	static void Main() {
		var inputStrings = new List<string>(){
			"INSERT INTO a VALUES ( ( 1 / 2 + 3 AND 4 OR 5 ) ) LIMIT 6",
			"INSERT INTO a VALUES ( 1 )",
			"INSERT INTO a VALUES ( 1 ) LIMIT ALL"
		};
		foreach (var inputString in inputStrings) {
			Console.WriteLine($"Входная строка: {inputString}");
			var tokens = Lexer.Lexer.GetTokens(inputString);
			Console.WriteLine("Результат работы лексера:");
			tokens?.ToList().ForEach(Console.WriteLine);
		}
	}
}
