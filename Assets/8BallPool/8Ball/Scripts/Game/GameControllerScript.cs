using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{

    public Text playerName;
    public Text opponentName;
    public Image playerImage;
    public Image opponentImage;
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject vibOn;
    public GameObject vibOff;
    public GameObject sfxOn;
    public GameObject sfxOff;
    public GameObject menuScreenObj;
    public GameObject settingsScreenObj;

    private Image imageClock1;
    private Image imageClock2;

    private Animator messageBubble;
    private Text messageBubbleText;

    private int currentImage = 1;

    public float playerTime;

    public GameObject cueController;
    private CueController cueControllerScript;
    public GameObject shotPowerObject;
    private ShotPowerScript shotPowerScript;

    private float messageTime = 0;
    private AudioSource[] audioSources;
    private bool timeSoundsStarted = false;

    int loopCount = 0;

    private float waitingOpponentTime = 0;
    // Use this for initialization
    void Start() {
        SoundManager.Instance.StopBackgroundMusic();
        audioSources = GetComponents<AudioSource>();
        shotPowerScript = shotPowerObject.GetComponent<ShotPowerScript>();
        cueControllerScript = cueController.GetComponent<CueController>();
        playerTime = GameManager.Instance.playerTime;
        imageClock1 = GameObject.Find("AvatarClock1").GetComponent<Image>();
        imageClock2 = GameObject.Find("AvatarClock2").GetComponent<Image>();

        messageBubble = GameObject.Find("MessageBubble").GetComponent<Animator>();
        messageBubbleText = GameObject.Find("BubbleText").GetComponent<Text>();

        if (GameManager.Instance.offlineMode) {
            GameObject.Find("Name1").GetComponent<Text>().text = StaticStrings.offlineModePlayer1Name;
            // if (GameManager.Instance.avatarMy != null)
            //     GameObject.Find("Avatar1").GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

            GameObject.Find("Name2").GetComponent<Text>().text = StaticStrings.offlineModePlayer2Name;
            GameObject.Find("Avatar2").GetComponent<Image>().color = Color.red;

            // if (GameManager.Instance.avatarOpponent != null)
            //     GameObject.Find("Avatar2").GetComponent<Image>().sprite = GameManager.Instance.avatarOpponent;
        } else {
            GameObject.Find("Name1").GetComponent<Text>().text = GameManager.Instance.nameMy;
            if (GameManager.Instance.avatarMy != null)
                GameObject.Find("Avatar1").GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

            GameObject.Find("Name2").GetComponent<Text>().text = GameManager.Instance.nameOpponent;

            if (GameManager.Instance.avatarOpponent != null)
                GameObject.Find("Avatar2").GetComponent<Image>().sprite = GameManager.Instance.avatarOpponent;
        }

        // GameObject.Find ("Name1").GetComponent <Text> ().text = GameManager.Instance.nameMy;
        // if (GameManager.Instance.avatarMy != null)
        //     GameObject.Find ("Avatar1").GetComponent <Image> ().sprite = GameManager.Instance.avatarMy;

        // GameObject.Find ("Name2").GetComponent <Text> ().text = GameManager.Instance.nameOpponent;

        // if (GameManager.Instance.avatarOpponent != null)
        //     GameObject.Find ("Avatar2").GetComponent <Image> ().sprite = GameManager.Instance.avatarOpponent;



        PlayerNameManage();

        playerTime = playerTime * Time.timeScale;


        if (GameManager.Instance.roomOwner) {
            showMessage(StaticStrings.youAreBreaking);
        } else {
            showMessage(GameManager.Instance.nameOpponent + " " + StaticStrings.opponentIsBreaking);
        }

        if (!GameManager.Instance.roomOwner)
            currentImage = 2;
    }

    // Update is called once per frame
    void Update() {
        if (!GameManager.Instance.stopTimer) {
            updateClock();
        }
    }


    private void updateClock() {
        float minus;
        if (currentImage == 1) {
            playerTime = GameManager.Instance.playerTime;
            if (GameManager.Instance.offlineMode)
                playerTime = GameManager.Instance.playerTime + GameManager.Instance.cueTime;
            minus = 1.0f / playerTime * Time.deltaTime;

            imageClock1.fillAmount -= minus;

            if (imageClock1.fillAmount < 0.25f && !timeSoundsStarted) {
                audioSources[0].Play();
                timeSoundsStarted = true;
            }

            if (imageClock1.fillAmount == 0) {
                //				imageClock1.fillAmount = 1;
                //				currentImage = 2;
                //				showMessage (GameManager.Instance.nameOpponent + " turn");
                audioSources[0].Stop();
                GameManager.Instance.stopTimer = true;
                shotPowerScript.resetCue();
                /*if (!GameManager.Instance.offlineMode)
                    PhotonNetwork.RaiseEvent(9, cueControllerScript.cue.transform.position, true, null);*/
                if(GameManager.Instance.offlineMode) {
                    GameManager.Instance.wasFault = true;
                    GameManager.Instance.cueController.setTurnOffline(true);
                }


                GameManager.Instance.cueController.ShotPowerIndicator.deactivate();
                GameManager.Instance.cueController.ShotPowerIndicator.resetCue();

                GameManager.Instance.cueController.cueSpinObject.GetComponent<SpinController>().hideController();

                GameManager.Instance.cueController.whiteBallLimits.SetActive(false);
                GameManager.Instance.ballHand.SetActive(false);

                showMessage("You " + StaticStrings.runOutOfTime);

                if (!GameManager.Instance.offlineMode) {
                    cueControllerScript.setOpponentTurn();
                }

            }

        } else {
            //Debug.Log(GameManager.Instance.opponentCueTime);
            playerTime = GameManager.Instance.playerTime;
            if (GameManager.Instance.offlineMode)
                playerTime = GameManager.Instance.playerTime + GameManager.Instance.opponentCueTime;
            minus = 1.0f / playerTime * Time.deltaTime;
            imageClock2.fillAmount -= minus;

            if (GameManager.Instance.offlineMode && imageClock2.fillAmount < 0.25f && !timeSoundsStarted) {
                audioSources[0].Play();
                timeSoundsStarted = true;
            }

            if (imageClock2.fillAmount == 0) {
                GameManager.Instance.stopTimer = true;

                if (GameManager.Instance.offlineMode) {
                    showMessage("You " + StaticStrings.runOutOfTime);
                } else {
                    showMessage(GameManager.Instance.nameOpponent + " " + StaticStrings.runOutOfTime);
                }

                //				imageClock2.fillAmount = 1;
                //				currentImage = 1;
                //				showMessage ("Your turn");

                if (GameManager.Instance.offlineMode) {
                    GameManager.Instance.wasFault = true;
                    GameManager.Instance.cueController.setTurnOffline(true);
                }
            }
        }

    }

    public void showMessage(string message) {


        //Debug.Log ("Time " + (Time.time - messageTime));
        //        if(Time.time - messageTime > )

        float timeDiff = Time.time - messageTime;

        //Debug.Log("Time diff: " + timeDiff);

        if (timeDiff > 6) {
            messageBubbleText.text = message;
            messageBubble.Play("ShowBubble");
            if (!message.Contains(StaticStrings.waitingForOpponent))
                Invoke("hideBubble", 5.0f);
            else {
                waitingOpponentTime = StaticStrings.photonDisconnectTimeout;
                StartCoroutine(updateMessageBubbleText());
            }
            messageTime = Time.time;
        } else {
            Debug.Log("Show message with delay");
            StartCoroutine(showMessageWithDelay(message, (6.0f - timeDiff) / 1.0f));
        }
    }

    public void hideBubble() {
        messageBubble.Play("HideBubble");
    }

    IEnumerator showMessageWithDelay(string message, float delayTime) {
        yield return new WaitForSeconds(delayTime);

        messageBubbleText.text = message;

        messageBubble.Play("ShowBubble");
        if (!message.Contains(StaticStrings.waitingForOpponent))
            Invoke("hideBubble", 5.0f);
        else {
            waitingOpponentTime = StaticStrings.photonDisconnectTimeout;
            StartCoroutine(updateMessageBubbleText());
        }
        messageTime = Time.time;

    }

    public IEnumerator updateMessageBubbleText() {
        yield return new WaitForSeconds(1.0f * 2);
        waitingOpponentTime -= 1;
        if (!GameManager.Instance.opponentDisconnected) {
            if (!messageBubbleText.text.Contains("disconnected from room"))
                messageBubbleText.text = StaticStrings.waitingForOpponent + " " + waitingOpponentTime;
        }
        if (waitingOpponentTime > 0 && !GameManager.Instance.opponentActive && !GameManager.Instance.opponentDisconnected) {
            StartCoroutine(updateMessageBubbleText());
        }
    }

    public void stopSound() {
        audioSources[0].Stop();
    }

    public void resetTimers(int currentTimer, bool showMessageBool) {

        stopSound();
        timeSoundsStarted = false;
        imageClock1.fillAmount = 1;
        imageClock2.fillAmount = 1;

        this.currentImage = currentTimer;

        if (GameManager.Instance.offlineMode) {
            if (showMessageBool) {

                if (currentTimer == 2) {
                    showMessage(StaticStrings.offlineModePlayer2Name + " turn");
                } else {
                    showMessage(StaticStrings.offlineModePlayer1Name + " turn");
                }

            }

        } else {
            if (currentTimer == 1 && showMessageBool) {
                showMessage("It's your turn");
            }
        }




        //        if (currentImage == 1) {
        //            currentImage = 2;
        //        } else {
        //            currentImage = 1;
        //            showMessage("It's your turn");
        //        }

        GameManager.Instance.stopTimer = false;
    }
    
    
    public void PlayerNameManage()
    {
        int index1 = DataManager.Instance.playerNo == 2 ? 1 : 0;
        int index2 = DataManager.Instance.playerNo == 2 ? 0 : 1;
        
        playerName.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
        opponentName.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index2].userName);

        Image img1 = playerImage;
        Image img2 = opponentImage;
        StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, img1));
        StartCoroutine(DataManager.Instance.GetImages(DataManager.Instance.joinPlayerDatas[index2].avtar, img2));
    }
    public string UserNameStringManage(string name)
    {
        if (name != null && name != "")
        {
            if (name.Length > 13)
            {
                name = name.Substring(0, 10) + "...";
            }
            else
            {
                name = name;
            }
        }
        return name;
    }

    
    
    #region Button Functions
    
    public void SettingsButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenSettingsScreen();
    }
    
    public void SettingsCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseSettingsScreen();
    }

    public void MenuButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        OpenMenuScreen();
    }
    
    public void MenuCloseButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        CloseMenuScreen();
    }
    
    void OpenSettingsScreen()
    {
        settingsScreenObj.SetActive(true);
    }
    
    void CloseSettingsScreen()
    {
        settingsScreenObj.SetActive(false);
    }
    
    void OpenMenuScreen()
    {
        menuScreenObj.SetActive(true);
    }
    
    void CloseMenuScreen()
    {
        menuScreenObj.SetActive(false);
    }


    
    
    public void SoundButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (soundOn.activeSelf)
        {
            DataManager.Instance.SetSound(1);
            SoundManager.Instance.StopBackgroundMusic();
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetSound(0);
            soundOff.SetActive(false);
            soundOn.SetActive(true);
            SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void VibrationButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (vibOn.activeSelf)
        {
            DataManager.Instance.SetVibration(1);
            //SoundManager.Instance.StopBackgroundMusic();
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
        else
        {
            DataManager.Instance.SetVibration(0);
            vibOff.SetActive(false);
            vibOn.SetActive(true);
            //SoundManager.Instance.StartBackgroundMusic();
        }
    }
    
    public void SfxButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        if (sfxOn.activeSelf)
        {
            sfxOn.SetActive(false);
            sfxOff.SetActive(true);
        }
        else
        {
            sfxOn.SetActive(true);
            sfxOff.SetActive(false);
        }
    }
    
    public void MenuSubButtonClick(int no)
    {
        SoundManager.Instance.ButtonClick();
        if (no == 1)
        {
            GameManager.Instance.resetAllData();
            Time.timeScale = 1;
            TestSocketIO.Instace.LeaveRoom();
            SoundManager.Instance.StartBackgroundMusic();
            SceneManager.LoadScene("Main");
        }
        else if (no == 2)
        {
            //OpenRuleScreen();
        }
        else if (no == 3)
        {
            //Shop
            //Instantiate(shopPrefab, shopPrefabParent.transform);
        }
    }

    
    private void ManageSoundButtons()
    {
        if (DataManager.Instance.GetSound() == 0)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }

        if (DataManager.Instance.GetVibration() == 0)
        {
            vibOn.SetActive(true);
            vibOff.SetActive(false);
        }
        else
        {
            vibOn.SetActive(false);
            vibOff.SetActive(true);
        }
    }


    

    #endregion


}
