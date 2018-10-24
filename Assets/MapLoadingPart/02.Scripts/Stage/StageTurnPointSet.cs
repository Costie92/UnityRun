using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hcp {

    [System.Serializable]
    public class StageTurnPointSet{

        public float turningPoint = 0;
        public E_WhichTurn whichTurn = E_WhichTurn.NOT_TURN;
        public bool Init = false;

        public StageTurnPointSet(float tp, E_WhichTurn wt)
        {
            turningPoint = tp;
            whichTurn = wt;
            Init = false;
        }
        public float ConvertTurnPointToRealTurnPoint(float chunkMargin)
        {
            return turningPoint * chunkMargin;
        }
        public void DisableThisSet()
        {
            turningPoint = -1;
            whichTurn = E_WhichTurn.NOT_TURN;
            Init = true;
        }
        public bool IsDisabled()
        {
            if (turningPoint == -1 && whichTurn == E_WhichTurn.NOT_TURN && Init == true)
                return true;
            else return false;
        }
    }
}
