using System.Diagnostics;
using System.Security.Cryptography;

public class Lexer{
    public IEnumerable<Token> Lex(StreamReader stream){
        bool CommentMode = false;
        bool StringMode = false;

        Token token = new();

        while(stream.EndOfStream is false){
            
            if(stream.Peek() != -1){
                char c = (char)stream.Read();
                char p = (char)stream.Peek();

                //Once you are in a comment mode the only thing that can get you out is new line.
                if(CommentMode){
                    if(c == '\n') {
                        CommentMode = false; 
                        continue;
                    }
                    else {
                        continue;
                    }
                }
                else{
                    //If you are not in comment mode or string mode and you read the comment characters enter comment mode.
                    if(StringMode is false && c == '/' && p == '/'){
                        stream.Read(); //consume that / you just peeked at.
                        CommentMode = true; 
                        continue;
                    }
                }

                if(StringMode){
                    if(c == '\\' && p == '"'){ 
                        token.text += p; stream.Read(); //consume p, but discard c;
                        continue;
                    }
                    if(c == '"'){
                        token.text += c;
                        StringMode = false;
                        yield return ParseString(token); token = new(); 
                        continue;
                    }
                    else{
                        token.text += c;
                        continue;
                    }
                }
                else{
                    if(c == '"'){
                        if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                        StringMode = true;
                        token.text += c;
                    }
                    else if(c == '$' && p == '"'){
                        if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                        StringMode = true;
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                    }
                    else if(c == '@' && p == '"'){
                        if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                        StringMode = true;
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                    }
                }

                if(char.IsWhiteSpace(c)){
                    if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}
                }

                if(token.IsDigit()){
                    if((c == '.' || c == 'x' || c == 'b' || c == '_') && char.IsDigit(p)){
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                        continue;
                    }
                }

                if(IsBlockSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                    token.text += c;
                    yield return ParseBlock(token); token = new();
                    continue;
                }

                if(IsPunctuationSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                    token.text += c;
                    yield return ParsePunctuation(token); token = new();
                    continue;
                }

                if(IsOperatorSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {yield return ParseToken(token); token = new();}

                    token.text += c;
                    if(IsOperatorSymbol(p)){
                        token.text += p; stream.Read();
                    }

                    yield return ParseOperator(token); token = new();
                    continue;
                }

                if(char.IsLetterOrDigit(c)){
                    token.text += c;
                    continue;
                }
            }
            else if(token.text.Length > 0){
                yield return ParseToken(token);
            }
        }
    

    }

    public (List<Token> AllTokens, List<Token> StructureTokens) Lex2(StreamReader stream){
        bool CommentMode = false;
        bool StringMode = false;

        List<Token> allTokens = new();
        List<Token> structureTokens = new();

        Token token = new();

        while(stream.EndOfStream is false){
            
            if(stream.Peek() != -1){
                char c = (char)stream.Read();
                char p = (char)stream.Peek();

                //Once you are in a comment mode the only thing that can get you out is new line.
                if(CommentMode){
                    if(c == '\n') {
                        CommentMode = false; 
                        continue;
                    }
                    else {
                        continue;
                    }
                }
                else{
                    //If you are not in comment mode or string mode and you read the comment characters enter comment mode.
                    if(StringMode is false && c == '/' && p == '/'){
                        stream.Read(); //consume that / you just peeked at.
                        CommentMode = true; 
                        continue;
                    }
                }

                if(StringMode){
                    if(c == '\\' && p == '"'){ 
                        token.text += p; stream.Read(); //consume p, but discard c;
                        continue;
                    }
                    if(c == '"'){
                        token.text += c;
                        StringMode = false;
                        allTokens.Add(ParseString(token)); token = new(); 
                        continue;
                    }
                    else{
                        token.text += c;
                        continue;
                    }
                }
                else{
                    if(c == '"'){
                        if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                        StringMode = true;
                        token.text += c;
                    }
                    else if(c == '$' && p == '"'){
                        if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                        StringMode = true;
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                    }
                    else if(c == '@' && p == '"'){
                        if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                        StringMode = true;
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                    }
                }

                if(char.IsWhiteSpace(c)){
                    if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}
                }

                if(token.IsDigit()){
                    if((c == '.' || c == 'x' || c == 'b' || c == '_') && char.IsDigit(p)){
                        token.text += c;
                        token.text += p; stream.Read(); //consume p.
                        continue;
                    }
                }

                if(IsBlockSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                    token.text += c;
                    Token t = ParseBlock(token);
                    allTokens.Add(t); 
                    if(t.type != Token_Type.OpenBox && t.type != Token_Type.ClosedBox){
                        structureTokens.Add(t);
                    }
                    
                    token = new();
                    continue;
                }

                if(IsPunctuationSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                    token.text += c;
                    Token t = ParsePunctuation(token);
                    allTokens.Add(t); 
                    //structureTokens.Add(t);
                    token = new();
                    continue;
                }

                if(IsOperatorSymbol(c)){
                    if(token.IsTokenBeingBuilt()) {allTokens.Add(ParseToken(token)); token = new();}

                    token.text += c;
                    if(IsOperatorSymbol(p)){
                        token.text += p; stream.Read();
                    }

                    Token t = ParseOperator(token);
                    allTokens.Add(t); 
                    if(t.type == Token_Type.FlowOperator) structureTokens.Add(t);
                    token = new();
                    continue;
                }

                if(char.IsLetterOrDigit(c)){
                    token.text += c;
                    continue;
                }
            }
            else if(token.text.Length > 0){
                allTokens.Add(ParseToken(token));
            }
        }
    
        return (allTokens, structureTokens);

    }

    private Token ParseToken(Token token){
        if(token.IsDigit()){
            if(token.IsHex()) token.type = Token_Type.Hexidecimal;
            else if(token.IsBinary()) token.type = Token_Type.Binary;
            else if(token.IsFloat()) token.type = Token_Type.Float;
            else {token.type = Token_Type.Integer;}
        }
        else{
            if(IsKeyword(token.text)){
                token = ParseKeyword(token);
            }
            else{
                token.type = Token_Type.Identifier;
            }
        }
        return token;
    }

    private Token ParseKeyword(Token token){
        token.type = token.text switch{
            "let" => Token_Type.KeywordLet,
            "var" => Token_Type.KeywordVar,
            "func" => Token_Type.KeywordFunc,
            "return" => Token_Type.KeywordReturn,
            _ => Token_Type.Keyword,
        };
        return token;
    }

    private Token ParseString(Token token){
        token.type = Token_Type.String;
        //TODO distinish interpolated string from exact string from normal strings.
        return token;
    }

    private Token ParseBlock(Token token){
        token.type = token.text[0] switch{
            '{' => Token_Type.OpenBrace,
            '}' => Token_Type.ClosedBrace,
            '(' => Token_Type.OpenParen,
            ')' => Token_Type.ClosedParen,
            '[' => Token_Type.OpenBox,
            ']' => Token_Type.ClosedBox,
            _ => throw new Exception(),
        };
        return token;
    }

    private Token ParsePunctuation(Token token){
        token.type = token.text switch{
            "," => Token_Type.Seperator,
            ";" => Token_Type.EndStatement,
            _ => Token_Type.Error,
        };

        return token;
    }

    private Token ParseOperator(Token token){
        //TODO distingish between mathmatical and logical operators.
        token.type = token.text switch {
            "==" => Token_Type.Equality,
            "+" => Token_Type.Addition,
            "-" => Token_Type.Subtraction,
            "*" => Token_Type.Multiplication,
            "/" => Token_Type.Division,
            "%" => Token_Type.Modulus,
            "^" => Token_Type.Exponention,
            "->" => Token_Type.FlowOperator,
            "|>" => Token_Type.NoFlowOperator,
            "||" => Token_Type.LogicalOR,
            "&&" => Token_Type.LogicalAnd,
            ":=" => Token_Type.Assignment,
            "|=" => Token_Type.Reassignment,
            "=>" => Token_Type.DefiningOperator,
            "<" => Token_Type.LessThan,
            ">" => Token_Type.GreaterThan,
            _ => Token_Type.Error,
        };
        return token;
    }

    private bool IsPunctuationSymbol(char c){
        return c switch{
            ',' => true,
            ';' => true,

            '.' => true,
            '?' => true,
            '\\' => true,
            '#' => true,
            '$' => true,
            '@' => true,
            _ => false,
        };
    }

    private bool IsOperatorSymbol(char c){
        return c switch{
            '+' => true,
            '-' => true,
            '*' => true,
            '/' => true,
            '%' => true,
            '=' => true,
            '<' => true,
            '>' => true,
            '&' => true,
            '|' => true,
            '^' => true,
            '!' => true,
            ':' => true,
            _ => false,
        };
    }

    private bool IsBlockSymbol(char c){
        return c switch{
            '{' => true,
            '}' => true,
            '(' => true,
            ')' => true,
            '[' => true,
            ']' => true,
            _ => false,
        };
    }

    private bool IsKeyword(string text){
        return text switch{
            "func" => true,
            "return" => true,
            "var" => true,
            "let" => true,

            "continue" => true,
            "break" => true,
            "class" => true,
            "for" => true,
            "foreach" => true,
            "while" => true,
            "do" => true,
            "switch" => true,
            "yeet" => true,
            "yield" => true,
            "property" => true,
            _ => false,
        };
    }
}
