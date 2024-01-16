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
        return $"func {args} -> {id} -> {returns} {{{program}}}";
    }

}


