using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageIcon : MonoBehaviour
{

    public Sprite[] sprites;
    public float lifetime;
    public GameObject particleFx;

    void Start()
    {
        Invoke("Destruction", lifetime);
        transform.DOScale(Vector3.zero, lifetime * 0.5f).From().SetEase(Ease.OutElastic);
    }

    public void ChooseIconBasedOnDamage(int damage)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Mathf.Clamp(damage - 1, 0, sprites.Length - 1)];
    }

    private void Destruction()
    {
        Instantiate(particleFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
