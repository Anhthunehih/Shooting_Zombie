using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using UnityEngine;
using System.Threading.Tasks;
using ZXing.Client.Result;
using System.Numerics;
using Thirdweb.Examples;


namespace Thirdweb.Examples
{
    public class StartScreenScript : MonoBehaviour
    {

        public GameObject StartGameState;
        public GameObject ClaimGameState;

        string address;
        Contract contract;
    

        // Start is called before the first frame update
        void Start()
        {
            contract = ThirdwebManager.Instance.SDK.GetContract("0x33EF3b9a57dFf0bcb0855caf46b7361B2192dcAC");
            StartGameState.SetActive(false);
            ClaimGameState.SetActive(false);
       

        }

        // Update is called once per frame
        void Update()
        {

        }
        public async void toogleStartScreen(GameObject ConnectedState, GameObject DisconnectedState, string address)// kết nối
        {
            ConnectedState.SetActive(true);
            DisconnectedState.SetActive(false);
            string stringBalance= await checkBalance(address);
            float floatBalance= float.Parse(stringBalance);

            if(floatBalance > 0.0)
            {
                StartGameState.SetActive(true) ;
                ClaimGameState.SetActive(false);
            }
            else
            {
                ClaimGameState.SetActive(true ) ;
                StartGameState.SetActive(false);
            }
        }
        public void toogleEndScreen(GameObject ConnectedState, GameObject DisconnectedState)// không kết nối
        {
            ConnectedState.SetActive(false);
            DisconnectedState.SetActive(true);
        }
        public async Task<string> checkBalance(string address)
        {
          contract = ThirdwebManager.Instance.SDK.GetContract("0x33EF3b9a57dFf0bcb0855caf46b7361B2192dcAC");
          string balance = await contract.Read<string>("balanceOf", address, 0);
          return balance;
        }

       
    }
}
