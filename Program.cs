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

        using (StreamReader reader = new StreamReader("VividMath.vd")){
            foreach (Token token in l.Lex(reader))
            {
                Console.WriteLine($"type: {token.type} = {token.text}");
            }
        }
    }
}
