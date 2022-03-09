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

    [MenuItem("JB Tools/Audio Utility")]
    
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBAudioUtility));
        
    }
    void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty audioClipsProperty = so.FindProperty("audioClips");
        EditorGUILayout.PropertyField(audioClipsProperty, true);
        so.ApplyModifiedProperties();
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
        for (int i = 0; i < clips.Count; i++)
        {
            
            float playhead = 0;
            {
                if (audioSource != null)
                {
                    playhead = audioSource.time;
                }
            }
            clips[i] = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", clips[i], typeof(AudioClip), false);
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Remove"))
                {
                    clips.Remove(clips[i]);
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
            using (var vertical = new GUILayout.VerticalScope())
            {
                
                if (audioSource != null && audioSource.clip == clips[i])
                {
                   audioSource.time = GUILayout.HorizontalSlider(playhead, 0, clips[i].length);
                }
                else {
                    
                    GUILayout.HorizontalSlider(0, 0, clips[i].length);
                    
                }
                
                GUILayout.FlexibleSpace();
            }
            using (var horizontalScope = new GUILayout.HorizontalScope())
            {
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
                    } else if (Selection.activeGameObject.GetComponent<AudioSource>())
                    {
                        Selection.activeGameObject.GetComponent<AudioSource>().clip = clips[i];
                        CheckForAudioPreviewer();
                    } else
                    {
                        Selection.activeGameObject.AddComponent<AudioSource>().clip = clips[i];
                        CheckForAudioPreviewer();
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
        
    }

    private void CheckForAudioPreviewer()
    {
        if (!musicObject.gameObject.GetComponent<AudioPreviewer>())
        {
            musicObject.gameObject.AddComponent<AudioPreviewer>();
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

