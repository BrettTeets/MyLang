public partial class Parser{
    private ASTNode ParseIdentifier(){
        string s = Peek(0)?.text ?? "";
        if(Expect(Token_Type.Identifier)){
            return new IdentifierNode(s);
        }
        else{
            return new Error("Expected a identifier");
        }
    }

    private ASTNode ParseTypeID(){
        if(PeekAtType(0) is not Token_Type.Identifier && 
            (PeekAtType(1) is not Token_Type.Identifier || 
                (PeekAtType(1) is not Token_Type.OpenBox && PeekAtType(3) is not Token_Type.Identifier))){
            return new Error("Expected a type and a name");
        }

        string s = Peek(0)?.text ?? "";
        Expect(Token_Type.Identifier);

        if(PeekAtType(0) == Token_Type.OpenBox && PeekAtType(1) == Token_Type.ClosedBox){
            Expect(Token_Type.OpenBox);
            Expect(Token_Type.ClosedBox);
            return new TypeIDNode(s, true);
        }
        else{
            return new TypeIDNode(s, false);
        }
    }
}