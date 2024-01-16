public class AmbigiousExpressionListNode : ASTNode{
    public AmbigiousExpressionListNode(ASTNode l, ASTNode r){
        left = l;
        right = r;
    }

    ASTNode left;
    ASTNode right;

    public override string ToString()
    {
        return $"{left}, {right} ";
    }
}


