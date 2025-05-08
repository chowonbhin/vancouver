using System.Collections;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Pool;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HP
{
    public class RhythmManager : MonoBehaviour
    {
        double songStartDspTime;
        public AudioSource audioSource;
        public AudioSource markSound;
        public TextAsset jsonFile;
        public float judgeWindow = 0.02f;
        public float noteDuration = 1f;

        private SongData songData;
        private int currentNoteIndex = 0;
        private int noteObjectIndex = 0;
        private int elementIndex = 0;

        [SerializeField]
        TMPro.TMP_Text songText;
        [SerializeField]
        NoteUI noteUIPref;
        [SerializeField]
        RectTransform NoteUITransform;
        List<NoteUI> noteUIList;
        private IObjectPool<NoteUI> objectPool;
        List<Note> noteList;
        void Start()
        {
            LoadSongData();
            PlaySongAfterDelay(1.0f);
            noteUIList = new List<NoteUI>();
            objectPool = new UnityEngine.Pool.ObjectPool<NoteUI>(createFunc: () =>
            {
                var note = Instantiate(noteUIPref, NoteUITransform, false).GetComponent<NoteUI>();
                note.gameObject.SetActive(false);
                return note;
            },
            actionOnGet: note =>
            {
                note.gameObject.SetActive(true);
            },
            actionOnRelease: note => note.gameObject.SetActive(false),
            actionOnDestroy: note => Destroy(note.gameObject),
            collectionCheck: false,
            defaultCapacity: 50);
        }

        void UpdateNoteUI(double currentTime)
        {
            // 노트를 추가할 시점에 맞춰 UI 업데이트
            if (noteObjectIndex < noteList.Count)
            {
                Note noteObj = noteList[noteObjectIndex];
                while (currentTime > noteObj.start - noteDuration / 2)
                {
                    var noteui = objectPool.Get();
                    noteui.Initialize(noteList[noteObjectIndex], noteDuration, noteObjectIndex);
                    noteUIList.Add(noteui);
                    noteObjectIndex++;
                    if (noteObjectIndex >= noteList.Count)
                    {
                        break;
                    }
                    noteObj = noteList[noteObjectIndex];
                }
            }

            // 노트 UI의 상태 업데이트
            var removeList = new List<NoteUI>();
            foreach (var noteUI in noteUIList)
            {
                bool isOver = noteUI.UpdatePos(currentTime);

                if (currentTime >= noteUI.note.start - judgeWindow &&
                            currentTime <= noteUI.note.start + judgeWindow)
                {

                    if (noteUI.state == NoteUI.State.normal)
                    {
                        noteUI.SwitchState(NoteUI.State.judging);    
                    }
                }


                // 노트가 실패 조건을 넘었을 때
                if (noteUI.note.start + judgeWindow < currentTime && noteUI.state != NoteUI.State.success && noteUI.state != NoteUI.State.fail)
                {

                    if (noteUI.state == NoteUI.State.normal)
                    {
                        Debug.LogWarning("Note Skipped!");
                    }

                    noteUI.SwitchState(NoteUI.State.fail);
                    if (currentNoteIndex < noteList.Count)
                    {
                        currentNoteIndex++;
                    }
                }

                if (isOver)
                {
                    removeList.Add(noteUI);
                    objectPool.Release(noteUI);
                }
            }

            foreach (var noteUI in removeList)
            {
                noteUIList.Remove(noteUI);
            }
        }

        void CheckUserInput(double currentTime)
        {
            var inputTime = currentTime;
            // 노트 판정
            if (currentNoteIndex < noteList.Count)
            {
                Note currentNote = noteList[currentNoteIndex];
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float judgeStart = currentNote.start - judgeWindow;
                    float judgeEnd = currentNote.start + judgeWindow;

                    // 입력이 판정 범위 내에 있는지 확인
                    if (inputTime > judgeStart && inputTime < judgeEnd)
                    {
                        Debug.Log($"Good! 🎵 {currentNote.start - inputTime}");

                        foreach (var noteUI in noteUIList)
                        {
                            if (noteUI.noteIndex == currentNoteIndex && noteUI.state == NoteUI.State.judging)
                            {
                                noteUI.SwitchState(NoteUI.State.success);
                                currentNoteIndex++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if( currentNoteIndex - 1>=0)
                        {
                            Note prevNote = noteList[currentNoteIndex-1];
                            Debug.Log($"Miss 😢 diff {currentNote.start - inputTime} {prevNote.start - inputTime}");
                        }
                    }
                }
            }
        }

        void Update()
        {
            if(audioSource.isPlaying)
            {
                int bufferLength;
                int numBuffers;
                AudioSettings.GetDSPBufferSize(out bufferLength, out numBuffers);
                double latency = (double)(bufferLength * numBuffers) / AudioSettings.outputSampleRate;
                
                // load/emit 두 번의 latency가 있을것으로 가정하고 작성. 보장할 수 없음으로 유저 입력으로 부터 calibration을 받는게 낫다. 
                double currentSongTime = AudioSettings.dspTime - songStartDspTime - 2 * latency;
                UpdateNoteUI(currentSongTime);
                CheckUserInput(currentSongTime);
            }
        }


        void LoadSongData()
        {
            songData = JsonConvert.DeserializeObject<SongData>(jsonFile.text);
            noteList = songData.notes.Values.ElementAt(elementIndex);
        }

        void PlaySongAfterDelay(float delay)
        {
            songText.text = songData.title;
            noteObjectIndex = 0;
            currentNoteIndex = 0;
            songStartDspTime = AudioSettings.dspTime + delay;
            audioSource.PlayScheduled(songStartDspTime);
            Debug.Log("노래 재생 시작 🎶");
        }
    }
}
