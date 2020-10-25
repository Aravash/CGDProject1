using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpriteRotation : MonoBehaviour
{
    private GameObject Enemy;
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(spriteRenderer.sprite == null)
        {
            spriteRenderer.sprite = sprites[0];
        }

        // get EnemyObject
        Enemy = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Enemy.transform.position;

        transform.rotation = Quaternion.Inverse(Quaternion.Euler(0.0f, 0.0f, Enemy.transform.rotation.z));      

        Vector2 facing = (Vector2)Enemy.transform.up;
        float signed_angle = Vector2.SignedAngle(Vector2.up, facing);

        float angle = signed_angle;
        if (angle < 0)
        {
            angle += 360f;
        }

        float rounded = Mathf.Round(angle / 22.5f);

        switch (rounded)
        {
            case 0:
                ChangeSprite(4);
                break;
            case 1:
                ChangeSprite(5);
                break;
            case 2:
                ChangeSprite(6);
                break;
            case 3:
                ChangeSprite(7);
                break;
            case 4:
                ChangeSprite(8);
                break;
            case 5:
                ChangeSprite(9);
                break;
            case 6:
                ChangeSprite(10);
                break;
            case 7:
                ChangeSprite(11);
                break;
            case 8:
                ChangeSprite(12);
                break;
            case 9:
                ChangeSprite(13);
                break;
            case 10:
                ChangeSprite(14);
                break;
            case 11:
                ChangeSprite(15);
                break;
            case 12:
                ChangeSprite(0);
                break;
            case 13:
                ChangeSprite(1);
                break;
            case 14:
                ChangeSprite(2);
                break;
            case 15:
                ChangeSprite(3);
                break;
            case 16:
                ChangeSprite(4);
                break;
            default:
                ChangeSprite(4);
                break;

        }
    }

    void ChangeSprite(int num)
    {
        spriteRenderer.sprite = sprites[num];
    }
}
