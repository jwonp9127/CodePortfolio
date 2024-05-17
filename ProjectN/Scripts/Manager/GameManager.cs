using UnityEngine;

public class GameManager : MonoBehaviour
{
	private static GameManager _instance;
	
	public static GameManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameObject(typeof(GameManager).Name).AddComponent<GameManager>();
			}
			return _instance;
		}
	}

	public DataManager dataManager;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);

		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
			return;
		}

		_instance = this;
	}
}
