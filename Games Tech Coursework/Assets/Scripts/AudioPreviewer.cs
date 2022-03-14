using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AudioPreviewer : MonoBehaviour
{
    public AudioSource source { get; set; }
    public bool isPaused { get; set; }
    public float playhead { get; set; }
}
[CustomEditor(typeof(AudioPreviewer))]
public class audioPreviewerButtons : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        AudioPreviewer ap = (AudioPreviewer)target;
        if (ap.source == null)
        {
            ap.source = ap.GetComponent<AudioSource>();
        }

            ap.playhead = ap.source.time;
        using (var horizontalScope = new GUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Play"))
            {
                ap.source.Play();
                ap.isPaused = false;

            }
            if (GUILayout.Button("Pause"))
            {

                if (ap.isPaused)
                {
                    ap.source.Play();
                    ap.isPaused = false;
                }
                else
                {
                    ap.isPaused = true;
                    ap.source.Pause();
                }

            }
            if (GUILayout.Button("Stop"))
            {
                ap.source.Stop();
            }
            if (ap.source.time < ap.source.clip.length)
            {
                ap.source.time = GUILayout.HorizontalSlider(ap.source.time, 0, ap.source.clip.length);
            }
        }
    }
}
