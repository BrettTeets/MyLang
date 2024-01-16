public class Statement: ASTNode{
    public Statement(ASTNode c){
        child = c;
    }

    ASTNode child;

    public override string ToString()
    {
        return $"{child}";
    }
}

