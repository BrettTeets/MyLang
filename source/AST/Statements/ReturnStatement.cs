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

public class SetStatementNode : ASTNode{
    public SetStatementNode(ASTNode i, ASTNode e){
        id = i;
        expressions = e;
    }

    ASTNode id;
    ASTNode expressions;

    public override string ToString()
    {
        return $"{id} |= {expressions};";
    }
}

public class DoStatementNode : ASTNode{
    public DoStatementNode(ASTNode e){
        expressions = e;
    }

    ASTNode expressions;

    public override string ToString()
    {
        return $"|> {expressions};";
    }
}


