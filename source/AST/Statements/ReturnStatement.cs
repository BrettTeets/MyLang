public class ReturnStatement : ASTNode{
    public ReturnStatement(ASTNode e){
        expressions = e;
    }

    ASTNode expressions;

    public override string ToString()
    {
        return $"return {expressions};";
    }
}


