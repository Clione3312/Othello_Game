using UnityEngine;

public class Sprite : MonoBehaviour
{
    public void SetState(int State){
        gameObject.SetActive(State != GameManager.SPRITE_NONE);
        switch (State)
        {
            case GameManager.SPRITE_BLACK:
                gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case GameManager.SPRITE_WHITE:
                gameObject.transform.localRotation = Quaternion.Euler(180, 0, 0);
                break;
        }
    }
}
