using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRegimentCharge : MonoBehaviour
{
    /*Regiment Charge
     * 
     * Variables : 
     * 
     * List "A" of classes with regiment, possible position, path and number of turns to get to the point
     * List "B" of classes with round and position of the regiment at this specific round
     * List "Selected cases" of classes with order, regiment, possible position, path and number of turns to get to the point
     * 
     * Script :
     * get the list of the regiments of the actual side
     * 
     * Start Loop :
     * get the regiments that has not already be prioritized (not in the "selected cases" list)
     * get the possible combat positions for each regiment (function for each regiment, with prefered slots example archers or pikemen,check if the slot is available (i.e no unit of osbstacle)) 
     * calculate the path for each possible combat position
     * check if the path simulated put the regiment in a cell that is already used by another regiment at the end of the same round
     * If it is the case, the previous regiment had the priority so put an obstacle on that path and rerun the path with these obstacle then delete all obstacles for next cases
     * store the case with regiment, possible position, path and number of turns to get to the point  (List A)
     * only keep the cases with minimum number of turns
     * to Choose who go first, we have 5 rules : 
     * 1) the ones who takes the minimum amount of rounds to get to the point, 
     * 2) then the one who is on the right for player or left for enemy,
     * 3) then the one who has the most speed
     * 4) if there is still 2 regiments that are on the list, take the one who is the lower in the map
     * 5) then just get one for god sake 
     * 
     * Insert the case who go first in "selected cases" with max order + 1
     * 
     * Then the one who go first get his path registered, and also register in a list the cases where its position at the end of each round is stored (List B)
     * 
     * Loop
     * 
     * Order the list "Selected Cases" and instantiate a pathline for each of them, push the path integrated in the selected cases iteration
     * 
     * Note : Display Path Variables Script should include a script to push directly a path into the PathLine
     * 
     * 
     * 
     * 
     * 
     */

}
