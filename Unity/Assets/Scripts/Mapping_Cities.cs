using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Mapping_Cities : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> CitiesTocheck = new List<GameObject>();
    public List<GameObject> CitiesCreated = new List<GameObject>();
    public int MaximumCitiesConnected = 2;
    public GameObject CityPrefab;
    public GameObject Moving_Lines;
    public GameObject[] objs;

    void Start()
    {

        CreatePointsOnMap();

        //get the map created into prevalidation function


        //At the end of the construction, we have to check multiple conditions in this function else we have to restart (creating points) and delete the previous ones
        int MaxTriesMap = 10;
        while (MapCondition() && MaxTriesMap > 0)
        {
            //Delete all cities created in the map (except start and end)
            foreach(GameObject City in CitiesCreated)
            {
                GameObject.Destroy(City);
            }
            CreatePointsOnMap();
            MaxTriesMap -= 1;
        }
        

        //get the number of max cities connected (random 2 or 3) and check if the point has the maximum amount of cities connected

        //pop the first city in city list, create one instance in random x y positions but the x has to be positive (looking east) and y has to stay in a range to define (100 ?)

        //Check the connexions of the cities in map (get the connecting cities function here instead of starting game script) and check if it is over the max number (4)

        // if it is ok, put the City in CityToCheckList and loop to get the next city in citylist

        // if it is not ok, restart with next city in citylist







        //
    }


    void CreatePointsOnMap ()
    {
        float MCMaximum_Distance_Between_Cities = GetComponent<GameVariables>().Maximum_Distance_Between_Cities;
        //Serialiserles gameobject pour éviter de renommer a chaque fois
        CityPrefab = Resources.Load("Ville") as GameObject;
        
        GameObject StartCity = GameObject.Find("Bordeaux");
        GameObject EndCity = GameObject.Find("Nice");
        //Debug.Log("here is the cities found"+StartCity+ EndCity);


        //Get a citylist of the cities to create  and mix them in order to create random and a CityToCheckList with only start at the beginning
        var CitiesToCreate = new Queue<string>();

        //Créer une boucle avec variable du nombre de ville 
        CitiesToCreate.Enqueue("1");
        CitiesToCreate.Enqueue("2");
        CitiesToCreate.Enqueue("3");
        CitiesToCreate.Enqueue("4");
        CitiesToCreate.Enqueue("5");
        CitiesToCreate.Enqueue("6");
        CitiesToCreate.Enqueue("7");
        CitiesToCreate.Enqueue("8");
        CitiesToCreate.Enqueue("9");
        CitiesToCreate.Enqueue("10");
        CitiesToCreate.Enqueue("11");
        CitiesToCreate.Enqueue("12");
        CitiesToCreate.Enqueue("13");
        CitiesToCreate.Enqueue("14");
        CitiesToCreate.Enqueue("15");
        CitiesToCreate.Enqueue("16");
        CitiesToCreate.Enqueue("17");
        CitiesToCreate.Enqueue("18");
        CitiesToCreate.Enqueue("19");
        CitiesToCreate.Enqueue("20");

        CitiesTocheck.Add(StartCity);
        int MaxTries = 1000;
        //pop one city (city start first) already created from CityToCheckList
        while (Map_in_construction(CitiesToCreate))
        {

            GameObject OriginCity = CitiesTocheck[0];
            Debug.Log("We're checking " + OriginCity);
            //a optimiser 
            float MaximumRandom = MCMaximum_Distance_Between_Cities;
            //les limites du random sont les limites de la carte
            Vector3 CityPosition = new Vector3(Random.Range(5, MaximumRandom), Random.Range(5, 12), 1);
            //une fois qu'on a créé la position de la ville, on check si les conditions sont respectées
            while (PointCondition(CityPosition) && MaxTries > 0)
            {
                CityPosition = new Vector3(Random.Range(5, MaximumRandom), Random.Range(5, 12), 1);
                MaxTries -= 1;
            }
            if (MaxTries == 0) Debug.Log("nb max de try atteint");
            GameObject CityCreated = Instantiate(CityPrefab, CityPosition, Quaternion.identity);
            CityCreated.name = CitiesToCreate.Dequeue();
            CityCreated.tag = "City";
            CitiesCreated.Add(CityCreated);
            CheckRoutes();
            Debug.Log("City " + CityCreated.name + " created at " + CityCreated.transform.position + "Origin City" + OriginCity.name + "with " + OriginCity.GetComponent<City_Variables>().CitiesConnected.Count + "Cities Connected");
            //Write the if condition for the city to check to be removed (3 or less cities connected)
            if (OriginCity.GetComponent<City_Variables>().CitiesConnected.Count > MaximumCitiesConnected) CitiesTocheck.RemoveAt(0);
            CitiesTocheck.Add(CityCreated);
        }
        //we check if the endcity has a minimum of 1 city connected
        if (EndCity.GetComponent<City_Variables>().CitiesConnected.Count == 0)
        {
            objs = GameObject.FindGameObjectsWithTag("City");
            //Debug.Log(GameObject.FindGameObjectsWithTag("City").Length);
            //we take a city from the list and compare it to all the other cities in the list
            GameObject LowestDistanceCity = StartCity;
            foreach (GameObject obj in objs)
            {
                if (obj != EndCity)
                { 
                    if (Vector3.Distance(EndCity.transform.position, obj.transform.position) < Vector3.Distance(EndCity.transform.position, LowestDistanceCity.transform.position))
                    {
                        LowestDistanceCity = obj;
                    }
                }
            }
            EndCity.GetComponent<City_Variables>().CitiesConnected.Add(LowestDistanceCity);
            LowestDistanceCity.GetComponent<City_Variables>().CitiesConnected.Add(EndCity);
        }
        //we check if the startcity has a minimum of 1 city connected
        if (StartCity.GetComponent<City_Variables>().CitiesConnected.Count==0);
        {
            objs = GameObject.FindGameObjectsWithTag("City");
            //Debug.Log(GameObject.FindGameObjectsWithTag("City").Length);
            //we take a city from the list and compare it to all the other cities in the list
            GameObject LowestDistanceCity = EndCity;
            foreach (GameObject obj in objs)
            {
                if (obj != StartCity)
                {
                    if (Vector3.Distance(StartCity.transform.position, obj.transform.position) < Vector3.Distance(StartCity.transform.position, LowestDistanceCity.transform.position))
                    {
                        LowestDistanceCity = obj;
                    }
                }
            }
            StartCity.GetComponent<City_Variables>().CitiesConnected.Add(LowestDistanceCity);
            LowestDistanceCity.GetComponent<City_Variables>().CitiesConnected.Add(StartCity);
        }
        //We Check if a path exists between start and end and else create it
        //Check if we can make a while function somtimes its taking iterations to connect all cities
        if (!PathExist())
        { 
            List<GameObject> NewCitiesToCheck = new List<GameObject>();
            List<GameObject> CitiesChecked = new List<GameObject>();
            GameObject ActualCity = StartCity;
            GameObject EasternCity = StartCity;
            //we get all the gameobjects connected to start city
            NewCitiesToCheck.Add(ActualCity);
            while (NewCitiesToCheck.Count()>0)
            {

                ActualCity = NewCitiesToCheck[0];
                List<GameObject> TempList = new List<GameObject>();
                TempList = ActualCity.GetComponent<City_Variables>().CitiesConnected;
                foreach (GameObject City in TempList)
                {
                    if (!CitiesChecked.Contains(City)) NewCitiesToCheck.Add(City);

                    Debug.Log("proutCheck" +  City);

                }
                NewCitiesToCheck.Remove(ActualCity);
                CitiesChecked.Add(ActualCity);
                Debug.Log("proutActual" + ActualCity);
            }
            //we search the easternCity in gameobjects connected to startCity
            foreach (GameObject City in CitiesChecked)
            {
                Debug.Log("proutEasternCompare" + EasternCity + City);
                if (EasternCity.transform.position.x > City.transform.position.x) EasternCity = City;

            }
            Debug.Log("proutEastern" + EasternCity);
            objs = GameObject.FindGameObjectsWithTag("City");
            //we set the easterngame object with a connected city to the 1st city next to it
            GameObject LowestDistanceCity = EndCity;
            foreach (GameObject obj in objs)
            {
                if (EasternCity.transform.position.x > obj.transform.position.x)
                {
                    Debug.Log("prout" + obj);
                    if (Vector3.Distance(EasternCity.transform.position, obj.transform.position) < Vector3.Distance(EasternCity.transform.position, LowestDistanceCity.transform.position))
                    {
                        LowestDistanceCity = obj;
                    }
                }

            }
            EasternCity.GetComponent<City_Variables>().CitiesConnected.Add(LowestDistanceCity);
            LowestDistanceCity.GetComponent<City_Variables>().CitiesConnected.Add(EasternCity);
        }
        //we check if a city is alone and else create a route to this one
        objs = GameObject.FindGameObjectsWithTag("City");
        foreach (GameObject obj in objs)
        {
            if (obj.GetComponent<City_Variables>().CitiesConnected.Count()==0)
            {
                Debug.Log("proutAlone" + obj);
                GameObject LowestDistanceCity = EndCity;
                foreach (GameObject objc in objs)
                {
                    if (obj!=objc && Vector3.Distance(obj.transform.position, objc.transform.position) < Vector3.Distance(obj.transform.position, LowestDistanceCity.transform.position))
                    {
                        LowestDistanceCity = obj;
                    }
                }
                obj.GetComponent<City_Variables>().CitiesConnected.Add(LowestDistanceCity);
                LowestDistanceCity.GetComponent<City_Variables>().CitiesConnected.Add(obj);
            }
        }
        

    }


    bool PathExist()
    {
        List<GameObject> NewCitiesToCheck = new List<GameObject>();
        List<GameObject> CitiesChecked = new List<GameObject>();
        GameObject StartCity = GameObject.Find("Bordeaux");
        GameObject EndCity = GameObject.Find("Nice");
        GameObject ActualCity = StartCity;
        NewCitiesToCheck.Add(StartCity);
        while (ActualCity != EndCity)
        {
            if (NewCitiesToCheck.Count() == 0) return false;
            ActualCity = NewCitiesToCheck[0];
            List<GameObject> TempList = new List<GameObject>(); 
            TempList = ActualCity.GetComponent<City_Variables>().CitiesConnected;
            foreach (GameObject City in TempList)
            {
                if (!CitiesChecked.Contains(City)) NewCitiesToCheck.Add(City);
            }
            NewCitiesToCheck.Remove(ActualCity);
            CitiesChecked.Add(ActualCity);
            
        }
        return true;
    }


    //Check if the finished map reach all conditions 
    bool MapCondition()
    {
        GameObject EndCity = GameObject.Find("Nice");
        if (EndCity.GetComponent<City_Variables>().CitiesConnected.Count == 0)
        {
            Debug.Log("probleme endcity trouvé");
            return true;
        }
        else
        {
            Debug.Log("Pas de probleme endcity");
            GameObject StartCity = GameObject.Find("Bordeaux");
            if (StartCity.GetComponent<City_Variables>().CitiesConnected.Count == 0)
            {
                Debug.Log("probleme startcity trouvé");
                return true;
            }
            else
            {
                Debug.Log("Pas de probleme startcity");
                return false;
            }
        }
        

    }

    //check if the point created randomly respond to all requirements

    public float MinimumDistanceBetweenCities = 1f;

    bool PointCondition(Vector3 CityPosition)
    {
        objs = GameObject.FindGameObjectsWithTag("City");
        foreach(GameObject obj in objs)
        {
            //check if the point is at the good distance of the origin city
            if (Vector3.Distance(CityPosition, obj.transform.position) < MinimumDistanceBetweenCities)
            {
                Debug.Log("point trop proche!" + CityPosition + obj.transform.position);
                return true;
            }
        }
        return false;
    }

    bool Map_in_construction (Queue<string> CitiesToCreate)
    {
        //Check if the EndCity has the maximum of cities connected or if there is no cities left to create
        if (CitiesToCreate.Count == 0) return false;
        else return true;
    }


    //Functions for Mapping Cities
    public Dictionary<GameObject, float> CityDistance = new Dictionary<GameObject, float>();

    // Start is called before the first frame update
    void CheckRoutes()
    {
        //Debug.Log("launching checkroutes");
        //we make a list with all cities existing in the scene
        objs = GameObject.FindGameObjectsWithTag("City");
        //Debug.Log(GameObject.FindGameObjectsWithTag("City").Length);
        //we take a city from the list and compare it to all the other cities in the list
        foreach (GameObject obj in objs)
        {
            obj.GetComponent<City_Variables>().CitiesConnected.Clear();
            foreach (GameObject objc in objs)// we get the city we want to compare from the list
            {
                objc.GetComponent<City_Variables>().CitiesConnected.Remove(obj);
                if (obj == objc) // if it is the same city, just pass 
                {
                    //Debug.Log("it is the same city!");

                }
                else // if it is a different city
                {
                    CityDistance.Add(objc, Vector3.Distance(objc.transform.position, obj.transform.position));
                }

            }
            Debug.Log("pour la ville" + obj);
            Debug.Log("voici les valeurs du dictionnaire avant traitement");
            foreach (KeyValuePair<GameObject, float> city in CityDistance.OrderBy(Key => Key.Value))
            {
                    Debug.Log("voici la valeur du dictionnaire" + city);
            }
            //Debug.Log("here is the connected cities for the city " + obj);
            int i = 0;
            foreach (KeyValuePair<GameObject, float> city in CityDistance.OrderBy(Key => Key.Value))
            {
                if (i <= MaximumCitiesConnected)
                {
                    Debug.Log("voici la valeur du dictionnaire en cours de traitement" + city);
                    //we set the both cities in the ConnectedCities List of each other
                    obj.GetComponent<City_Variables>().CitiesConnected.Add(city.Key);
                    city.Key.GetComponent<City_Variables>().CitiesConnected.Add(obj);
                    i += 1;
                }
                if (i > MaximumCitiesConnected) Debug.Log("ville " + city + "non connectée car trop de villes connectées ");
            }
                
            CityDistance.Clear();
            //remettre à jour les distances à chaque fois sinon on a des distances en fonction des premiers points créés...

        }

    }


}
