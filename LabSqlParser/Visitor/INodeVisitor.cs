interface INodeVisitor {
	void VisitInsert(Insert insert);
	void VisitParenthesis(Parenthesis parenthesis);
	void VisitBinaryOperation(BinaryOperation binaryOperation);
	void VisitIdentifier(Identifier identifier);
	void VisitNumber(Number number);
}
