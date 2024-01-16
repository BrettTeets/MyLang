public class VariableDeclarationStatement : ASTNode{
    public VariableDeclarationStatement(ASTNode t, ASTNode i, ASTNode e){
        type = t;
        id = i;
        expression = e;
    }

    ASTNode type;
    ASTNode id;
    ASTNode expression;

    public override string ToString()
    {
        return $"(var {type} {id} = {expression}) ";
    }

}


