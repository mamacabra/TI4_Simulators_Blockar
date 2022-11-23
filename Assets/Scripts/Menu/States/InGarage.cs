using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu.States
{
    public class InGarage : IMenuState
    {
        private readonly Menu menu;

        public InGarage(Menu menu)
        {
            this.menu = menu;
            SceneManager.LoadScene("SampleCarCreation");
            EditModeLevelController.BackToEditScene();
        }
        
        public void Enter()
        {
            AudioManager.Instance.Play("Music 1", true);

            AudioManager.Instance.Stop("Motor");
            menu.inGarage.SetActive(true);
            menu.garageBlocks.SetActive(true);
        }

        public void Update()
        {
            if (!Input.GetKeyDown(KeyCode.C)) return;
            
            SceneManager.LoadScene(EditModeLevelController.instance.currentScene);
            IMenuState newState = new InGame(menu);
            menu.SetState(newState);
        }

        public void Exit() {}
    }
}