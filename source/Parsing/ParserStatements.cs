public partial class Parser{
    private ASTNode ParseLetStatement(){
        Expect(Token_Type.KeywordLet);
        ASTNode type = ParseTypeID();
        ASTNode id = ParseIdentifier();

        Expect(Token_Type.Assignment);

        ASTNode expr = ParseExpressions();
        Expect(Token_Type.EndStatement);
        return new LetDeclarationStatement(type, id, expr);
    }
}