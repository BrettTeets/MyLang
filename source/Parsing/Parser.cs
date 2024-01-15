public class Parser
{
    List<Token> Tokens;
    public ASTNode root;

    public Parser(List<Token> tokens){
        Tokens = tokens;
        root = ParseExpression();
    }

    public Token? Peek(int k){
        if(Tokens.Count > k){
            return Tokens[k];
        }
        else{
            return null;
        }
        
    }

    public bool Expect(Token_Type t){
        if(Tokens[0].type == t){
            Tokens.RemoveAt(0);
            return true;
        }
        else{
            return false;
        }
    }

    public ASTNode ParseExpression(){
        //Token? t = Peek(0);

        ASTNode a = ParseTermMD();

        while(true){
            if(Peek(0)?.type == Token_Type.Addition){
                Expect(Token_Type.Addition);
                ASTNode b = ParseTermMD();
                a = new AdditionNode(a, b);
            }
            else if(Peek(0)?.type == Token_Type.Subtraction){
                Expect(Token_Type.Subtraction);
                ASTNode b = ParseTermMD();
                a = new SubtractionNode(a, b);
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseTermMD(){
        //Token? t = Peek(0);

        ASTNode a = ParseTermE();
        
        while(true){
            if(Peek(0)?.type == Token_Type.Multiplication){
                Expect(Token_Type.Multiplication);
                ASTNode b = ParseTermE();
                a = new MultiplicationNode(a, b);
            }
            else if(Peek(0)?.type == Token_Type.Division){
                Expect(Token_Type.Division);
                ASTNode b = ParseTermE();
                a = new MultiplicationNode(a, b);
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseTermE(){
        ASTNode a = ParseFactor();

        while(true){
            if(Peek(0)?.type == Token_Type.Exponention){
                Expect(Token_Type.Exponention);
                ASTNode b = ParseFactor();
                a = new ExponentionNode(a, b);
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseFactor(){
        //Token? t = Peek(0);

        if(Peek(0)?.type == Token_Type.Integer){
            string s = Peek(0)?.text ?? "";
            Expect(Token_Type.Integer);
            return new IntegerNode(s);
        }   
        else if(Peek(0)?.type == Token_Type.OpenParen){
            Expect(Token_Type.OpenParen);
            ASTNode n = ParseExpression();
            Expect(Token_Type.OpenParen);
            return n;
        }
        else{
            throw new Exception();
        }
    }

    
}
