using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;

public partial class Difficulity
{
    public class Difficulity_Public
    {
        private enum EnemyDifficulity: int
        {
            Easy,
            Normal,
            Hard,
            VeryHard,
            Impossible
        }

        // private static Difficulity_Public _instance= new Difficulity_Public();
        // public static Difficulity_Public I { get; private set; } = _instance;

        private int ySprite;
        private int eSprite;
        private string posString;

        public string GetPutPositionString(int ySp, int eSp) {

            ySprite = ySp;
            eSprite = eSp;

            switch (GameManager.I.Difficulity())
            {
                case (int)EnemyDifficulity.Easy:
                    Difficulity_Easy difficulity_Easy = new Difficulity_Easy();
                    posString = difficulity_Easy.GetSpritePosition(GameManager.I.FieldState(), ySprite, eSprite).ToString();
                    break;
                case (int)EnemyDifficulity.Normal:
                    Difficulity_Normal difficulity_Normal = new Difficulity_Normal();
                    posString = difficulity_Normal.GetSpritePosition(GameManager.I.FieldState(), ySprite, eSprite).ToString();
                    break;
                case (int)EnemyDifficulity.Hard:
                    posString = string.Empty;
                    break;
                case (int)EnemyDifficulity.VeryHard:
                    posString = string.Empty;
                    break;
                case (int)EnemyDifficulity.Impossible:
                    posString = string.Empty;
                    break;
            }

            return posString;
        }

        public int[][] CreateCloneField(int[][] filed) {
            int[][] clone = new int[][]{new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE], new int[GameManager.BOARD_SIZE]};

            for (int y = 0; y < GameManager.BOARD_SIZE; y++) {
                for (int x = 0; x < GameManager.BOARD_SIZE; x++) clone[y][x] = filed[y][x];
            }

            return clone;
        }
    }
}
