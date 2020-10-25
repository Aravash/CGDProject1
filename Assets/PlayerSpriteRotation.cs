using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRotation : MonoBehaviour
{

    private GameObject Player;
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    private Sprite[] unarmedSprites;
    private Sprite[] armedSprites;
    private bool armed;
    // Start is called before the first frame update
    void Start()
    {
        // get EnemyObject
        Player = gameObject.transform.parent.gameObject;
        armed = true;
        // getSpriteSheet
        unarmedSprites = Resources.LoadAll<Sprite>("playerBlueUnarmed");
        armedSprites = Resources.LoadAll<Sprite>("playerBlue");

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = armedSprites[0];
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
        if(armed)
        {
            spriteRenderer.sprite = armedSprites[num];
        }
        else
        {
            spriteRenderer.sprite = unarmedSprites[num];
        }

    }

    public void ChangeArmed (bool arm)
    {
        armed = arm;
    }
}
