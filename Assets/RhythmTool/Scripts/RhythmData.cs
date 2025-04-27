using System.Collections.Generic;

[System.Serializable]
public class Note
{
    public float start;
    public float end;
}

[System.Serializable]
public class SongData
{
    public string title;
    public float tempo;
    public Dictionary<string, List<Note>> notes;
}
