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
        RectTransform rectTransform = GetComponent<RectTransform>();
        DOTween.To(() => rectTransform.position.y, y => rectTransform.position = new Vector3(rectTransform.position.x, y, rectTransform.position.z), 2400, duration)
        .OnComplete(() =>
        {
            banner.sprite = sprite;
            DOTween.To(() => rectTransform.position.y, y => rectTransform.position = new Vector3(rectTransform.position.x, y, rectTransform.position.z), initialBannerYPos, 5 * duration).SetEase(Ease.OutElastic);
        });
    }
}

public enum Player
{
    PLAYER1, PLAYER2
}


