using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hcp;
public interface IMapTurnToUI {

    float GetTurningPointInUI();
    E_WhichTurn GetWhichTurnInUI();
    void SetTurningPointToUI(float turningPoint);
    void SetWhichTurnToUI(E_WhichTurn whichTurn);
}
