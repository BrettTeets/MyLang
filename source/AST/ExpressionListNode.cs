

public class AmbigiousExpressionListNode : ASTNode{
    public AmbigiousExpressionListNode(ASTNode l, ASTNode r){
        left = l;
        right = r;
    }

    ASTNode left;
    ASTNode right;

    public override string ToString()
    {
        return $"({left}), ({right}) ";
    }
}

public class StatementListNode : ASTNode{
    public StatementListNode(ASTNode l, ASTNode r){
        left = l;
        right = r;
    }

    ASTNode left;
    ASTNode right;

    public override string ToString()
    {
        return $"({left}), ({right}) ";
    }
}

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

