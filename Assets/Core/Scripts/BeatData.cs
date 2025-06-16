using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BeatData
{
    public string songName;
    public float bpm;
    public List<Tuple<float, bool>> beatTimes = new List<Tuple<float, bool>>();
    
    // 미디 데이터에서 비트 시간 추출
    public static BeatData FromMidiJson(TextAsset jsonFile)
    {
        try
        {
            // JSON 문자열에서 비트 타이밍 추출
            MidiData midiData = JsonUtility.FromJson<MidiData>(jsonFile.text);
            
            BeatData beatData = new BeatData();
            beatData.songName = jsonFile.name;
            
            // BPM 설정
            if (midiData.tempo != null && midiData.tempo.Count > 0)
            {
                beatData.bpm = midiData.tempo[0].bpm;
            }
            
            // 드럼 비트 트랙 찾기
            MidiTrack beatTrack = null;
            foreach (var track in midiData.tracks)
            {
                if (track.name == "BASS_SWING" || track.id == 1)
                {
                    beatTrack = track;
                    break;
                }

                if (track.name == "Drum" || track.id == 1)
                {
                    beatTrack = track;
                    break;
                }

                if (track.name == "DRUM_BASS" || track.id == 1)
                {
                    beatTrack = track;
                    break;
                }
            }
            
            // 비트 시간 추출
            if (beatTrack != null && beatTrack.notes != null)
            {
                if(beatTrack.name == "BASS_SWING")
                {
                    foreach (var note in beatTrack.notes)
                    {
                        beatData.beatTimes.Add(new Tuple<float, bool>(note.time, false));
                    }
                }

                if(beatTrack.name == "Drum" || beatTrack.id == 1)
                {
                    foreach (var note in beatTrack.notes)
                    {
                        if(note.name == "F#2"){
                            beatData.beatTimes.Add(new Tuple<float, bool>(note.time, false));
                        }
                    }
                }

                if(beatTrack.name == "DRUM_BASS" || beatTrack.id == 1)
                {
                    foreach (var note in beatTrack.notes)
                    {
                        if(note.name == "D3"){
                            beatData.beatTimes.Add(new Tuple<float, bool>(note.time, true));
                        }

                        if(note.name == "A2"){
                            beatData.beatTimes.Add(new Tuple<float, bool>(note.time, false));
                        }
                    }
                }
            }
            
            return beatData;
        }
        catch (Exception e)
        {
            Debug.LogError($"비트맵 파싱 오류: {e.Message}");
            return new BeatData();
        }
    }
}

// JSON 파싱을 위한 구조체들 (MidiData.cs에서 가져옴)
[Serializable]
public class MidiData
{
    public MidiHeader header;
    public List<TempoEvent> tempo;
    public List<TimeSignatureEvent> timeSignature;
    public float startTime;
    public float duration;
    public List<MidiTrack> tracks;
}

[Serializable]
public class MidiHeader
{
    public int PPQ;
    public List<int> timeSignature;
    public float bpm;
    public string name;
}

[Serializable]
public class TempoEvent
{
    public float absoluteTime;
    public float seconds;
    public float bpm;
}

[Serializable]
public class TimeSignatureEvent
{
    public float absoluteTime;
    public float seconds;
    public int numerator;
    public int denominator;
    public int click;
    public int notesQ;
}

[Serializable]
public class MidiTrack
{
    public float startTime;
    public float duration;
    public int length;
    public List<MidiNote> notes;
    public int id;
    public string name;
    public int channelNumber;
    public bool isPercussion;
}

[Serializable]
public class MidiNote
{
    public string name;
    public int midi;
    public float time;
    public float velocity;
    public float duration;
}