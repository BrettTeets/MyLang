

public class TypeIDNode: ASTNode{
    public TypeIDNode(string s, bool b){
        id = s;
        isArray = b;
    }
    string id;
    bool isArray;

    public override string ToString()
    {
        if(isArray is false){
            return id;
        }
        else{
            return $"{id}[]";
        }
    }
}

