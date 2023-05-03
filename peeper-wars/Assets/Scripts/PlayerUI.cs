using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // -----------------------------------------
    #region Private Fields

    [SerializeField]
    private Text playerNameText;
    [SerializeField]
    private Slider playerHealthSlider;

    PlayerManager target;

    float charControllerHeight = 0f;
    Transform targetTransform;
    Vector3 targetPosition;

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Public Fields

    [SerializeField]
    private Vector3 screenOffset = new Vector3(0f, 30f, 0f);

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region MonoBehaviour Callbacks

    void Awake() {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
    }

    void Update() {
        if(playerHealthSlider != null){
            playerHealthSlider.value = target.Health;
        }

        if(target == null) {
            Destroy(this.gameObject);
            return;
        }
    }

    void LateUpdate()
    {
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += charControllerHeight;
            this.transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    #endregion
    // -----------------------------------------

    // -----------------------------------------
    #region Public Methods

    public void SetTarget(PlayerManager _target) {
        if(_target == null) {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        target = _target;
        targetPosition = target.transform.position;
        targetTransform = target.transform;
        CharacterController _charController = target.GetComponent<CharacterController>();
        if(_charController != null) {
            charControllerHeight = _charController.height;
        }

        if(playerNameText != null) {
            playerNameText.text = target.photonView.Owner.NickName;
        }
    }

    #endregion
    // -----------------------------------------
}