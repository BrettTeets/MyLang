public class Blank: ASTNode{
    public Blank(string s = "EOF"){
        message = s;
    }

    string message;

    public override string ToString()
    {
        return $"{message}";
    }
}

