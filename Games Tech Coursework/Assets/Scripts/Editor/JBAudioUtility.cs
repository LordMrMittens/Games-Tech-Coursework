using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[ExecuteAlways]

public class JBAudioUtility : EditorWindow
{
    public AudioClip[] audioClips;
    List<AudioClip> clips = new List<AudioClip>();
    public AudioClip clipToPlay;
    AudioSource audioSource;
    AudioSource musicObject;
    bool isPaused;
    float playTime;
    Vector2 scrollPos;
    [MenuItem("JB Tools/Audio Utility")]
    
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBAudioUtility));
    }
    private void OnEnable()
    {
        audioClips = Resources.LoadAll<AudioClip>("Audio");
        if (audioClips != null && audioClips.Length > 0)
        {
            foreach (var clip in audioClips)
            {
                if (!clips.Contains(clip))
                {
                    clips.Add(clip);
                }
            }
        }
    }
    void OnGUI()
    {
        scrollPos = GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.ExpandHeight(true));
        for (int i = 0; i < clips.Count; i++)
        {
            using (var vertical = new GUILayout.VerticalScope())
            {
                float playhead = 0;
            {
                if (audioSource != null)
                {
                    playhead = audioSource.time;
                }
            }
            clips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", clips[i], typeof(AudioClip), false, GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
                using (var horizontalScope = new GUILayout.HorizontalScope())
                {

                    if (audioSource != null && audioSource.clip == clips[i])
                    {
                        GUILayout.Label($"{ClipDuration(audioSource.time)} / {ClipDuration(clips[i].length)}");
                        audioSource.time = GUILayout.HorizontalSlider(playhead, 0, clips[i].length, GUILayout.MinWidth(50), GUILayout.MaxWidth(3000), GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
                    }
                    else
                    {
                        GUILayout.Label($"00:00/{ClipDuration(clips[i].length)}");
                        GUILayout.HorizontalSlider(0, 0, clips[i].length, GUILayout.MinWidth(50), GUILayout.MaxWidth(3000), GUILayout.MinHeight(20), GUILayout.MaxHeight(20));
                    }
                }
                using (var horizontalScope = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Remove"))
                {
                    clips.Remove(clips[i]);
                }
                    if (GUILayout.Button("Create Audio Object"))
                    {

                        musicObject = AssignClipToAudioSource("Audio  Object", musicObject, clips[i]);
                        CheckForAudioPreviewer();
                        Selection.activeGameObject = musicObject.gameObject;


                    }
                    if (GUILayout.Button("Add Audio to Selected Object"))
                    {
                        if (Selection.activeGameObject == null)
                        {
                            Debug.Log("You must select an object to assign an audio source");
                        }
                        else if (Selection.activeGameObject.GetComponent<AudioSource>())
                        {
                            Selection.activeGameObject.GetComponent<AudioSource>().clip = clips[i];
                            CheckForAudioPreviewer();
                        }
                        else
                        {
                            Selection.activeGameObject.AddComponent<AudioSource>().clip = clips[i];
                            CheckForAudioPreviewer(Selection.activeGameObject);
                        }
                    }
                    GUILayout.FlexibleSpace();
                if (GUILayout.Button("Play"))
                {
                    audioSource = AssignClipToAudioSource("Temp Audio Source", audioSource, clips[i]);
                    audioSource.Play();
                    isPaused = false;
                }
                if (GUILayout.Button("Pause"))
                {
                    if (audioSource != null)
                    {
                        if (isPaused) { 
                            audioSource.Play();
                            isPaused = false;
                        }
                        else
                        {
                            isPaused = true;
                            audioSource.Pause();
                        }
                    }

                }
                if (GUILayout.Button("Stop"))
                {
                    if (audioSource != null)
                    {
                        audioSource.Stop();
                        DestroyImmediate(audioSource.gameObject);
                    }

                }
            }
            }
            if (audioSource && !isPaused)
            {
                if (!audioSource.isPlaying)
                {
                    DestroyImmediate(audioSource.gameObject);
                }
            }
        }
        GUILayout.EndScrollView();
    }

    private string ClipDuration(float clipLength)
    {
        float minutes = Mathf.FloorToInt(clipLength / 60);
        float seconds = Mathf.FloorToInt(clipLength % 60);
        string clipDuration = string.Format("{0:00}:{1:00}", minutes, seconds);
        return clipDuration;
    }

    private void CheckForAudioPreviewer()
    {
        if (!musicObject.gameObject.GetComponent<AudioPreviewer>())
        {
            musicObject.gameObject.AddComponent<AudioPreviewer>();
        }
    }
    private void CheckForAudioPreviewer(GameObject selectedObject)
    {
        if (!selectedObject.GetComponent<AudioPreviewer>())
        {
            selectedObject.AddComponent<AudioPreviewer>();
        }
    }
    private AudioSource AssignClipToAudioSource(string objectName, AudioSource source, AudioClip clip)
    {
        if (!GameObject.Find(objectName))
        {
            source = new GameObject().AddComponent<AudioSource>();
            source.name = objectName;
        }
        else
        {
            source = GameObject.Find(objectName).GetComponent<AudioSource>();
        }
        source.clip = clip;
        return source;
    }
}

