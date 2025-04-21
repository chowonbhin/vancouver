using System.Collections;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine.Pool;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UI;

namespace HP
{
    public class RhythmManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public TextAsset jsonFile;
        public float judgeWindow = 0.2f;
        public float noteDuration = 1f;


        private SongData songData;
        private int currentNoteIndex = 0;
        private int noteObjectIndex = 0;

        [SerializeField]
        Text songText;
        [SerializeField]
        NoteUI noteUIPref;
        [SerializeField]
        RectTransform gameUI;
        List<NoteUI> noteUIList;
        private IObjectPool<NoteUI> objectPool;
        List<Note> noteList;
        void Start()
        {
            LoadSongData();
            StartCoroutine(PlaySongAfterDelay(1.0f));
            noteUIList = new List<NoteUI>();
            objectPool = new UnityEngine.Pool.ObjectPool<NoteUI>(
            createFunc: () =>
            {
                var note = Instantiate(noteUIPref, gameUI, false).GetComponent<NoteUI>();
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


        void UpdateNoteUI(float currentTime)
        {
            if (noteObjectIndex < noteList.Count)
            {
                Note noteObj = noteList[noteObjectIndex];
                while (currentTime > noteObj.start - noteDuration / 2)
                {
                    var noteui = objectPool.Get().GetComponent<NoteUI>();
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

            var removeList = new  List<NoteUI>();
            foreach (var noteUI in noteUIList)
            {
                bool isOver = noteUI.UpdatePos(currentTime);
                if(noteUI.note.end + judgeWindow < currentTime)
                {
                    if(noteUI.state == NoteUI.State.normal)
                    {
                        noteUI.SwitchState(NoteUI.State.fail);
                        if (currentNoteIndex < noteList.Count)
                        {
                            currentNoteIndex++;
                        }
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

        void CheckUserInput(float currentTime)
        {
            if (currentNoteIndex < noteList.Count)
            {
                Note currentNote = noteList[currentNoteIndex];
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    float judgeStart = currentNote.start - judgeWindow;
                    float judgeEnd = currentNote.end + judgeWindow;
                    if(currentTime >= judgeStart && currentTime <= judgeEnd)
                    {
                        Debug.Log($"Good! 🎵");
                        foreach (var noteUI in noteUIList)
                        {
                            if (noteUI.noteIndex == currentNoteIndex)
                            {
                                if (noteUI.state == NoteUI.State.normal)
                                {
                                    noteUI.SwitchState(NoteUI.State.success);
                                    if (currentNoteIndex < noteList.Count)
                                    {
                                        currentNoteIndex++;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log($"Miss 😢");
                    }
                }
            }
        }

        void Update()
        {          
            float currentTime = audioSource.time;
            UpdateNoteUI(currentTime);
            CheckUserInput(currentTime);
        }   

        void LoadSongData()
        {
            songData = JsonConvert.DeserializeObject<SongData>(jsonFile.text);
            noteList = songData.notes.Values.ElementAt(1);
        }

        IEnumerator PlaySongAfterDelay(float delay)
        {
            songText.text = songData.title;
            noteObjectIndex = 0;
            currentNoteIndex = 0;
            yield return new WaitForSeconds(delay);
            audioSource.Play();
            Debug.Log("노래 재생 시작 🎶");
        }
    }
}