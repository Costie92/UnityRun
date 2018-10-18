
namespace hcp
{
        public  enum E_ITEM 
        {
            HPPLUS = 0,
            INVINCIBLE,
            SHIELD,
            COIN,
            MAGNET,
            EITEMMAX
        };

        public  enum E_OBSTACLE 
        {
            BALL = 0,
            HUDDLE,
            UPPER_HUDDLE,
            FIRE,
            EOBSMAX
        };
    public enum E_SPAWN_OBJ_TYPE
    {
        HPPLUS = 0,
        INVINCIBLE,
        SHIELD,
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
    [System.Serializable]
    public class MapObjST
    {
        public float keyPos;
        E_SPAWN_OBJ_TYPE[] spawnObjType=
            { E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING, E_SPAWN_OBJ_TYPE.NOTHING,};
    };
}