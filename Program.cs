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

        using (StreamReader reader = new StreamReader("sample.vd")){
            foreach (Token token in l.Lex(reader))
            {
                tokens.Add(token);
                Console.WriteLine($"type: {token.type} = {token.text}");
            }
        }

        Parser pare = new Parser(tokens);

        Console.WriteLine(pare.root.ToString());

    }
}
