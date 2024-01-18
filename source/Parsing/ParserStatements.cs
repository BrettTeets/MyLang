public partial class Parser{
    private ASTNode ParseLetStatement(){
        ASTNode? error = null;

        Expect(Token_Type.KeywordLet);
        ASTNode type = ParseTypeID();
        ASTNode id = ParseIdentifier();

        if(Expect(Token_Type.Assignment) is false) error = new Error("Expected an Assignment.");

        ASTNode expr = ParseExpressions();
        Expect(Token_Type.EndStatement);
        
        if(error is not null) {
            return error;
        }
        else{
            return new LetDeclarationStatement(type, id, expr);
        }
       
    }

    private ASTNode ParseVarStatement(){
        ASTNode? error = null;

        Expect(Token_Type.KeywordVar);
        ASTNode type = ParseTypeID();
        ASTNode id = ParseIdentifier();

        if(Expect(Token_Type.Assignment) is false) error = new Error("Expected an Assignment.");

        ASTNode expr = ParseExpressions();
        if(Expect(Token_Type.EndStatement) is false) error = new Error("Expected a ;");
    
        if(error is not null) {
            return error;
        }
        else{
            return new VariableDeclarationStatement(type, id, expr);
        }
    }

    private ASTNode ParseReturnStatement(){
        Expect(Token_Type.KeywordReturn);
        ASTNode e = ParseExpressions();
        Expect(Token_Type.EndStatement);
        return new ReturnStatement(e);
    }

    private ASTNode ParseSetStatement(){
        ASTNode id = ParseIdentifier();

        Expect(Token_Type.Reassignment);

        ASTNode e = ParseExpressions();

        Expect(Token_Type.EndStatement);
        return new SetStatementNode(id, e);
    }

    private ASTNode ParseDoStatement(){
        Expect(Token_Type.NoFlowOperator);

        ASTNode e = ParseExpressions();

        Expect(Token_Type.EndStatement);
        return new DoStatementNode(e);
    }
}