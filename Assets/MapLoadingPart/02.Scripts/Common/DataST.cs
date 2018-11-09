using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    [System.Serializable]
    public enum E_STAGE
    {
        NONE=0,
        //절대 다른 것 이 사이에 삽입 불가.
        STAGE_1,
        STAGE_2,
        STAGE_3,
        STAGE_4,
        STAGE_5,
        STAGE_6,
        STAGE_7,
        STAGE_8,
        STAGE_9,
        STAGE_10,
        STAGE_11,
        STAGE_12,
        STAGE_13,
        STAGE_14,
        STAGE_15,
        STAGE_16,

        INFINITY,
        BOSS,
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

        BOSS_FIREBALL,
        BOSS_BREATH,
        BOSS_METEOR,

        DEATH_WALL,

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