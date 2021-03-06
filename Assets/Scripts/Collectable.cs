using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Collectable : MonoBehaviour
    {
        private GameObject Player;
        [SerializeField]  private float collectDistance = 2.5f;
        [SerializeField] private float collectionSpeed = 4; 

        private void Awake()
        {
            Player = GameObject.FindWithTag("Player");
        }

        private void Update()
        {
            if (IsNearGameObject(Player))
            {
                Collect(this);
            }
        }
        
        private void Collect(Collectable collectable)
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position, Time.deltaTime * collectionSpeed);
            var distance = Vector3.Distance(transform.position, Player.transform.position);
            if (distance <= 0.5f)
            {
                SeelenManager.instance.AddSoul();
                this.gameObject.SetActive(false);
            }
        }

        private bool IsNearGameObject(GameObject player)
        {
            return Vector3.Distance(this.transform.position, player.transform.position) < collectDistance;
        }
    }
}