using UnityEngine;
using UnityEngine.UI;


public class MusicMute : MonoBehaviour
{

    public Sprite muteSprite;
    public Sprite unmuteSprite;
    private bool isMuted;


    void Start()
    {
        isMuted = false;
    }

    public void Toggle()
    {
        SoundManager.instance.ToggleMusic();
        isMuted = !isMuted;
        GetComponent<Image>().sprite = isMuted ? unmuteSprite : muteSprite;
    }


}
