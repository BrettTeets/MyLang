

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

