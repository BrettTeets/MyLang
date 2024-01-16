

public class IdentifierNode: ASTNode{
    public IdentifierNode(string s){
        id = s;
    }
    string id;

    public override string ToString()
    {
        return id;
    }
}

