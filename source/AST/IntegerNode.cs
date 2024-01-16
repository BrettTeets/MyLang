

public class IntegerNode: ASTNode{
    public IntegerNode(string s){
        i = int.Parse(s);
    }

    int i;

    public override string ToString()
    {
        return $"{i}";
    }
}

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

