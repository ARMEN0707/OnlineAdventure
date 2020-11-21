using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataScenes
{
    public static int numberSkinCharacter=0;
    public static bool client=false;
    public static bool finish = false;
    public static int collectedFruits=0;
    public static int allFruits;
    public static int priceWin = 100;
    public static int place = 0;
    public static GameObject characterClient;

    public static string IPAddress;

    public struct CharacterInfomation
    {
        public Vector3 position;
        public Vector2 velocity;
        public bool doubleJump;
        public bool wallStick;
        public bool wallJump;
        public bool run;
        public bool flipX;
    }
}
