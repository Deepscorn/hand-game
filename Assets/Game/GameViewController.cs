using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using Game.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameViewController : MonoBehaviour
    {
        private const float ANIMATION_DURATION = 0.1f;
        
        [SerializeField] 
        private Toggle honestToggle;

        [SerializeField] 
        private Slider unhonestCoefSlider;

        [SerializeField] 
        private Text scoreText;

        [SerializeField] 
        private Text opponentDecisionText;

        [SerializeField] 
        private Text roundResultWin;
        
        [SerializeField] 
        private Text roundResultLose;
        
        [SerializeField] 
        private Text roundResultDraw;
        
        [SerializeField] 
        private Toggle stoneToggle;
        
        [SerializeField] 
        private Toggle scissorsToggle;
        
        [SerializeField] 
        private Toggle paperToggle;

        [SerializeField] 
        private ToggleGroup buttonsLayout;
        
        [SerializeField] 
        private GameObject unhonestyPanel;

        private Logic logic;
        private int playerScore;
        private int opponentScore;

        private void Awake()
        {
            logic = Logic.Instance;
            AddListener(stoneToggle, HandDecision.Stone);
            AddListener(scissorsToggle, HandDecision.Scissors);
            AddListener(paperToggle, HandDecision.Paper);
            
            unhonestyPanel.gameObject.SetActive(!honestToggle.isOn);
            honestToggle.onValueChanged.AddListener(on =>
            {
                unhonestyPanel.gameObject.SetActive(!on);
            });
        }

        private IEnumerator RoundRoutine(HandDecision playerDecision)
        {
            buttonsLayout.SetInteractable(false);
            
            HandDecision opponentDecision;
            if (honestToggle.isOn)
            {
                opponentDecision = logic.MakeOpponentDecision();
            }
            else
            {
                opponentDecision = logic.MakeOpponentDecision(
                    playerDecision, 
                    unhonestCoefSlider.value, 
                    playerScore, 
                    opponentScore);
            }

            yield return ShowOpponentDecision(opponentDecision);

            GameResult roundResult = logic.GetRoundResult(playerDecision, opponentDecision);
        
            yield return ShowRoundResult(roundResult);

            UpdateScore(roundResult);
            
            // TODO show user some notification like "press to continue"
            yield return new WaitWhile(() => Input.GetMouseButtonDown(0) == false);
            
            HideRoundResult();
            // TODO localization
            opponentDecisionText.text = "ваш ход";
            buttonsLayout.SetAllTogglesOff();
            buttonsLayout.SetInteractable(true);
        }

        private void UpdateScore(GameResult roundResult)
        {
            playerScore += roundResult != GameResult.PlayerLose ? 1 : 0;
            opponentScore += roundResult != GameResult.PlayerWins ? 1 : 0;

            string postfix = "";
            if (playerScore > opponentScore)
            {
                // in real app need localization
                postfix = "в вашу пользу";
            }

            if (playerScore < opponentScore)
            {
                // in real app need localization
                postfix = "в пользу ИИ";
            }

            scoreText.text = string.Format("{0}:{1} {2}", playerScore, opponentScore, postfix);
        }

        private void HideRoundResult()
        {
            GameResult[] allResults = EnumExt.GetValues<GameResult>();
            foreach (GameResult gameResult in allResults)
            {
                GetRoundResultText(gameResult).gameObject.SetActive(false);
            }
        }

        private IEnumerator ShowRoundResult(GameResult roundResult)
        {
            CanvasGroup resultText = GetRoundResultText(roundResult).gameObject.GetOrAddComponent<CanvasGroup>();
            resultText.gameObject.SetActive(true);
            
            resultText.alpha = 0;
            yield return resultText.DOFade(1, ANIMATION_DURATION).WaitForCompletion();
        }

        private Text GetRoundResultText(GameResult roundResult)
        {
            switch (roundResult)
            {
                case GameResult.Draw:
                    return roundResultDraw;
                case GameResult.PlayerLose:
                    return roundResultLose;
                case GameResult.PlayerWins:
                    return roundResultWin;
                default:
                    throw new NotImplementedException();
            }
        }

        private IEnumerator ShowOpponentDecision(HandDecision opponentDecision)
        {
            opponentDecisionText.text = ".";
            
            yield return new WaitForSeconds(ANIMATION_DURATION);
            opponentDecisionText.text = "..";
            
            yield return new WaitForSeconds(ANIMATION_DURATION);
            opponentDecisionText.text = "...";
            
            yield return new WaitForSeconds(ANIMATION_DURATION);
            opponentDecisionText.text = Localization.Localize(opponentDecision);
        }

        private void AddListener(Toggle decisionToggle, HandDecision decision)
        {
            decisionToggle.onValueChanged.AddListener(on =>
            {
                if (on)
                {
                    StartCoroutine(RoundRoutine(decision));
                }
            });
        }
    }
}