using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Extensions;
using Utilities;

public class ScoreManager : PersistentSingleton<ScoreManager>
{
    public float roomOrganisationScore = 0;
    public int heroSatisfactionRating = 100;
    public int stolenItems = 0;

    private List<int> recordedHeroSatisfactionRatings = new List<int>();
    public int averageHeroSatisfactionRating
    {
        get
        {
            int total = 0;
            foreach(int rating in recordedHeroSatisfactionRatings)
            {
                total += rating;
            }

            return total / recordedHeroSatisfactionRatings.Count;
        }
    }

    public TextMesh organisationTextMesh;
    public TextMesh satisfactionTextMesh;

    public void ChangeSatisfaction(int satisfactionDifference)
    {
        heroSatisfactionRating += satisfactionDifference;

        if(heroSatisfactionRating < 0)
        {
            heroSatisfactionRating = 0;
            GameOver();
        }
        else if(heroSatisfactionRating > 100)
        {
            heroSatisfactionRating = 100;
        }

        recordedHeroSatisfactionRatings.Add(heroSatisfactionRating);
    }

    public void CalculateAndDisplayValues()
    {
        satisfactionTextMesh.text = heroSatisfactionRating.ToString();
        organisationTextMesh.text = RoomManager.instance.CalculateOverallScore().ToString();
    }

    public void GameOver()
    {
        // TODO: End the game.
    }
}
