public partial class Parser{
    private bool NextTokenIsStatement(){

        switch (PeekAtType(0)){
            case Token_Type.KeywordLet:
                return true;
            case Token_Type.KeywordVar:
                return true;
            case Token_Type.KeywordReturn:
                return true;
            case Token_Type.NoFlowOperator:
                return true;
            case Token_Type.Identifier:
                //this is something like "x |= expr;"
                if(PeekAtType(1) == Token_Type.Reassignment) return true;
                return false;
            default:
                return false;
        }
    }

    private bool NextTokenIsTypedVar(){
        //This must be type var declaration, func name -> type var { and func name { would not have two identifiers back to back.
        return  PeekAtType(0) == Token_Type.Identifier && 
                (PeekAtType(1) == Token_Type.Identifier || PeekAtType(1) == Token_Type.OpenBox);
    }

    private bool IsNearAFlowButNextTokenIsNotAFlow(){
        return StructurePeek(0)?.type == Token_Type.FlowOperator && Peek(0)?.type != Token_Type.FlowOperator;
    }
}