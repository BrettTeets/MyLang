public class ModulusNode: ASTNode{
    public ModulusNode(ASTNode left, ASTNode right){
        lhs = left;
        rhs = right;
    }

    ASTNode lhs;
    ASTNode rhs;

    public override string ToString()
    {
        return $"{lhs} % {rhs}";
    }
}

