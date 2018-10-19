using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp
{
    public static class ObjFactory
    {
        static GameObject  GenByPool(GameObject objByPool, Transform spawnPoint)
        {
            if (objByPool == null)
            {
                Debug.Log("풀링 해오지 못함");
                return null;
            }
            objByPool.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            objByPool.SetActive(true);
            return objByPool;
        }

        public static GameObject ObjSpawnFactory(E_SPAWN_OBJ_TYPE typeOfObj, Transform spawnPoint)
        {
            GameObject spawned;
            switch (typeOfObj)
            {
                case E_SPAWN_OBJ_TYPE.BALL:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsBallInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.COIN:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemCoinInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.COIN_PARABOLA://포물선 중앙에 허들 이나 그거 문제
                                                    //생성을 할것이냐. 풀링 자체에서 허들을 그냥 false시킬 것이냐.
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemCoin_Parabola_InPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.COIN_STRAIGHT:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemCoin_StraightLine_InPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.FIRE:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsFireInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.HPPLUS:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemHPPlusInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.HUDDLE:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsHuddleInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.INVINCIBLE:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemInvincibleInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.MAGNET:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemMagnetInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.NOTHING:
                    spawned =
                        null;
                    break;
                case E_SPAWN_OBJ_TYPE.SHIELD:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetItemShieldInPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_1:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_1_InPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_2:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_2_InPool(), spawnPoint);
                    break;
                case E_SPAWN_OBJ_TYPE.UPPER_HUDDLE_3:
                    spawned =
                    GenByPool(MapAndObjPool.GetInstance().GetObsUpperHuddle_3_InPool(), spawnPoint);
                    break;
                default:
                    ErrorManager.SpurtError("오브젝트 팩토리에서 에러");
                    spawned = null;
                    break;
            }
            return spawned;
        }
    }
}