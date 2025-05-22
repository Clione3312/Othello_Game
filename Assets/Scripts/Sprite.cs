using UnityEngine;

public class Sprite : MonoBehaviour
{
    public void SetState(int state)
    {
        gameObject.SetActive(state != GameManager.SPRITE_NONE);

        switch (state)
        {
            case GameManager.SPRITE_BLACK:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case GameManager.SPRITE_WHITE:
                transform.rotation = Quaternion.Euler(180, 0, 0);
                break;
        }
    }
}
