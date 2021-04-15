using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SinglePlayerModeMain : MonoBehaviour
{
    private float totalTimeElapsed = 0;
    private int totalDeliviriesMade = 0;

    private float startCountdown = 3;

    public int startingDeliveriesAmount = 3;
    private int remainingDeliveries;

    private int round = 1;

    public ArrowIndicator indicator;

    public Player player;

    public ParticleSystem dollarsParticleSystem;

    private List<Building> allBuildings = new List<Building>();

    public UnityEngine.UI.Text tipMoneyText;
    public UnityEngine.UI.Text gainedTipMoneyText;
    public UnityEngine.UI.Text deliveriesText;

    public Timer timer;
    public GameObject victoryPanel;
    public GameObject lossPanel;

    public Transform pizzaPlace;

    private List<Building> buildings = new List<Building>();

    public GameObject pausedPanel;

    public AudioController audioController;
    public PlayAudio playAudio;
    public DialogueManager dialogueManager;

    private int runMoney = 0;

    void Start()
    {

        remainingDeliveries = startingDeliveriesAmount;

        tipMoneyText.text = "$" + PlayerPrefs.GetInt("money", 0).ToString();

        RefreshDeliveryPoints();

    }


    void RefreshDeliveryPoints()
    {
        allBuildings.Clear();
        buildings.Clear();

        GameObject[] _allBuildings = GameObject.FindGameObjectsWithTag("Building");

        foreach (GameObject go in _allBuildings)
        {
            allBuildings.Add(go.GetComponent<Building>());
        }

        List<int> usedBuildings = new List<int>();

        for (int i = 0; i < remainingDeliveries; i++)
        {
            int rnd = Random.Range(0, allBuildings.Count);

            if (i == 1)
            {

                while (Vector3.Distance(allBuildings[usedBuildings[i - 1]].transform.position,
                    allBuildings[rnd].transform.position) < 5000)
                {
                    rnd = Random.Range(0, allBuildings.Count);
                }

            }
            if (i == 2)
            {

                while (Vector3.Distance(allBuildings[usedBuildings[i - 2]].transform.position,
                    allBuildings[rnd].transform.position) < 5000 ||
                    Vector3.Distance(allBuildings[usedBuildings[i - 1]].transform.position,
                    allBuildings[rnd].transform.position) < 5000)
                {
                    rnd = Random.Range(0, allBuildings.Count);
                }

            }
            usedBuildings.Add(rnd);
        }


        for (int i = 0; i < usedBuildings.Count; i++)
        {
            allBuildings[usedBuildings[i]].SetAsDeliveryPoint(i, this);
            buildings.Add(allBuildings[usedBuildings[i]]);
        }

        indicator.target = allBuildings[usedBuildings[0]].transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForClosestBuilding();

        if (Input.GetButtonDown("Cancel"))
        {
            setPause();
        }

        if (startCountdown > 0)
        {
            startCountdown -= Time.deltaTime;
        }
        else if (!player.StartedRun)
        {
            player.StartedRun = true;
        }

        totalTimeElapsed += Time.deltaTime;
    }

    public void setPause()
    {
        audioController.PlaySound("SelectButton");

        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            pausedPanel.SetActive(false);
            audioController.PlaySound("UnPause");
            audioController.PlayMainSong(true);

        }
        else
        {
            Time.timeScale = 0;
            pausedPanel.SetActive(true);
            audioController.PlaySound("Pause");
            audioController.PauseMusic(false); 
        }
    }

    private void CheckForClosestBuilding()
    {

        List<float> buildingsDists = new List<float>();

        foreach (Building b in buildings)
        {
            if (b != null)
            {
                buildingsDists.Add(Vector3.Distance(player.transform.position, b.transform.position));
            }
            else // to compensate for when itereting delivered buildings
            {
                buildingsDists.Add(0);

            }
        }

        int closest = -1;

        for (int i = 0; i < buildingsDists.Count; i++)
        {
            if (buildings[i] != null)
            {
                if (closest < 0)
                {
                    closest = (int)buildingsDists[i];
                }
                if (closest > (int)buildingsDists[i])
                {
                    closest = (int)buildingsDists[i];
                }
            }
        }

        for (int i = 0; i < buildingsDists.Count; i++)
        {
            if (buildings[i] != null && closest == (int)buildingsDists[i])
            {
                indicator.target = buildings[i].transform;
                return;
            }
        }

    }


    public void SetDelivered(int deliveryPointIndex)
    {
        if (buildings[deliveryPointIndex] != null)
        {
            float extraTime = 0;

            if (round == 1)
            {
                extraTime = 10; 
            } else if (round == 2)
            {
                extraTime = 8;
            } else if (round == 3)
            {
                extraTime = 6;
            } else if (round == 4)
            {
                extraTime = 6;
            } else
            {
                extraTime = 3;
            }

            buildings[deliveryPointIndex] = null;
            totalDeliviriesMade += 1;
            remainingDeliveries -= 1;
            dollarsParticleSystem.Play(true);
            audioController.PlaySound("Money");
            audioController.PlaySound("DoorBell");

            // cycle through delivery audios
            // randomly generate a number
            // based on that number wew play the audio

            // call function to play an audio clip here

           // int audioFind = Random.Range(0, 4);

//            if (audioFind == 0)
  //          {
  //
    //        }
      //      else if (audioFind == 1)
        //    {


//        else if (audioFind == 2)
  //              {

    //            }
      //          else if (audioFind == 3)
        //        {

          //      }


                deliveriesText.text = remainingDeliveries.ToString(); // this updates the delivery amount near the pizza icon

            timer.timeRemaining += extraTime;
            int money = Random.Range(0, 5);
            runMoney += money; 

            // play a sound if the tip is $0
            if(money <= 0)
            {
                // grab an audio file from an array
                // audioController.PlaySound("Borrowers");
                //playAudio.PlaySound("NoTips");
               //audioController.PlayerSpeak("NoTips");
            }

            if (money <= 5)
            {
                // grab an audio file from an array
                //audioController.PlaySound("Borrowers");
               // audioController.PlayerSpeak("BigTips");
            }

            if (remainingDeliveries <= 0)
            {
                // time to get more pizzas audio
                // audioController.PlaySound("Brave");
                // playAudio.PlaySound("RoundStart");

              //  audioController.PlayerSpeak("RoundStart");

                player.PlayDeliveredFx(money, (int)extraTime);
                indicator.target = pizzaPlace;

                

                // this method shows GET MORE PIZZA text
                // player.PlayOutOfPizzas();
            }
            else
            {
                player.PlayDeliveredFx(money, (int)extraTime);
                money += PlayerPrefs.GetInt("money");
                PlayerPrefs.SetInt("money", money);
                tipMoneyText.text = "$" + PlayerPrefs.GetInt("money", 0).ToString();

            }

        }

    }


    public void playerEnteredPizzaPlace()
    {
        if (remainingDeliveries <= 0)
        {
            audioController.PlaySound("ExtraTime");

            // random number
            //System.Random random = new System.Random();
            //int genPizza = random.Next(1, 5);
            int genPizza = Random.Range(0, 5);
            remainingDeliveries = startingDeliveriesAmount + genPizza;
           // remainingDeliveries = startingDeliveriesAmount + round; // use this if you want to go back to the increment stuff
            round += 1;
            RefreshDeliveryPoints();
            deliveriesText.text = remainingDeliveries.ToString();

            // extra time on the clock
            float extraTime = timer.timeRemaining += 25;

            // display the extra time on the clock
            // not working at the moment
            player.PizzaRoundExtraTime((int)extraTime);

        }

    }


    public void RanOutOfTime()
    {
        player.FinishedRun = true;
        victoryPanel.SetActive(true);

        int minutes = (int)(totalTimeElapsed / 60);
        int seconds = (int)(totalTimeElapsed % 60);

        string strSeconds = seconds.ToString();
        if (seconds < 10)
        {
            strSeconds = "0" + seconds.ToString();
        }

        // end of panel play
   

        string tte = minutes.ToString() + ":" + seconds.ToString();

        gainedTipMoneyText.text = "Total Run Time: " + tte + " Minutes \n \n" +
            "Number of Pizzas Delivered: " + totalDeliviriesMade.ToString() + "\n \n" +
            "Tip Money Baby! $" + runMoney.ToString();
        audioController.PlaySound("Victory");
    }

    public void GoToTitleScreen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TitleScreen");
    }

    public void AnotherRound()
    {
        SceneManager.LoadScene("SinglePlayerMode");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
