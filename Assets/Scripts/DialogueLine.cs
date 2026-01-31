using UnityEngine;

public struct DialogueLine
{
    public Sprite portrait; 
    public string speakerName;
    [TextArea(2, 6)]
    public string text;
}
