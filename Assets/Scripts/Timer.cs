using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    int secondsPlayed;
    bool playing = false;

    public void StartTimer()
    {
        playing = true;
        StartCoroutine(Count());
    }

    public void StopTimer()
    {
        playing = false;
    }

    IEnumerator Count()
    {
        while (playing)
        {
            yield return new WaitForSeconds(1);
            secondsPlayed++;
            _text.SetText(SecondsToTimer(secondsPlayed));
        }
    }

    string SecondsToTimer(int sec)
    {
        int seconds = sec % 60;
        int minutes = (int)Mathf.Floor(sec / 60f);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
