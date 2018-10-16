using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using hcp;
public interface IObjToCharactor {

    void BeenHitByObs(ObstacleST obstacleST);
    void GetItem(ItemST itemST);
    bool GetMagnetState();

}
