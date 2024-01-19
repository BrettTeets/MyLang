public partial class Parser{
    
    //Top level grammar rule, also runs inside of code blocks
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
        if(PeekAtType(0) == Token_Type.KeywordLet){
            return ParseLetStatement();
        }
        else if(PeekAtType(0) == Token_Type.KeywordVar){
            return ParseVarStatement();
        }
        else if(PeekAtType(0) == Token_Type.KeywordReturn){
            return ParseReturnStatement();
        }
        else if(PeekAtType(0) == Token_Type.NoFlowOperator){
            return ParseDoStatement();
        }
        else if(PeekAtType(1) == Token_Type.Reassignment){
            return ParseSetStatement();
        }
        else{
            return new Error("Expected a Statement");
        }
    }

    private ASTNode ParseArgument(){
        ASTNode t = ParseTypeID();
        ASTNode i = ParseIdentifier();
        return new ArgumentNode(t, i);

    }
}
