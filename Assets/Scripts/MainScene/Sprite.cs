using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using System.Collections;

public class Sprite : MonoBehaviour
{
    private const int SPRITE_NONE = 0;
    private const int SPRITE_BLACK = 1;
    private const int SPRITE_WHITE = 2;

    private int prevState = 0;
    private int currState = 0;

    private Sequence sequence;

    private void Start()
    {
        sequence = DOTween.Sequence();
    }

    public void SetState(int state)
    {
        currState = state;
        gameObject.SetActive(state != SPRITE_NONE);

        Sequence sequence;

        switch (state)
        {
            case SPRITE_BLACK:
                sequence = PutSpriteBlack();
                sequence.Play();
                break;
            case SPRITE_WHITE:
                sequence = PutSpriteWhite();
                sequence.Play();
                break;
        }
    }

    public void ChangeState(int state)
    {
        prevState = currState;
        currState = state;

        gameObject.SetActive(state != SPRITE_NONE);

        if (currState != SPRITE_NONE)
        {
            Sequence sequence;

            if (prevState == SPRITE_NONE)
            {
                if (currState == SPRITE_BLACK)
                {
                    sequence = PutSpriteBlack();
                }
                else
                {
                    sequence = PutSpriteWhite();
                }
            }
            else
            {
                if (currState == SPRITE_BLACK)
                {
                    sequence = ChangeSpriteBlack();
                }
                else
                {
                    sequence = ChangeSpriteWhite();
                }
            }

            sequence.SetAutoKill(true).Play();
        }
    }

    private Sequence PutSpriteBlack()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOMoveY(GameManager.INST_OFFSET_Y, 0.1f));

        return sequence;
    }

    private Sequence PutSpriteWhite()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(180, 0, 0);

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOMoveY(GameManager.INST_OFFSET_Y, 0.1f));

        return sequence;
    }

    private Sequence ChangeSpriteWhite()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveY(1f, 0.1f))
            .Join(transform.DORotateQuaternion(Quaternion.Euler(90, 0, 0), 0.1f))
            .Append(transform.DOMoveY(GameManager.INST_OFFSET_Y, 0.1f))
            .Join(transform.DORotateQuaternion(Quaternion.Euler(180, 0, 0), 0.1f));

        return sequence;
    }

    private Sequence ChangeSpriteBlack()
    {
        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(transform.DOLocalMoveY(1f, 0.1f))
            .Join(transform.DORotateQuaternion(Quaternion.Euler(90, 0, 0), 0.1f))
            .Append(transform.DOMoveY(GameManager.INST_OFFSET_Y, 0.1f))
            .Join(transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), 0.1f));

        return sequence;
    }
}
