public class LetDeclarationStatement : ASTNode{
    public LetDeclarationStatement(ASTNode t, ASTNode i, ASTNode e){
        type = t;
        id = i;
        expression = e;
    }

    ASTNode type;
    ASTNode id;
    ASTNode expression;

    public override string ToString()
    {
        return $"(let {type} {id} = {expression}) ";
    }

}


