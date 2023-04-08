using System.IO;
sealed class DebugPrintingVisitor : INodeVisitor {
	readonly TextWriter output;
	int indent;
	public DebugPrintingVisitor(TextWriter output, int indent = 0) {
		this.output = output;
		this.indent = indent;
	}
	public void WriteLine(INode node) {
		WriteNode(node);
		Write("\n");
	}
	void WriteNode(INode node) {
		node.AcceptVisitor(this);
	}
	void Write(string s) {
		output.Write(s);
	}
	void WriteIndent() {
		Write(new string(' ', 4 * indent));
	}
	void INodeVisitor.VisitInsert(Insert insert) {
		WriteIndent();
		Write($"new {nameof(Insert)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(insert.TableName);
			Write(",\n");
			WriteIndent();
			WriteNode(insert.Row);
			Write(",\n");
			WriteIndent();
			Write(insert.LimitIsAll ? "true" : "false");
			Write(",\n");
			WriteIndent();
			if (insert.Limit != null) {
				WriteNode(insert.Limit);
			}
			else {
				Write("null");
			}
			Write("\n");
			indent -= 1;
			WriteIndent();
		}
		Write(")");
	}
	void INodeVisitor.VisitParenthesis(Parenthesis parenthesis) {
		Write($"new {nameof(Parenthesis)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(parenthesis.Expression);
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitBinaryOperation(BinaryOperation binaryOperation) {
		Write($"new {nameof(BinaryOperation)}(\n");
		{
			indent += 1;
			WriteIndent();
			WriteNode(binaryOperation.Left);
			Write(",\n");
			WriteIndent();
			Write($"{nameof(BinaryOperationType)}.{binaryOperation.Operator}");
			Write(",\n");
			WriteIndent();
			WriteNode(binaryOperation.Right);
			Write("\n");
			indent -= 1;
		}
		WriteIndent();
		Write(")");
	}
	void INodeVisitor.VisitIdentifier(Identifier identifier) {
		Write($"new {nameof(Identifier)}(\"{identifier.Lexeme}\")");
	}
	void INodeVisitor.VisitNumber(Number number) {
		Write($"new {nameof(Number)}(\"{number.Lexeme}\")");
	}
}
