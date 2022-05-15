using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Logic
{
	public class SceneLoader : MonoBehaviour
	{
		public void ReloadCurrentScene()
		{
			string currentSceneName = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene(currentSceneName);
		}
	}
}