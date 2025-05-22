using UnityEngine;

public class CalcSprite
{
    public int[][] ConvertJuggedArray(string str)
    {
        int[][] field = new int[str.Split(' ').Length][];
        int idx = 0;
        foreach (string pos in str.Split(' '))
        {
            string[] posXY = pos.Split(',');
            field[idx] = new int[2];

            field[idx][0] = int.Parse(posXY[0]);
            field[idx][1] = int.Parse(posXY[1]);

            idx++;
        }

        return field;
    }

    public string GetPutPosition(int[][] field, int ySprite, int eSprite)
    {
        string putPosition = "";    // 返り値の変数宣言

        for (int posY = 0; posY < GameManager.BOARD_SIZE; posY++)
        {
            for (int posX = 0; posX < GameManager.BOARD_SIZE; posX++)
            {
                if (field[posY][posX] != GameManager.SPRITE_NONE) continue;

                string getSprite = GetSpritesPosition(posY, posX, field, ySprite, eSprite);
                if (getSprite != string.Empty)
                {
                    string addPutPos = string.Format("{0},{1}", posY, posX);
                    putPosition = (putPosition == string.Empty ? string.Empty : putPosition + " ") + addPutPos;
                }
            }
        }

        return putPosition;
    }

    public string GetSpritesPosition(int posY, int posX, int[][] field, int ySprite, int eSprite)
    {
        string putPosition = "";    // 返り値の変数宣言

        for (int i = 0; i < GameManager.BOARD_SIZE; i++)
        {
            string getSprite = GetSpritesLine(posY, posX, i, field, ySprite, eSprite, i);
            if (getSprite != string.Empty)
            {
                putPosition = (putPosition == string.Empty ? string.Empty : putPosition + " ") + getSprite;
            }
        }

        return putPosition;
    }

    private string GetSpritesLine(int posY, int posX, int dire, int[][] field, int ySprite, int eSprite, int j)
    {
        string putPosition = "";    // 返り値の変数宣言
        int[][] directions = new int[][] { new int[2] { -1, 0 }, new int[2] { -1, 1 }, new int[2] { 0, 1 }, new int[2] { 1, 1 }, new int[2] { 1, 0 }, new int[2] { 1, -1 }, new int[2] { 0, -1 }, new int[2] { -1, -1 } };

        while (posY >= 0 && posY < GameManager.BOARD_SIZE && posX >= 0 && posX < GameManager.BOARD_SIZE)
        {
            posY += directions[dire][0];
            posX += directions[dire][1];

            if (!(posY >= 0 && posY < GameManager.BOARD_SIZE && posX >= 0 && posX < GameManager.BOARD_SIZE)) return string.Empty;

            if (field[posY][posX] == eSprite)
            {
                string addPutPos = string.Format("{0},{1}", posY, posX);
                putPosition = (putPosition != string.Empty ? putPosition + " " : string.Empty) + addPutPos;
                continue;
            }

            if (field[posY][posX] == GameManager.SPRITE_NONE) return string.Empty;

            if (field[posY][posX] == ySprite) {
                if (putPosition == string.Empty) return string.Empty;
                else break;
            }
        }

        return putPosition;
    }
}
