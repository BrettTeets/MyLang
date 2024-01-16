public class LessThanNode: ASTNode{
    public LessThanNode(ASTNode left, ASTNode right){
        lhs = left;
        rhs = right;
    }

    ASTNode lhs;
    ASTNode rhs;

    public override string ToString()
    {
        return $"{lhs} < {rhs}";
    }
}

