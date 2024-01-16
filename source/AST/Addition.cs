public class AdditionNode: ASTNode{
    public AdditionNode(ASTNode left, ASTNode right){
        lhs = left;
        rhs = right;
    }

    ASTNode lhs;
    ASTNode rhs;

    public override string ToString()
    {
        return $"{lhs} + {rhs}";
    }
}


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

public class Blank: ASTNode{
    public Blank(){
    }

    public override string ToString()
    {
        return $"EOF";
    }
}

