// mod made by fault#2022

using BepInEx;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.XR;
using Utilla;
using Photon.Pun;
using Photon.Realtime;

namespace BasketBallMod.Scripts
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

         GameObject basketball;
         GameObject hoop;

         AudioSource yipeeSound;

         GameObject LHand;
         GameObject RHand;
         GameObject Body;

        void Start()
        {
            /* A lot of Gorilla Tag systems will not be set up when start is called /*
			/* Put code in OnGameInitialized to avoid null references */

            Events.GameInitialized += OnGameInitialized;
        }

        void OnEnable()
        {
            /* Set up your mod here */
            /* Code here runs at the start and whenever your mod is enabled*/

            HarmonyPatches.ApplyHarmonyPatches();
        }

        void OnDisable()
        {
            /* Undo mod setup here */
            /* This provides support for toggling mods with ComputerInterface, please implement it :) */
            /* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

            HarmonyPatches.RemoveHarmonyPatches();
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("BasketBallMod.Assets.hoop");
            AssetBundle bundle2 = AssetBundle.LoadFromStream(str2);
            GameObject assets2 = bundle2.LoadAsset<GameObject>("basketballhoop");
            hoop = Instantiate(assets2);

            hoop.transform.name = "ForestHoop";

            hoop.transform.position = new Vector3(-69.11f, 5.83f, -70.235f);

            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("BasketBallMod.Assets.basketball");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject assets = bundle.LoadAsset<GameObject>("Ball");
            basketball = Instantiate(assets);
            basketball.transform.position = new Vector3(-66.999f, 11.4f, -82.786f);

            basketball.transform.name = "Basketball";

            basketball.GetComponent<Rigidbody>().useGravity = false;
            basketball.GetComponent<SphereCollider>().enabled = false;

            LHand = GameObject.Find("palm.02.L");
            RHand = GameObject.Find("palm.02.R");

            Body = GameObject.Find("body");

            yipeeSound = basketball.GetComponent<AudioSource>();
            yipeeSound.Stop();

        }

        void Update()
        {
            // grabbing stuff

            float Lgrip;
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.grip, out Lgrip);
            float Rgrip;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.grip, out Rgrip);
            float Ltrigger;
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.trigger, out Ltrigger);
            float Rtrigger;
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out Rtrigger);
            bool primaryButton;
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton);

            float _RDistance;
            _RDistance = Vector3.Distance(RHand.transform.position, basketball.transform.position);
            float _LDistance;
            _LDistance = Vector3.Distance(LHand.transform.position, basketball.transform.position); 

            float hoopR;
            hoopR = Vector3.Distance(RHand.transform.position, hoop.transform.position);
            float hoopL;
            hoopL = Vector3.Distance(LHand.transform.position, hoop.transform.position);

            if (_LDistance < 0.4)
            {
                if (Lgrip > 0.3)
                {
                    basketball.transform.parent = LHand.transform;
                    basketball.transform.rotation = LHand.transform.rotation;
                    basketball.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            if (_RDistance < 0.2)
            {
                if (Rgrip > 0.3)
                {
                    basketball.transform.parent = RHand.transform;
                    basketball.transform.rotation = RHand.transform.rotation;
                    basketball.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            if (hoopR < 0.6)
            {
                if (Rtrigger > 0.3)
                {
                    hoop.transform.parent = RHand.transform;
                    hoop.transform.rotation = RHand.transform.rotation;
                    hoop.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            if (hoopL < 0.4)
            {
                if (Ltrigger > 0.3)
                {
                    hoop.transform.parent = LHand.transform;
                    hoop.transform.rotation = LHand.transform.rotation;
                    hoop.transform.localPosition = new Vector3(0, 0, 0);
                }
            }

            if (Lgrip < 0.3)
            {
                basketball.transform.parent = null;
                hoop.transform.parent = null;
            }
            if (Rgrip < 0.3)
            {
                basketball.transform.parent = null;
                hoop.transform.parent = null;
            }

            if (primaryButton)
            {
                basketball.transform.position = Body.transform.position + new Vector3(-0.5f, 0, 0);
            }
        }
        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {


            inRoom = false;
        }
    }
}
