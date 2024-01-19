public partial class Parser{
    public ASTNode ParseExpressions(bool flow = false){
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
