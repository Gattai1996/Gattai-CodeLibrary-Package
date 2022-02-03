using System.Threading.Tasks;
using Gattai.Runtime.Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gattai.Runtime.Systems
{
    public class ScenePreloader : SingletonPersistent<ScenePreloader>
    {
        [SerializeField] private Canvas loadingScreenCanvas;
        [SerializeField] private GameObject[] loadingScreenProgressGameObjects;
        [SerializeField] private RawImage loadingScreenBackgroundImage;
        [SerializeField] private Slider progressBar;

        private float _targetProgress;
        private AsyncOperation _currentLoadingScene;
        
        private void Update()
        {
            progressBar.value =
                Mathf.MoveTowards(progressBar.value, _targetProgress, 3 * Time.deltaTime);
        }

        public async Task Preload(string sceneName)
        {
            _targetProgress = 0;
            progressBar.value = 0;
            ActivateProgressGameObjects(true);
            
            await Task.Delay(100);

            _currentLoadingScene = SceneManager.LoadSceneAsync(sceneName);
            _currentLoadingScene.allowSceneActivation = true;

            do
            {
                await Task.Delay(100);
                
                _targetProgress = _currentLoadingScene.progress;

            } while (!_currentLoadingScene.isDone);

            await Task.Delay(1000);
            
            ActivateProgressGameObjects(false);
        }

        private void ActivateProgressGameObjects(bool activate)
        {
            foreach (var o in loadingScreenProgressGameObjects)
            {
                o.SetActive(activate);
            }
        }
    }
}