using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public GameObject Player;
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.rotation = Quaternion.Inverse(Quaternion.Euler(0.0f, 0.0f, Player.transform.rotation.z));

        Vector2 facing = (Vector2)Player.transform.up;
        float signed_angle = Vector2.SignedAngle(Vector2.up, facing);

        float angle = signed_angle;
        if (angle < 0)
        {
            angle += 360f;
        }

        float rounded = Mathf.Round(angle / 22.5f);
        Debug.Log(rounded);

        if (rounded <= 15)
        {
            ChangeSprite((int)rounded);
        }
        else
        {
            ChangeSprite(0);
        }
    }

    void ChangeSprite(int num)
    {
        spriteRenderer.sprite = sprites[num];
    }
}
