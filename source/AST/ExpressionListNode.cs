

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

public class ArgumentListNode : ASTNode{
    public ArgumentListNode(ASTNode l, ASTNode r){
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

public class ReturnStatement : ASTNode{
    public ReturnStatement(ASTNode e){
        expressions = e;
    }

    ASTNode expressions;

    public override string ToString()
    {
        return $"(return {expressions}";
    }
}

public class FuncDeclarationStatement : ASTNode{
    public FuncDeclarationStatement(ASTNode a, ASTNode i, ASTNode r,  ASTNode p){
        args = a;
        id = i;
        returns = r;
        program = p;
    }

    ASTNode args;
    ASTNode id;
    ASTNode returns;
    ASTNode program;

    public override string ToString()
    {
        return $"(func {args} -> {id} = {returns} {{{program})}}";
    }

}

public class ArgumentNode : ASTNode{
    public ArgumentNode(ASTNode t, ASTNode i){
        type = t;
        id = i;
    }

    ASTNode type;
    ASTNode id;

    public override string ToString()
    {
        return $"({type} {id}) ";
    }

}


