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

    /// <summary>
    /// Consumes the next token of this type and gives a bool 
    /// </summary>
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
}
