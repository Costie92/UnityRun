using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    [System.Serializable]
    public enum E_STAGE
    {
        STAGE_1 = 0,
        STAGE_2,

        INFINITY,
        E_STAGEMAX
    };

    [System.Serializable]
    public enum E_WhichTurn
    {
        LEFT = 0,
        RIGHT,
        NOT_TURN
    };
    [System.Serializable]
    public  enum E_ITEM
    {
        HPPLUS = 0,
        INVINCIBLE,
        SHIELD,COIN,
        MAGNET,

        EITEMMAX
    };
    [System.Serializable]
    public  enum E_OBSTACLE
    {
        BALL = 0,
        HUDDLE,
        UPPER_HUDDLE,
        FIRE,

        EOBSMAX
    };
    [System.Serializable]
    public enum E_SPAWN_OBJ_TYPE
    {
        HPPLUS = 0,
        INVINCIBLE,
        SHIELD,
        COIN,
        COIN_STRAIGHT,
        COIN_PARABOLA,
        MAGNET,

        BALL,
        HUDDLE,
        UPPER_HUDDLE_1,
        UPPER_HUDDLE_2,
        UPPER_HUDDLE_3,
        FIRE,

        NOTHING,

        EOBJTYPEMAX
    };
    
    [System.Serializable]
    public class ItemST
    {
        public E_ITEM itemType { set; get; }
        public float value { set; get; }
    };
    [System.Serializable]
    public class ObstacleST
    {
        public E_OBSTACLE obstacleType { set; get; }
        public bool beenHit {set;get;}
    };
}