using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
[ExecuteAlways]
public class JBAudioUtility : EditorWindow
{
    public AudioClip clipToPlay;
    AudioSource audioSource;
    AudioSource musicObject;
    [MenuItem("JB Tools/Audio Utility")]
    static void OpenWindow()
    {
        EditorWindow.GetWindow(typeof(JBAudioUtility));
    }
    void OnGUI()
    {
        clipToPlay = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", clipToPlay, typeof(AudioClip), false);
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Play"))
            {
               audioSource = AssignClipToAudioSource("Temp Audio Source", audioSource);
               audioSource.Play();
            }
            if (GUILayout.Button("Stop"))
            {
                if(audioSource != null)
                {
                audioSource.Stop();
                DestroyImmediate(audioSource.gameObject);
                }

            }
        }
        if (GUILayout.Button("Create Music Object"))
            {
            if (musicObject == null)
            {
                musicObject = AssignClipToAudioSource("Music Object", musicObject);
                Selection.activeGameObject = musicObject.gameObject;
            } else
            {
                musicObject = AssignClipToAudioSource(musicObject.name, musicObject);
            }
            }
    }
    private AudioSource AssignClipToAudioSource(string objectName, AudioSource source)
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
        source.clip = clipToPlay;
        return source;
    }
}
