public class Token{
    public Token_Type type = Token_Type.None;
    public string text = "";
    public int position = -1;

    public bool IsTokenBeingBuilt(){
        return text.Length > 0;
    }

    public bool IsDigit(){
        if(text.Length > 0){
            return char.IsDigit(text[0]);
        }
        else{
            return false;
        }
    }

    public bool IsHex(){
        if(text.Length > 2){
            return text[1] == 'x';
        }
        else{
            return false;
        }
    }

    public bool IsBinary(){
        if(text.Length > 2){
            return text[1] == 'b';
        }
        else{
            return false;
        }
    }

    public bool IsFloat(){
        foreach(char c in text){
            if(c == '.') return true;  
        }
        return false;
    }
}
