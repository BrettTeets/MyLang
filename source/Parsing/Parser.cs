using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

public partial class Parser
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

    //Called at the top level to get this started and inside functions.
    public ASTNode ParseProgram(){
        ASTNode a = new Blank();

        if(PeekAtType(0) == Token_Type.KeywordFunc){
            a = ParseFunctionDeclaration();
            GetNextProgram();
        }
        else if(NextTokenIsStatement()){
            a = ParseStatement();
            GetNextProgram();
        }
        else{
            return a;
        }

        void GetNextProgram(){
            if(Peek(0) is not null){
                if(PeekAtType(0) == Token_Type.KeywordFunc || NextTokenIsStatement()){
                    a = new ProgramListNode(a, ParseProgram());
                }
            }
        }

        return a;
    }

    private ASTNode ParseFunctionDeclaration(){
        Expect(Token_Type.KeywordFunc);

        ASTNode args = new Blank("NOARGS");
        ASTNode func;
        ASTNode output = new Blank("NORETURN");
        ASTNode body;

        if(NextTokenIsTypedVar()){
            args = ParseArgumentsInFuncDecl();
            Expect(Token_Type.FlowOperator);
        }

        func = ParseIdentifier();
        
        if(Peek(0)?.type == Token_Type.FlowOperator){
            Expect(Token_Type.FlowOperator);
            output = ParseReturnsInFuncDecl();
        }

        Expect(Token_Type.OpenBrace);
        body = ParseProgram();
        Expect(Token_Type.ClosedBrace);

        return new FuncDeclarationStatement(args, func, output, body);
    }

    //This needs to confirm type x, type x, ... ends in a -> using the structure list.
    private ASTNode ParseArgumentsInFuncDecl(){
        if(IsNearAFlowButNextTokenIsNotAFlow()){
            ASTNode a = ParseArgument();
            while(true){
                if(Peek(0)?.type == Token_Type.Seperator){
                    Expect(Token_Type.Seperator);
                    a = new ArgumentListNode(a, ParseArgumentsInFuncDecl());
                }
                else{
                    return a;
                }
            }
        }
        else{
            return new Error("Expected a ->");
        }
    }

    //This can be simpler because  its only getting called after "funcName ->"
    private ASTNode ParseReturnsInFuncDecl(){
        ASTNode a = ParseArgument();
        while(true){
            if(Peek(0)?.type == Token_Type.Seperator){
                Expect(Token_Type.Seperator);
                a = new ArgumentListNode(a, ParseReturnsInFuncDecl());
            }
            else{
                return a;
            }
        }
    }

    public ASTNode ParseStatement(){
        if(Peek(0)?.type == Token_Type.KeywordLet){
            return ParseLetStatement();
        }
        else if(Peek(0)?.type == Token_Type.KeywordVar){
            Expect(Token_Type.KeywordVar);
            ASTNode t = ParseTypeID();
            ASTNode i = ParseIdentifier();

            Expect(Token_Type.Assignment);
            
            ASTNode e = ParseExpressions();
            Expect(Token_Type.EndStatement);
            return new VariableDeclarationStatement(t, i, e);
        }
        else if(Peek(0)?.type == Token_Type.KeywordReturn){
            Expect(Token_Type.KeywordReturn);
            ASTNode e = ParseExpressions();
            Expect(Token_Type.EndStatement);
            return new ReturnStatement(e);
        }
        else{
            return new Blank();
        }
    }

    

    

    

    private ASTNode ParseArgument(){
        ASTNode t = ParseTypeID();
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
}
