﻿internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Lexer l = new();

        List<Token> tokens = new();
        List<Token> stoks = new();

        using (StreamReader reader = new StreamReader("sample.vd")){
            (List<Token> AllTokens, List<Token> StructureTokens) o = l.Lex2(reader);
            tokens = o.AllTokens;
            stoks = o.StructureTokens;
            foreach (Token token in o.AllTokens)
            {
                Console.WriteLine($"type: {token.type} = {token.text}");
            }
        }

        Parser pare = new Parser();
        ASTNode n = pare.Parse(tokens, stoks);

        Console.WriteLine(n.ToString());

    }
}
