using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mapping_POIs : MonoBehaviour
{
    
    public List<GameObject> POIsTocheck = new List<GameObject>();
    public List<GameObject> POIsCreated = new List<GameObject>();
    public int MaximumPOIsConnected = 2;
    public GameObject POIPrefab;
    public GameObject StartPOI;
    public GameObject EndPOI;
    public GameObject[] objs;
    

    /* 
    Author : Kevin Duret
    Last Update : 19/08/2020
    Object of the class : Create the points randomly on map and connect them
    Macro-working : 
    1) Create points function is called until all macro conditions are OK (for the moment no macro conditions but we can store conditions in it)
    2) The function will take 1 POI as origin and create points (looping until the conditions for the point created are ok)
    3) For each point created we check the routes between the points, if the origin point is full of connected points, we get to the next point to set it as origin
    4) When all points are created, we check various conditions (Starting Point and Ending point are connected, other specific) and modify to make this conditions OK
    5) not writen yet : Multiple Starts / multiple Ends / Specific conditions for the capital POI
    */
    
    //Start function is called at the start of the script
    void Start()
    {

        //Launch the random points generation to get it into prevalidation function (is the map randomly generated respects all conditions ? if not, restart the creation) X times
        CreatePointsOnMap();
        int MaxTriesMap = 10;
        while (MapConditionNotOk() && MaxTriesMap > 0)
        {
            //Delete all POIs created in the map
            foreach(GameObject POI in POIsCreated)
            {
                GameObject.Destroy(POI);
            }
            CreatePointsOnMap();
            MaxTriesMap -= 1;
        }
    }
    

    void Update()
    {

    }


    void CreatePointsOnMap ()
    {
        //Get a POIlist of the POIs to create  and mix them in order to create random and a POIToCheckList with only start at the beginning
        var POIsToCreate = new Queue<string>();

        //Créer une boucle avec variable du nombre de ville 
        POIsToCreate.Enqueue("1");
        POIsToCreate.Enqueue("2");
        POIsToCreate.Enqueue("3");
        POIsToCreate.Enqueue("4");
        POIsToCreate.Enqueue("5");
        POIsToCreate.Enqueue("6");
        POIsToCreate.Enqueue("7");
        POIsToCreate.Enqueue("8");
        POIsToCreate.Enqueue("9");
        POIsToCreate.Enqueue("10");
        POIsToCreate.Enqueue("11");
        POIsToCreate.Enqueue("12");
        POIsToCreate.Enqueue("13");
        POIsToCreate.Enqueue("14");
        POIsToCreate.Enqueue("15");
        POIsToCreate.Enqueue("16");
        POIsToCreate.Enqueue("17");
        POIsToCreate.Enqueue("18");
        POIsToCreate.Enqueue("19");
        POIsToCreate.Enqueue("20");

        POIsTocheck.Add(StartPOI);
        int MaxTries = 1000;
        //pop one POI (POI start first) already created from POIToCheckList
        while (Map_in_construction(POIsToCreate))
        {
            GameObject OriginPOI = POIsTocheck[0];
            Debug.Log("We're checking " + OriginPOI);
            //The limits of random are the limits of the map
            Vector3 POIPosition = new Vector3(Random.Range(-10, 8), Random.Range(-1, 5), 1);
            //once we created the POI position, we check if all conditions are ok
            while (PointCondition(POIPosition) && MaxTries > 0)
            {
                POIPosition = new Vector3(Random.Range(-10, 8), Random.Range(-1, 5), 1);
                MaxTries -= 1;
            }
            if (MaxTries == 0) Debug.Log("nb max de try atteint");
            GameObject POICreated = Instantiate(POIPrefab, POIPosition, Quaternion.identity);
            POICreated.name = POIsToCreate.Dequeue();
            POICreated.tag = "POI";
            POIsCreated.Add(POICreated);
            CheckRoutes();
            Debug.Log("POI " + POICreated.name + " created at " + POICreated.transform.position + "Origin POI" + OriginPOI.name + "with " + OriginPOI.GetComponent<POI_Variables>().POIsConnected.Count + "POIs Connected");
            //Write the if condition for the POI to check to be removed (3 or less POIs connected)
            if (OriginPOI.GetComponent<POI_Variables>().POIsConnected.Count > MaximumPOIsConnected) POIsTocheck.RemoveAt(0);
            POIsTocheck.Add(POICreated);
        }
        //we check if the endPOI has a minimum of 1 POI connected
        if (EndPOI.GetComponent<POI_Variables>().POIsConnected.Count == 0)
        {
            Debug.Log("EndPOI sans POI connecté");
            objs = GameObject.FindGameObjectsWithTag("POI");
            //Debug.Log(GameObject.FindGameObjectsWithTag("POI").Length);
            //we take a POI from the list and compare it to all the other POIs in the list
            GameObject LowestDistancePOI = StartPOI;
            foreach (GameObject obj in objs)
            {
                if (obj != EndPOI)
                { 
                    if (Vector3.Distance(EndPOI.transform.position, obj.transform.position) < Vector3.Distance(EndPOI.transform.position, LowestDistancePOI.transform.position))
                    {
                        LowestDistancePOI = obj;
                    }
                }
            }
            EndPOI.GetComponent<POI_Variables>().POIsConnected.Add(LowestDistancePOI);
            LowestDistancePOI.GetComponent<POI_Variables>().POIsConnected.Add(EndPOI);
        }
        //we check if the startPOI has a minimum of 1 POI connected
        if (StartPOI.GetComponent<POI_Variables>().POIsConnected.Count==0);
        {
            Debug.Log("StartPOI sans POI connecté");
            objs = GameObject.FindGameObjectsWithTag("POI");
            //Debug.Log(GameObject.FindGameObjectsWithTag("POI").Length);
            //we take a POI from the list and compare it to all the other POIs in the list
            GameObject LowestDistancePOI = EndPOI;
            foreach (GameObject obj in objs)
            {
                if (obj != StartPOI)
                {
                    if (Vector3.Distance(StartPOI.transform.position, obj.transform.position) < Vector3.Distance(StartPOI.transform.position, LowestDistancePOI.transform.position))
                    {
                        LowestDistancePOI = obj;
                    }
                }
            }
            StartPOI.GetComponent<POI_Variables>().POIsConnected.Add(LowestDistancePOI);
            LowestDistancePOI.GetComponent<POI_Variables>().POIsConnected.Add(StartPOI);
        }
        //We Check if a path exists between start and end
        if (PathExist()) Debug.Log("PathExist");
        while (!PathExist())
        {
            Debug.Log("proutCheckcheck");
            List<GameObject> NewPOIsToCheck = new List<GameObject>();
            List<GameObject> POIsChecked = new List<GameObject>();
            GameObject ActualPOI = StartPOI;
            GameObject EasternPOI = StartPOI;
            //we get all the gameobjects connected to start POI
            NewPOIsToCheck.Add(ActualPOI);
            while (NewPOIsToCheck.Count()>0)
            {
                
                ActualPOI = NewPOIsToCheck[0];
                List<GameObject> TempList = new List<GameObject>();
                TempList = ActualPOI.GetComponent<POI_Variables>().POIsConnected;
                foreach (GameObject POI in TempList)
                {
                    if (!POIsChecked.Contains(POI)) NewPOIsToCheck.Add(POI);

                    Debug.Log("proutCheck" +  POI);

                }
                NewPOIsToCheck.Remove(ActualPOI);
                POIsChecked.Add(ActualPOI);
                Debug.Log("proutActual" + ActualPOI);
            }
            //we search the easternPOI in gameobjects connected to startPOI
            foreach (GameObject POI in POIsChecked)
            {
                Debug.Log("proutEasternCompare" + EasternPOI + POI);
                if (EasternPOI.transform.position.x > POI.transform.position.x) EasternPOI = POI;

            }
            Debug.Log("proutEastern" + EasternPOI);
            objs = GameObject.FindGameObjectsWithTag("POI");
            //we set the easterngame object with a connected POI to the 1st POI next to it
            GameObject LowestDistancePOI = EndPOI;
            foreach (GameObject obj in objs)
            {
                if (EasternPOI.transform.position.x > obj.transform.position.x)
                {
                    Debug.Log("prout" + obj);
                    if (Vector3.Distance(EasternPOI.transform.position, obj.transform.position) > Vector3.Distance(EasternPOI.transform.position, LowestDistancePOI.transform.position))
                    {
                        LowestDistancePOI = obj;
                    }
                }

            }
            EasternPOI.GetComponent<POI_Variables>().POIsConnected.Add(LowestDistancePOI);
            LowestDistancePOI.GetComponent<POI_Variables>().POIsConnected.Add(EasternPOI);
        }
        //we check if a POI is alone and else create a route to this one
        objs = GameObject.FindGameObjectsWithTag("POI");
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<POI_Variables>().POIsConnected.Count()==0)
            {
                Debug.Log("proutAlone" + obj);
                GameObject LowestDistancePOI = EndPOI;
                foreach (GameObject objc in objs)
                {
                    if (obj!=objc && Vector3.Distance(obj.transform.position, objc.transform.position) < Vector3.Distance(obj.transform.position, LowestDistancePOI.transform.position))
                    {
                        LowestDistancePOI = obj;
                    }
                }
                obj.GetComponent<POI_Variables>().POIsConnected.Add(LowestDistancePOI);
                LowestDistancePOI.GetComponent<POI_Variables>().POIsConnected.Add(obj);
            }
        }
        

    }
    
    //To check if there is a path between start point and endpoint
    bool PathExist()
    {
        List<GameObject> NewPOIsToCheck = new List<GameObject>();
        List<GameObject> POIsChecked = new List<GameObject>();
        //GameObject StartPOI = GameObject.Find("Bordeaux");
        //GameObject EndPOI = GameObject.Find("Nice");
        GameObject ActualPOI = StartPOI;
        NewPOIsToCheck.Add(StartPOI);
        while (ActualPOI != EndPOI)
        {
            //if there is no POI to check and no endPOI touched, return false
            if (NewPOIsToCheck.Count() == 0) return false;
            ActualPOI = NewPOIsToCheck[0];
            //get the list of the actual POI, get the list of the POIs connected not visited yet
            List<GameObject> TempList = new List<GameObject>(); 
            TempList = ActualPOI.GetComponent<POI_Variables>().POIsConnected;
            foreach (GameObject POI in TempList)
            {
                if (!POIsChecked.Contains(POI)) NewPOIsToCheck.Add(POI);
            }
            //go to next POI connected
            NewPOIsToCheck.Remove(ActualPOI);
            POIsChecked.Add(ActualPOI);
            
        }
        Debug.Log("path found");
        return true;
    }


    //Check if the finished map reach all conditions, not useful yet but still may be ...
    bool MapConditionNotOk()
    {
        //GameObject EndPOI = GameObject.Find("Nice");
        if (EndPOI.GetComponent<POI_Variables>().POIsConnected.Count == 0)
        {
            Debug.Log("probleme endPOI trouvé");
            return true;
        }
        else
        {
            Debug.Log("Pas de probleme endPOI");
            //GameObject StartPOI = GameObject.Find("Bordeaux");
            if (StartPOI.GetComponent<POI_Variables>().POIsConnected.Count == 0)
            {
                Debug.Log("probleme startPOI trouvé");
                return true;
            }
            else
            {
                Debug.Log("Pas de probleme startPOI");
                return false;
            }
        }
        

    }

    //check if the point created randomly respond to all requirements

    public float MinimumDistanceBetweenPOIs = 1f;

    bool PointCondition(Vector3 POIPosition)
    {
        objs = GameObject.FindGameObjectsWithTag("POI");
        foreach(GameObject obj in objs)
        {
            //check if the point is at the good distance of the origin POI
            if (Vector3.Distance(POIPosition, obj.transform.position) < MinimumDistanceBetweenPOIs)
            {
                Debug.Log("point trop proche!" + POIPosition + obj.transform.position);
                return true;
            }
        }
        return false;
    }

    bool Map_in_construction (Queue<string> POIsToCreate)
    {
        //Check if the EndPOI has the maximum of POIs connected or if there is no POIs left to create
        if (POIsToCreate.Count == 0) return false;
        else return true;
    }
    

    //Functions for Mapping POIs
    public Dictionary<GameObject, float> POIDistance = new Dictionary<GameObject, float>();

    // Start is called before the first frame update
    void CheckRoutes()
    {
        //Debug.Log("launching checkroutes");
        //we make a list with all POIs existing in the scene
        objs = GameObject.FindGameObjectsWithTag("POI");
        //Debug.Log(GameObject.FindGameObjectsWithTag("POI").Length);
        //we take a POI from the list and compare it to all the other POIs in the list
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<POI_Variables>().POIsConnected.Clear();
            foreach (GameObject objc in objs)// we get the POI we want to compare from the list
            {
                objc.GetComponent<POI_Variables>().POIsConnected.Remove(obj);
                if (obj == objc) // if it is the same POI, just pass 
                {
                    //Debug.Log("it is the same POI!");

                }
                else // if it is a different POI
                {
                    POIDistance.Add(objc, Vector3.Distance(objc.transform.position, obj.transform.position));
                }

            }
            Debug.Log("pour la ville" + obj);
            Debug.Log("voici les valeurs du dictionnaire avant traitement");
            foreach (KeyValuePair<GameObject, float> POI in POIDistance.OrderBy(Key => Key.Value))
            {
                    Debug.Log("voici la valeur du dictionnaire" + POI);
            }
            //Debug.Log("here is the connected POIs for the POI " + obj);
            int i = 0;
            foreach (KeyValuePair<GameObject, float> POI in POIDistance.OrderBy(Key => Key.Value))
            {
                if (i <= MaximumPOIsConnected)
                {
                    Debug.Log("voici la valeur du dictionnaire en cours de traitement" + POI);
                    //we set the both POIs in the ConnectedPOIs List of each other
                    obj.GetComponent<POI_Variables>().POIsConnected.Add(POI.Key);
                    POI.Key.GetComponent<POI_Variables>().POIsConnected.Add(obj);
                    i += 1;
                }
                if (i > MaximumPOIsConnected) Debug.Log("ville " + POI + "non connectée car trop de villes connectées ");
            }
                
            POIDistance.Clear();
            //remettre à jour les distances à chaque fois sinon on a des distances en fonction des premiers points créés...

        }

    }
    

}
