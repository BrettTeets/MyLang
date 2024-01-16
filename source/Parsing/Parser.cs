using System.Runtime.CompilerServices;

public class Parser
{
    List<Token> AllTokens;
    List<Token> StructureTokens;
    public ASTNode root;

    public Parser(List<Token> allTokens, List<Token> structureTokens){
        AllTokens = allTokens;
        StructureTokens = structureTokens;
        root = ParseProgram();
    }

    public Token? Peek(int k){
        if(AllTokens.Count > k){
            return AllTokens[k];
        }
        else{
            return null;
        }
    }

    public bool Expect(Token_Type t){
        Token_Type at = AllTokens[0].type;
        if(at == t){
            AllTokens.RemoveAt(0);
            if(StructurePeek(0)?.type == at )StructureTokens.RemoveAt(0);
            return true;
        }
        else{
            return false;
        }
    }

    public Token? StructurePeek(int k){
        if(StructureTokens.Count > k){
            return StructureTokens[k];
        }
        else{
            return null;
        }
    }

    public ASTNode ParseProgram(){
        ASTNode a = ParseStatement();
        
        while(true){
            if(Peek(0)?.type == Token_Type.EndStatement){
                Expect(Token_Type.EndStatement);
                a = new StatementListNode(a, ParseProgram());
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseStatement(){
        if(Peek(0)?.type == Token_Type.KeywordLet){
            Expect(Token_Type.KeywordLet);
            ASTNode t = ParseIdentifier();
            ASTNode i = ParseIdentifier();

            Expect(Token_Type.Assignment);

            ASTNode e = ParseExpressions();
            return new LetDeclarationStatement(t, i, e);
        }
        else if(Peek(0)?.type == Token_Type.KeywordVar){
            Expect(Token_Type.KeywordVar);
            ASTNode t = ParseIdentifier();
            ASTNode i = ParseIdentifier();

            Expect(Token_Type.Assignment);
            
            ASTNode e = ParseExpressions();
            return new VariableDeclarationStatement(t, i, e);
        }
        else{
            return new Blank();
        }
    }

    public ASTNode ParseExpressions(bool flow = false){
        //Token? t = Peek(0);

        ASTNode a = ParseExpression(flow);

        while(true){
            if(Peek(0)?.type == Token_Type.Seperator){
                Expect(Token_Type.Seperator);
                a = new AmbigiousExpressionListNode(a, ParseExpressions(false));
            }
            if(Peek(0)?.type == Token_Type.FlowOperator){
                Expect(Token_Type.FlowOperator);
                a = new AmbigiousExpressionListNode(a, ParseExpressions(true));
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseExpression(bool flow){
        if(flow is false){
            return ParseTermA();
        }
        else{
            Expect(Token_Type.FlowOperator);
            return new AmbigiousFuncNode(ParseIdentifier());
        }
    }

    public ASTNode ParseTermA(){
        ASTNode a = ParseTermM();

        while(true){
            if(Peek(0)?.type == Token_Type.Addition){
                Expect(Token_Type.Addition);
                ASTNode b = ParseTermM();
                a = new AdditionNode(a, b);
            }
            else if(Peek(0)?.type == Token_Type.Subtraction){
                Expect(Token_Type.Subtraction);
                ASTNode b = ParseTermM();
                a = new SubtractionNode(a, b);
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseTermM(){
        //Token? t = Peek(0);

        ASTNode a = ParseTermC();
        
        while(true){
            if(Peek(0)?.type == Token_Type.Multiplication){
                Expect(Token_Type.Multiplication);
                ASTNode b = ParseTermC();
                a = new MultiplicationNode(a, b);
            }
            else if(Peek(0)?.type == Token_Type.Division){
                Expect(Token_Type.Division);
                ASTNode b = ParseTermC();
                a = new MultiplicationNode(a, b);
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseTermC(){
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
            ASTNode n = ParseExpressions();
            Expect(Token_Type.OpenParen);
            return n;
        }
        if(Peek(0)?.type == Token_Type.Identifier){
            return ParseIdentifier();
        }
        else{
            Token? t = Peek(0);
            throw new Exception($"Error on {t} for a factor");
        }
    }

    private ASTNode ParseIdentifier(){
        string s = Peek(0)?.text ?? "";
        Expect(Token_Type.Identifier);
        return new IdentifierNode(s);
    }

    
}
