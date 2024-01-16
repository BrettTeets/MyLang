

public class AmbigiousFuncNode : ASTNode{
    public AmbigiousFuncNode(ASTNode r){
        func = r;
    }
    ASTNode func;

    public override string ToString()
    {
        return $"-> {func}";
    }
}

