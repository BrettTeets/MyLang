public class EqualityNode: ASTNode{
    public EqualityNode(ASTNode left, ASTNode right){
        lhs = left;
        rhs = right;
    }

    ASTNode lhs;
    ASTNode rhs;

    public override string ToString()
    {
        return $"{lhs} == {rhs}";
    }
}

