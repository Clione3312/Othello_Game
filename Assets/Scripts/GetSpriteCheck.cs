using UnityEngine;

public class GetSpriteCheck
{
    private static GetSpriteCheck _instance = new GetSpriteCheck();
    public static GetSpriteCheck I { get; private set; } = _instance;

    private const int DIRECTION_COUNT = 8;

    private int[][] juggedArray;

    public int[][] ConvertJuggedArray(string str){
        int idx = 0;
        if (str == string.Empty){
            return null;
        }

        juggedArray = new int[str.Split(' ').Length][];
        foreach (string item in str.Split(' '))
        {
            string[] pos = item.Split(',');
            juggedArray[idx] = new int[pos.Length];

            int y = int.Parse(pos[0]);
            int x = int.Parse(pos[1]);

            juggedArray[idx][0] = y;
            juggedArray[idx][1] = x;
            idx++;
        }

        return juggedArray;
    }


    /// <summary>
    /// 獲得できる駒を探す
    /// </summary>
    /// <param name="fieldState">盤面</param>
    /// <param name="ySprite">自分の駒</param>
    /// <param name="eSprite">相手の駒</param>
    /// <returns></returns>
    public string PutSpritePosition(int[][] fieldState, int ySprite, int eSprite) {
        string spritePos = string.Empty;

        for (int y = 0; y < GameManager.BOARD_SIZE; y++){
            for (int x = 0; x < GameManager.BOARD_SIZE; x++){
                // 空白以外はスキップ
                if(fieldState[y][x] != GameManager.SPRITE_NONE) continue;

                // 獲得できる駒がある場合、現在の位置を文字列に追加する
                string str = GetSpritePosition(y, x, fieldState, ySprite, eSprite);
                if (str != string.Empty){
                    string addPos = y.ToString() + "," + x.ToString();
                    spritePos = (spritePos != string.Empty ? spritePos + " " : string.Empty) + addPos;
                }
            }
        }

        return spritePos;
    }

    /// <summary>
    /// 獲得できる駒を探す
    /// </summary>
    /// <param name="posY">Y位置</param>
    /// <param name="posX">X位置</param>
    /// <param name="fieldState">盤面</param>
    /// <param name="ySprite">自分の駒</param>
    /// <param name="eSprite">相手の駒</param>
    /// <returns></returns>
    public string GetSpritePosition(int posY, int posX, int[][] fieldState, int ySprite, int eSprite) {
        string spritePos = string.Empty;

        // 現在の位置から、各方向に獲得できる駒を探す
        for (int i = 0; i < DIRECTION_COUNT; i++){
            string str = GetSpriteLine(posY, posX, i, fieldState, ySprite, eSprite);
            if (str != string.Empty) spritePos = (spritePos != string.Empty ? spritePos + " " : string.Empty) + str;
        }

        return spritePos;
    }

    /// <summary>
    /// 獲得できる駒を探す
    /// </summary>
    /// <param name="posY">Y位置</param>
    /// <param name="posX">X位置</param>
    /// <param name="dire">方向</param>
    /// <param name="fieldState">盤面</param>
    /// <param name="ySprite">自分の駒</param>
    /// <param name="eSprite">相手の駒</param>
    /// <returns></returns>
    private string GetSpriteLine(int posY, int posX, int dire, int[][] fieldState, int ySprite, int eSprite) {
        string spritePos = string.Empty;
        int[][] directions = new int[][] {new int[]{-1, 0}, new int[]{-1, 1}, new int[]{0, 1}, new int[]{1, 1}, new int[]{1, 0}, new int[]{1, -1}, new int[]{0, -1}, new int[]{-1, -1}};

        // 現在の位置から獲得できる駒を探す
        while (posY >= 0 && posY < GameManager.BOARD_SIZE && posX >= 0 && posX < GameManager.BOARD_SIZE)
        {
            posY += directions[dire][0];
            posX += directions[dire][1];

            // 盤外なら終了
            if (!(posY >= 0 && posY < GameManager.BOARD_SIZE)) return string.Empty;
            if (!(posX >= 0 && posX < GameManager.BOARD_SIZE)) return string.Empty;

            // 獲得できる駒の場合、現在の位置を文字列に追加
            if(fieldState[posY][posX] == eSprite) {
                string addPos = posY.ToString() + "," + posX.ToString();
                spritePos = (spritePos != string.Empty ? spritePos + " " : string.Empty) + addPos;
                continue;
            }

            // 空白なら終了
            if(fieldState[posY][posX] == GameManager.SPRITE_NONE) return string.Empty;

            // 自分の駒なら終了
            if (fieldState[posY][posX] == ySprite) {
                if (spritePos == string.Empty) {
                    return string.Empty;
                } else {
                    break;
                }
            }
        }

        return spritePos;
    }
}
