public class Subtraction: ASTNode{
    public Subtraction(ASTNode left, ASTNode right){
        lhs = left;
        rhs = right;
    }

    ASTNode lhs;
    ASTNode rhs;

    public override string ToString()
    {
        return $"{lhs} - {rhs}";
    }
}

