public class StatementListNode : ASTNode{
    public StatementListNode(ASTNode l, ASTNode r){
        left = l;
        right = r;
    }

    ASTNode left;
    ASTNode right;

    public override string ToString()
    {
        return $"{left} {right}";
    }
}

public class ProgramListNode : ASTNode{
    public ProgramListNode(ASTNode l, ASTNode r){
        left = l;
        right = r;
    }

    ASTNode left;
    ASTNode right;

    public override string ToString()
    {
        return $"{left} {right}";
    }
}


