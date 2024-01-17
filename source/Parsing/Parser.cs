using System.Security.Cryptography.X509Certificates;

public class Parser
{
    List<Token> AllTokens = new();
    List<Token> StructureTokens = new();

    public ASTNode Parse(List<Token> allTokens, List<Token> structureTokens){
        AllTokens = allTokens;
        StructureTokens = structureTokens;
        return ParseProgram();
    }

    public Token? Peek(int k){
        if(AllTokens.Count > k){
            return AllTokens[k];
        }
        else{
            return null;
        }
    }

    public Token_Type? PeekAtType(int k){
        return Peek(k)?.type;
    }

    public Token? StructurePeek(int k){
        if(StructureTokens.Count > k){
            return StructureTokens[k];
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

    public ASTNode ParseProgram(){

        ASTNode a;

        if(PeekAtType(0) == Token_Type.KeywordFunc){
            a = ParseFunction();

            if(Peek(0) != null){
                a = new ProgramListNode(a, ParseProgram());
            }
        }
        else{
            a = ParseStatement();

            if(PeekAtType(0) == Token_Type.EndStatement){
                Expect(Token_Type.EndStatement);
                a = new ProgramListNode(a, ParseProgram());
            }
        }
        return a;
    }

    private ASTNode ParseFunction(){
        Expect(Token_Type.KeywordFunc);

        ASTNode args = new Blank("NOARGS");
        ASTNode func;

        //This must be type var declaration.
        if(PeekAtType(0) == Token_Type.Identifier && PeekAtType(1) == Token_Type.Identifier){
            args = ParseArguments();
            Expect(Token_Type.FlowOperator);
        }

        func = ParseIdentifier();

        ASTNode output = new Blank("NORETURN");
        if(Peek(0)?.type == Token_Type.FlowOperator){
            Expect(Token_Type.FlowOperator);
            output = ParseReturns();
        }

        Expect(Token_Type.OpenBrace);
        ASTNode body = ParseCode();
        Expect(Token_Type.ClosedBrace);
        
        return new FuncDeclarationStatement(args, func, output, body);
    }

    public ASTNode ParseCode(){
        ASTNode a = ParseStatement();
        
        while(true){
            if(PeekAtType(0) == Token_Type.EndStatement){
                Expect(Token_Type.EndStatement);
                a = new StatementListNode(a, ParseCode());
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
        else if(Peek(0)?.type == Token_Type.KeywordReturn){
            Expect(Token_Type.KeywordReturn);
            ASTNode e = ParseExpressions();
            return new ReturnStatement(e);
        }
        else{
            return new Blank();
        }
    }

    private ASTNode ParseArguments(){
        if(StructurePeek(0)?.type == Token_Type.FlowOperator && Peek(0)?.type != Token_Type.FlowOperator){
            ASTNode a = ParseArgument();
            while(true){
                if(Peek(0)?.type == Token_Type.Seperator){
                    Expect(Token_Type.Seperator);
                    a = new ArgumentListNode(a, ParseArguments());
                }
                else{
                    return a;
                }
            }
        }
        else{
            return new Blank();
        }
    }

    private ASTNode ParseReturns(){
        ASTNode a = ParseArgument();
        while(true){
            if(Peek(0)?.type == Token_Type.Seperator){
                Expect(Token_Type.Seperator);
                a = new ArgumentListNode(a, ParseReturns());
            }
            else{
                return a;
            }
        }
    }

    private ASTNode ParseArgument(){
        ASTNode t = ParseIdentifier();
        ASTNode i = ParseIdentifier();
        return new ArgumentNode(t, i);

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
                a = new DivisionNode(a, b);
            }
            else if(Peek(0)?.type == Token_Type.Modulus){
                Expect(Token_Type.Modulus);
                ASTNode b = ParseTermC();
                a = new ModulusNode(a, b);
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
