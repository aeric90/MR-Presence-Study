using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.UI;
using UnityEngine;

    public  static class QuestionFactory
    {
        private static Dictionary<string, Type> questionDictionary;
        private static bool IsInitialized => questionDictionary != null;
        private static GameObject questionPrefab;



        private static void InitializeFactory()
        {
            if (IsInitialized)
                return;
            
            var questionClasses = Assembly.GetAssembly(typeof(Question)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Question)));

            questionDictionary = new Dictionary<string, Type>();

            foreach (var questionTypeClass in questionClasses)
            {
                var tempQuestion = Activator.CreateInstance(questionTypeClass) as Question;
                questionDictionary.Add(tempQuestion.UnityType, questionTypeClass);
            }
        }

        public static GameObject CreateQuestion(GoogleQuestionData questionData)
        {
            InitializeFactory();

            if (questionDictionary.ContainsKey(questionData.type))
            {
            var question = Resources.Load<Question>(questionData.type).GetComponent<Question>();
            question.Instantiate(questionData);
            questionPrefab = question.gameObject;
            //Debug.Log(question._title.text);
            return questionPrefab;
            }

            return null;
        }

        public static Button CreateQuestionButton()
        {
            
        return null;
        }

        internal static IEnumerable<string>  GetQuestionNames()
        {
            InitializeFactory();
            return questionDictionary.Keys;
        }
    }

