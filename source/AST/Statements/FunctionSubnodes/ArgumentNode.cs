public class ArgumentNode : ASTNode{
    public ArgumentNode(ASTNode t, ASTNode i){
        type = t;
        id = i;
    }

    ASTNode type;
    ASTNode id;

    public override string ToString()
    {
        return $"{type} {id}";
    }

}


