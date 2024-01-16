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

