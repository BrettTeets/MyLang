using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

internal class Program
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

        Parser pare = new Parser(tokens, stoks);

        Console.WriteLine(pare.root.ToString());

    }
}
