using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    // -----------------------------------------
    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(isFiring);
            stream.SendNext(Health);
        } else
        {
            this.isFiring = (bool)stream.ReceiveNext();
            this.Health = (float)stream.ReceiveNext();
        }
    }

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Public Fields

    [Tooltip("The current Health of our player")]
    public float Health = 1f;

    public static GameObject LocalPlayerInstance;

    [SerializeField]
    private GameObject playerUIPrefab;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Private Fields

    [Tooltip("The beams GameObjec to control")]
    [SerializeField]
    private GameObject beams;
    bool isFiring;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region MonoBehaviour Callbacks
    
    void Awake()
    {
        if(beams == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> Beams Reference.", this);
        } else
        {
            beams.SetActive(false);
        }

        if(photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
        if(_cameraWork != null)
        {
            if(photonView.IsMine)
            {
                _cameraWork.OnStartFollowing();
            }
        } else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

#if UNITY_5_4_OR_NEWER

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
         this.CalledOnLevelWasLoaded(scene.buildIndex);
        };

#endif
        if(playerUIPrefab != null) {
            GameObject _ui = Instantiate(playerUIPrefab);
            _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        } else {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
        }
    }

    void Update()
    {   
        if(photonView.IsMine)
        {
            ProcessInputs();
        }

        if (beams != null && isFiring != beams.activeInHierarchy)
        {
            beams.SetActive(isFiring);
        }
        
        if(Health <= 0f) 
        {
            GameManager.Instance.LeaveRoom();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(!other.name.Contains("Beam"))
        {
            return;
        }

        Health -= 0.1f;
    }

    void OnTriggerStay(Collider other)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if (!other.name.Contains("Beam"))
        {
            return;
        }

        Health -= 0.1f * Time.deltaTime;
    }

#if !UNITY_5_4_OR_NEWER
    void OnLevelWasLoaded(int level)
    {
        this.CalledOnLevelWasLoaded(level);
    }
#endif

    void CalledOnLevelWasLoaded(int level)
    {
        if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
        {
            transform.position = new Vector3(0f, 5f, 0f);
        }
        GameObject _ui = Instantiate(playerUIPrefab);
        _ui.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
    }

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Custom

    void ProcessInputs()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(!isFiring)
            {
                isFiring = true;
            }
        }

        if(Input.GetButtonUp("Fire1"))
        {
            if(isFiring) 
            {
                isFiring = false;
            }
        }
    }

    #endregion
    // -----------------------------------------
}
