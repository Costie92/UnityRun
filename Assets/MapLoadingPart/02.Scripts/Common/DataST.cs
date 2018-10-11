
namespace hcp
{
        public  enum E_ITEM 
        {
            HPPLUS = 0,
            INVINCIBLE,
            SHIELD,
            COIN,
            MAGNET,
        };

        public  enum E_OBSTACLE 
        {
            BALL = 0,
            HUDDLE,
            UPPER_HUDDLE,
            FIRE,
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