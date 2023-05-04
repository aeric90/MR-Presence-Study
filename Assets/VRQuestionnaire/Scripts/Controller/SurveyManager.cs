using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class SurveyManager : MonoBehaviour
{
    public static SurveyManager instance = null;
    public int surveyID;
    //Variables connected to gameobjects (metadata)
    public Transform questionPositions;
    public TextMeshPro title;
    public TextMeshPro description;
    public int numberofQuestions { get; set; }
    public GameObject pageContainer;
    public GameObject submitButton;
    //Variables for internal use in this class
    int numberOfPositions;
    int numberOfPages;
    int currentPage = 0;
    int surveyPage;
    int posIndex;
    int questionIndex;
    bool isCleanedVacantContainers = false;
    public int lastIndexFromPreviousSurvey;
    // Contains the data received from google forms
    private GoogleSurveyData googleSurveyData;
    // Keeps track of questions on each page to view them according to the page we are currently on.
    Dictionary<int, List<PageQuestion>> questionsPerPage;

    private List<GameObject> QuestionPrefabs = new List<GameObject>();
    void Awake()
    {
        if (instance == null) instance = this;
        surveyPage = 0;
        posIndex = 0;
        questionIndex = 0;
        numberOfPositions = questionPositions.childCount;
        googleSurveyData = new GoogleSurveyData();
        questionsPerPage = new Dictionary<int, List<PageQuestion>>();
    }
    void Start()
    {
        Setup(surveyID);
    }

    public void Setup(int surveyID)
    {
        Debug.Log("SETTING UP SURVEY " + surveyID);
        //Data should have been loaded in PlayfabController since this will trigger when clicking on the button tab
        this.googleSurveyData = PlayFabController.PFC.googleSurveyData[surveyID];
        lastIndexFromPreviousSurvey = PlayFabController.PFC.latestIndex;
        if (googleSurveyData == null)
        {
            Debug.LogError("googleSurveyData has not been aquired, please reach out to developer!");
            return;
        }

        foreach (GoogleQuestionData item in googleSurveyData.items)
        {
            if(GameController.instance.currentTrialID > 0) { 
                item.index += 6 * (GameController.instance.currentTrialID - 1) + 3;
            }
        }
        updateMetaData();


        // Instantiates question prefabs at the positions of the Containers gameobjects in the inspector
        for (int i = 0; i < numberofQuestions; i++)
        {
            GameObject question = Instantiate(QuestionFactory.CreateQuestion(googleSurveyData.items[i]), questionPositions.GetChild(posIndex));
            QuestionPrefabs.Add(question);
            PageQuestion pageQuestion = question.AddComponent<PageQuestion>();
            pageQuestion.page = surveyPage;
            // Turn off all questions not on the first page, so the questions on the first page appear only.
            if (surveyPage != 0)
            {
                question.SetActive(false);
            }
            //This if loop will add each questions on the same page as "pageQuestions" to their respective page. So you can say for example page 0 has 3 pageQuestions. Keeps track of the numbers of questions on each page.
            if (questionsPerPage.ContainsKey(surveyPage))
            {
                questionsPerPage[surveyPage].Add(pageQuestion);
            }
            else
            {
                questionsPerPage[surveyPage] = new List<PageQuestion>() { pageQuestion };
            }
            //Increment page when the number of positions available on a page is full. 
            if (posIndex < numberOfPositions - 1)
            {
                posIndex += 1;
            }
            else
            // Reset the position ID because a new page starts with new positions to fill. When the last question (question ID = numberofQuestions) is reached we do not want to add pages anymore.
            {
                if (questionIndex < numberofQuestions - 1)
                {
                    surveyPage += 1;
                }
                posIndex = 0;
            }
            //Incrememnts question so that we can tell if we need to add a new page.
            questionIndex++;
        }

        numberOfPages = surveyPage;
        SetUpNumberScene();
        CleanUpVacantQuestionContainers();
    }

    private void Update()
    {
        //Before cleaning up, this is the last page so we need to activate the submit button only if the questions satisfy the isRequired condition.
        submitButton.SetActive(QuestionsSatisfyIsRequired());

    }
    public void Prev()
    {
        // We return back to normal execution if the currentPage is the first page. 
        if (currentPage <= 0)
        {
            return;
        }

        // This for loop deactivates all questions on the current page. 
        foreach (PageQuestion pageQuestion in questionsPerPage[currentPage])
        {
            pageQuestion.gameObject.SetActive(false);
        }

        // Decrement the current page ( as if you went a page back) & 
        currentPage -= 1;
        ResetContainers();

        // This for loop activates all question on the previous page. Illusion that we switched the page. Hence we have a dictionary to keep track of which questions are on which page.
        foreach (PageQuestion pageQuestion in questionsPerPage[currentPage])
        {
            pageQuestion.gameObject.SetActive(true);
        }
        SetUpNumberScene();
        CleanUpVacantQuestionContainers();
    }

    public void Next()
    {

        //You can't virtually go up a page since you are on the last page
        if (currentPage >= numberOfPages)
        {
            return;
        }

        //Deactive the questionsItems on the current page
        foreach (PageQuestion pageQuestion in questionsPerPage[currentPage])
        {
            pageQuestion.gameObject.SetActive(false);
        }

        // Increment to the page as if you went a page next & 
        currentPage += 1;
        // When a the last page has less questions than the number of positions, the containers of questions are cleaned so that they do not appear empty. There removing some containers. When we return to a page that is full of questions and needs
        // the containers, we need to reset the containers everytime.
        ResetContainers();

        // Activate the questionsItems on  the next page.
        foreach (PageQuestion pageQuestion in questionsPerPage[currentPage])
        {
            pageQuestion.gameObject.SetActive(true);
        }
        // This will check if there are vacant containers. Meaning we have less questions than the number of positions assigned, therefore it removes (or cleans up) the containers not needed.
        CleanUpVacantQuestionContainers();
        SetUpNumberScene();
    }

    private void updateMetaData()
    {
        title.text = googleSurveyData.metadata.title;
        description.text = googleSurveyData.metadata.description;
        numberofQuestions = googleSurveyData.metadata.count;
    }

    void ResetContainers()
    {
        if (currentPage != numberOfPages && isCleanedVacantContainers)
        {
            for (int i = numberOfPositions - 1; i >= 0; i--)
            {
                questionPositions.GetChild(i).GetComponentInChildren<Container>(true).gameObject.SetActive(true);
            }

            isCleanedVacantContainers = false;
        }
    }
    void CleanUpVacantQuestionContainers()
    {
        //Cleaning up question containers that have no been used if the questions fill in the containers then we do nothing.
        if (currentPage == numberOfPages)
        {
            if (numberofQuestions % numberOfPositions == 0) { return; }

            int vacantContainers = numberOfPositions - numberofQuestions % numberOfPositions;

            for (int i = numberOfPositions - 1; i >= 0; i--)
            {
                questionPositions.GetChild(i).GetComponentInChildren<Container>().gameObject.SetActive(false);
                vacantContainers--;

                if (vacantContainers == 0) { break; }
            }

            isCleanedVacantContainers = true;
        }
    }

    private bool QuestionsSatisfyIsRequired()
    {
        //If this is not the last page we do not want the button to apear!
        if (currentPage == numberOfPages)
        {
            //return true only if all questions that are required have an answer!
            foreach (var question in FindObjectsOfType<Question>(true))
            {
                if (question._isRequired == true && question._answer == "No Answer")
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    private void SetUpNumberScene()
    {
        pageContainer.GetComponentInChildren<TextMeshPro>().text = "Page " + (currentPage + 1) + "/" + (numberOfPages + 1);
    }
}
   


