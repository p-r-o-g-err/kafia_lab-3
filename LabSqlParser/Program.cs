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
		var tree = new Insert(
			new Identifier("a"),
			new Parenthesis(
				new BinaryOperation(
					new BinaryOperation(
						new BinaryOperation(
							new BinaryOperation(
								new Number("1"),
								BinaryOperationType.Division,
								new Number("2")
							),
							BinaryOperationType.Add,
							new Number("3")
						),
						BinaryOperationType.And,
						new Number("4")
					),
					BinaryOperationType.Or,
					new Number("5")
				)
			),
			new Number("6")
		);
		Console.WriteLine($"Отформатированная строка: {tree.ToFormattedString()}");
	}
}
