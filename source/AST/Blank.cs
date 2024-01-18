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

public class Error: ASTNode{
    public Error(string s = "err"){
        message = s;
    }

    string message;

    public override string ToString()
    {
        return $"{message}";
    }
}

