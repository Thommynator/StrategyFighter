using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnIndicator : MonoBehaviour
{

    public Player currentPlayer;
    public Image banner;
    public Sprite player1;
    public Sprite player2;
    private float initialBannerYPos;
    [SerializeField] private AudioClip turnSound;

    void Start()
    {
        banner.sprite = player1;
        initialBannerYPos = GetComponent<RectTransform>().position.y;
    }

    public void SwitchTurn()
    {
        currentPlayer = currentPlayer == Player.PLAYER1 ? Player.PLAYER2 : Player.PLAYER1;
        SoundManager.instance.PlayAudio(turnSound);
        ChangeBannerTo(currentPlayer);
    }

    private void ChangeBannerTo(Player player)
    {
        if (player == Player.PLAYER1)
        {
            ChangeSprite(player1);
        }

        if (player == Player.PLAYER2)
        {
            ChangeSprite(player2);
        }
    }

    private void ChangeSprite(Sprite sprite)
    {
        var duration = 0.2f;
        RectTransform rectTransform = banner.GetComponent<RectTransform>();
        DOTween.To(() => rectTransform.rotation.y, y => rectTransform.rotation = Quaternion.Euler(0, y, 0), 270, duration)
        .OnComplete(() =>
        {
            banner.sprite = sprite;
            DOTween.To(() => rectTransform.rotation.y, y => rectTransform.rotation = Quaternion.Euler(0, y, 0), 0, duration);
        });
    }
}

public enum Player
{
    PLAYER1, PLAYER2
}


