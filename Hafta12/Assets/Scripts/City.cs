using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class City : MonoBehaviour
{
    public int money;
    public int day;
    public int curPopulation;
    public int curJobs;
    public int curFood;
    public int maxPopulation;
    public int maxJobs;
    public int incomePerJob;
    public TextMeshProUGUI statsText;
    public List<Building> buildings = new List<Building>();

    public static City instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        updateStatsText();
    }

    public void onPlaceBuilding(Building building)
    {
        money -= building.Preset.cost;
        maxPopulation += building.Preset.population;
        maxJobs += building.Preset.jobs;
        buildings.Add(building);
        updateStatsText();

    }

    public void onRemoveBuilding(Building building)
    {
        
        maxPopulation -= building.Preset.population;
        maxJobs -= building.Preset.jobs;
        buildings.Remove(building);
        Destroy(building.gameObject);
        updateStatsText();
    }

    void updateStatsText()
    {
        statsText.text = string.Format("Day: {0} Money: {1} Pop: {2} / {3} Jobs: {4} / {5} Food: {6}" , new object[7] {day , money, curPopulation ,maxPopulation, curJobs ,maxJobs, curFood});
    }

    public void EndTurn()
    {
        day++;
        CalculateMoney();
        CalculatePopulation();
        CalculateJob();
        CalculateFood();
        updateStatsText();


    }

    void CalculateMoney()
    {
        money += curJobs * incomePerJob;

        foreach(Building building in buildings)
        {
            money -= building.Preset.costPerTurn;
        }
    }

    void CalculatePopulation()
    {
        if(curFood >= curPopulation && curPopulation < maxPopulation)
        {
            curFood -= curPopulation / 4;
            curPopulation = Mathf.Min(curPopulation + (curFood / 4), maxPopulation);
        }

        else if(curFood < curPopulation)
        {
            curPopulation = curFood;
        }
    }

    void CalculateJob()
    {
        curJobs = Mathf.Min(curPopulation, maxJobs);
    }

    void CalculateFood()
    {
        curFood = 0;

        foreach(Building building in buildings)
        {
            curFood += building.Preset.food;
        }
    }
}

