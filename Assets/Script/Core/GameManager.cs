using System;
using System.Collections;
using UnityEngine;

//Сделать его неуничтожаемым и получать данные когда сцена была запущена. 
public class GameManager : MonoBehaviourSingleton<GameManager>
{
   public bool IsPlayGame = false;
   public float MoveSpeed = 0.1f;
   private PlayerController _playerController;
   
   private void Start()
   {
      _playerController = PlayerController.Instance;
      StartCoroutine(StartGameCor());
   }

   private void StartGame()
   { 
      //Дать управление игроку.
      IsPlayGame = true;
   }

   private void Update()
   {
      if(!IsPlayGame)
         return;

      _playerController.MovePosZ = MoveSpeed;
   }

   private IEnumerator StartGameCor()
   {
      yield return new WaitForSeconds(1f);
      StartGame();
   }
}
