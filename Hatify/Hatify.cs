using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace Hatify
{
  [BepInPlugin("com.Nuxlar.Hatify", "Hatify", "1.1.0")]

  public class Hatify : BaseUnityPlugin
  {
    private Material hatMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Commando/matCommandoDualies.mat").WaitForCompletion();
    private GameObject hat = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Bandit2/mdlBandit2.fbx").WaitForCompletion().transform.GetChild(4).GetChild(2).GetChild(0).GetChild(6).GetChild(0).GetChild(2).GetChild(0).gameObject;
    private Material banditHatMat = Addressables.LoadAssetAsync<Material>("RoR2/Base/Bandit2/matBandit2AltColossus.mat").WaitForCompletion();

    public static ConfigEntry<float> commandoSize;
    public static ConfigEntry<float> banditSize;
    public static ConfigEntry<float> huntressSize;
    public static ConfigEntry<float> engiSize;
    public static ConfigEntry<float> engiTurretSize;
    public static ConfigEntry<float> engiWalkerTurretSize;
    public static ConfigEntry<float> artiSize;
    public static ConfigEntry<float> mercSize;
    public static ConfigEntry<float> loaderSize;
    public static ConfigEntry<float> acridSize;
    public static ConfigEntry<float> captainSize;
    public static ConfigEntry<float> railgunnerSize;
    public static ConfigEntry<float> fiendSize;
    private static ConfigFile HatifyConfig { get; set; }

    public void Awake()
    {
      HatifyConfig = new ConfigFile(Paths.ConfigPath + "\\com.Nuxlar.Hatify.cfg", true);
      commandoSize = HatifyConfig.Bind<float>("General", "Commando Hat Size", 1.3f, "The scale of the hat.");
      banditSize = HatifyConfig.Bind<float>("General", "Bandit Hat Size", 1f, "The scale of the hat.");
      huntressSize = HatifyConfig.Bind<float>("General", "Huntress Hat Size", 1.3f, "The scale of the hat.");
      engiSize = HatifyConfig.Bind<float>("General", "Engi Hat Size", 1.6f, "The scale of the hat.");
      engiTurretSize = HatifyConfig.Bind<float>("General", "Engi Turret Hat Size", 10f, "The scale of the hat.");
      engiWalkerTurretSize = HatifyConfig.Bind<float>("General", "Engi Walker Turret Hat Size", 8f, "The scale of the hat.");
      artiSize = HatifyConfig.Bind<float>("General", "Artificer Hat Size", 1f, "The scale of the hat.");
      mercSize = HatifyConfig.Bind<float>("General", "Merc Hat Size", 1.4f, "The scale of the hat.");
      loaderSize = HatifyConfig.Bind<float>("General", "Loader Hat Size", 1.5f, "The scale of the hat.");
      acridSize = HatifyConfig.Bind<float>("General", "Acrid Hat Size", 15f, "The scale of the hat.");
      captainSize = HatifyConfig.Bind<float>("General", "Captain Hat Size", 1.3f, "The scale of the hat.");
      railgunnerSize = HatifyConfig.Bind<float>("General", "Railgunner Hat Size", 1f, "The scale of the hat.");
      fiendSize = HatifyConfig.Bind<float>("General", "Fiend Hat Size", 1.4f, "The scale of the hat.");

      On.RoR2.CharacterModel.Start += CharacterModel_Start;
    }

    private void CharacterModel_Start(On.RoR2.CharacterModel.orig_Start orig, CharacterModel self)
    {
      orig(self);
      self.StartCoroutine(this.HatifyThese(self));
    }

    private void SetupHat(GameObject hat, CharacterModel model, Vector3 size, Vector3 localPos, Vector3 rotation)
    {
      Material mat = model.body.name == "Bandit2Body(Clone)" ? this.banditHatMat : this.hatMat;
      gameObject.AddComponent<NetworkIdentity>();
      gameObject.transform.localScale = size;
      gameObject.transform.localPosition = localPos;
      gameObject.transform.Rotate(rotation);
      gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
      List<CharacterModel.RendererInfo> rendererInfos = ((IEnumerable<CharacterModel.RendererInfo>)model.baseRendererInfos).ToList<CharacterModel.RendererInfo>();
      Renderer[] rendererArray = gameObject.GetComponentsInChildren<Renderer>();
      for (int index = 0; index < rendererArray.Length; ++index)
      {
        Renderer renderer = rendererArray[index];
        rendererInfos.Add(new CharacterModel.RendererInfo()
        {
          renderer = renderer,
          defaultMaterial = renderer.sharedMaterial,
          defaultShadowCastingMode = renderer.shadowCastingMode,
          hideOnDeath = false,
          ignoreOverlays = false
        });
        renderer = (Renderer)null;
      }
      rendererArray = (Renderer[])null;
      model.baseRendererInfos = rendererInfos.ToArray();
      rendererInfos = (List<CharacterModel.RendererInfo>)null;
    }

    private IEnumerator HatifyThese(CharacterModel model)
    {
      yield return new WaitForFixedUpdate();
      if ((bool)model.body)
      {
        GameObject hatObject = null;
        Vector3 hatSize = Vector3.zero;
        switch (model.body.name)
        {
          case "Bandit2Body(Clone)":
            if (model.GetComponent<ModelSkinController>().currentSkinIndex == 2)
            {
              hatSize = new Vector3(banditSize.Value, banditSize.Value, banditSize.Value);
              if (hatSize != Vector3.zero)
              {
                hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(4).GetChild(2).GetChild(0).GetChild(6).GetChild(0).GetChild(2));
                SetupHat(hatObject, model, new Vector3(banditSize.Value, banditSize.Value, banditSize.Value), new Vector3(0f, 0.15f, 0f), new Vector3(10f, 0.0f, 0.0f));
              }
            }
            break;
          case "CaptainBody(Clone)":
            hatSize = new Vector3(captainSize.Value, captainSize.Value, captainSize.Value);
            if (hatSize != Vector3.zero)
            {
              Transform captainHead = model.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0);
              Transform captainHat = captainHead.Find("CaptainHat");
              if (captainHat)
              {
                captainHat.gameObject.SetActive(false);
              }
              hatObject = Object.Instantiate<GameObject>(this.hat, captainHead);
              SetupHat(hatObject, model, hatSize, new Vector3(0f, 0.15f, 0), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "CommandoBody(Clone)":
            hatSize = new Vector3(commandoSize.Value, commandoSize.Value, commandoSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(3).GetChild(0).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.3f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "RailgunnerBody(Clone)":
            hatSize = new Vector3(railgunnerSize.Value, railgunnerSize.Value, railgunnerSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(5).GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetChild(2).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.175f, -0.025f), new Vector3(30f, 0.0f, 0.0f));
            }
            break;
          case "MageBody(Clone)":
            hatSize = new Vector3(artiSize.Value, artiSize.Value, artiSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0).GetChild(2).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.15f, -0.1f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "HuntressBody(Clone)":
            hatSize = new Vector3(huntressSize.Value, huntressSize.Value, huntressSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(2).GetChild(3).GetChild(0).GetChild(2).GetChild(0).GetChild(1));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.3f, -0.05f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "CrocoBody(Clone)":
            hatSize = new Vector3(acridSize.Value, acridSize.Value, acridSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(5).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0f, 1.6f), new Vector3(55f, 180f, 180f));
            }
            break;
          case "EngiBody(Clone)":
            hatSize = new Vector3(engiSize.Value, engiSize.Value, engiSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.65f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "EngiWalkerTurretBody(Clone)":
            hatSize = new Vector3(engiWalkerTurretSize.Value, engiWalkerTurretSize.Value, engiWalkerTurretSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 1f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "EngiTurretBody(Clone)":
            hatSize = new Vector3(engiTurretSize.Value, engiTurretSize.Value, engiTurretSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(1).GetChild(0).GetChild(4).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.3f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "MercBody(Clone)":
            hatSize = new Vector3(mercSize.Value, mercSize.Value, mercSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(3).GetChild(0).GetChild(3).GetChild(1));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.2f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "VoidSurvivorBody(Clone)":
            hatSize = new Vector3(fiendSize.Value, fiendSize.Value, fiendSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(7).GetChild(0).GetChild(0).GetChild(1).GetChild(2).GetChild(2).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.1f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          case "LoaderBody(Clone)":
            hatSize = new Vector3(loaderSize.Value, loaderSize.Value, loaderSize.Value);
            if (hatSize != Vector3.zero)
            {
              hatObject = Object.Instantiate<GameObject>(this.hat, model.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(5).GetChild(0).GetChild(3).GetChild(0));
              SetupHat(hatObject, model, hatSize, new Vector3(0.0f, 0.2f, 0.0f), new Vector3(15f, 0.0f, 0.0f));
            }
            break;
          default:
            break;
        }
      }
    }
  }
}