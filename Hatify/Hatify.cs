using static BodyModelAdditionsAPI.Main;
using BepInEx;
using BepInEx.Configuration;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Hatify
{
  [BepInPlugin("com.Nuxlar.Hatify", "Hatify", "1.3.2")]

  public class Hatify : BaseUnityPlugin
  {
    private static GameObject hat = Addressables.LoadAssetAsync<GameObject>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Bandit2.mdlBandit2_fbx).WaitForCompletion().transform.GetChild(4).GetChild(2).GetChild(0).GetChild(6).GetChild(0).GetChild(2).GetChild(0).gameObject;
    private static Material hatMat = Addressables.LoadAssetAsync<Material>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Commando.matCommandoDualies_mat).WaitForCompletion();
    private static Material banditHatMat = Addressables.LoadAssetAsync<Material>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Bandit2.matBandit2AltColossus_mat).WaitForCompletion();
    private static Material voidlingHatMat = Addressables.LoadAssetAsync<Material>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC1_voidraid.matVoidMetalTrimGrassyVertexColorsOnlyRaidBoss_mat).WaitForCompletion();
    private static List<string> bodyNames = new List<string>()
    {
      "Bandit2Body",
      "CaptainBody",
      "CommandoBody",
      "RailgunnerBody",
      "MageBody",
      "HuntressBody",
      "CrocoBody",
      "MercBody",
      "VoidSurvivorBody",
      "EngiBody",
      "EngiTurretBody",
      "EngiWalkerTurretBody",
      "LoaderBody",
      "SeekerBody",
      "FalseSonBody",
      "ChefBody",
      "TreebotBody",
      "ToolbotBody",
      "VoidRaidCrabBody"
    };

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
    public static ConfigEntry<float> seekerSize;
    public static ConfigEntry<float> rexSize;
    public static ConfigEntry<float> chefSize;
    public static ConfigEntry<float> falseSonSize;
    public static ConfigEntry<float> multSize;
    public static ConfigEntry<float> voidlingSize;
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
      seekerSize = HatifyConfig.Bind<float>("General", "Seeker Hat Size", 1f, "The scale of the hat.");
      rexSize = HatifyConfig.Bind<float>("General", "REX Hat Size", 2f, "The scale of the hat.");
      chefSize = HatifyConfig.Bind<float>("General", "CHEF Hat Size", 2f, "The scale of the hat.");
      falseSonSize = HatifyConfig.Bind<float>("General", "False Son Hat Size", 2.5f, "The scale of the hat.");
      multSize = HatifyConfig.Bind<float>("General", "MUL-T Hat Size", 15f, "The scale of the hat.");
      voidlingSize = HatifyConfig.Bind<float>("General", "Fathomless Hat Size", 200f, "The scale of the hat.");

      ApplyHats();
    }

    private static void ApplyHats()
    {
      hat.AddComponent<NetworkIdentity>();
      SkinDef banditColossusSkin = Addressables.LoadAssetAsync<SkinDef>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_Base_Bandit2.skinBandit2AltColossus_asset).WaitForCompletion();
      SkinDef chefSkin = Addressables.LoadAssetAsync<SkinDef>(RoR2BepInExPack.GameAssetPaths.Version_1_39_0.RoR2_DLC2_Chef.skinChefAlt_asset).WaitForCompletion();

      foreach (string bodyName in bodyNames)
      {
        Vector3 hatSize = Vector3.zero;
        switch (bodyName)
        {
          case "SeekerBody":
            hatSize = new Vector3(seekerSize.Value, seekerSize.Value, seekerSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItSeeker,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "FalseSonBody":
            hatSize = new Vector3(falseSonSize.Value, falseSonSize.Value, falseSonSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItFalseSon,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "ChefBody":
            hatSize = new Vector3(chefSize.Value, chefSize.Value, chefSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItChef,
                skinDef = chefSkin
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "ToolbotBody":
            hatSize = new Vector3(multSize.Value, multSize.Value, multSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItMult,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "TreebotBody":
            hatSize = new Vector3(rexSize.Value, rexSize.Value, rexSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "FlowerBase",
                codeAfterApplying = PlaceItRex,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "Bandit2Body":
            hatSize = new Vector3(banditSize.Value, banditSize.Value, banditSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItBandit,
                skinDef = banditColossusSkin
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "CaptainBody":
            hatSize = new Vector3(captainSize.Value, captainSize.Value, captainSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItCaptain,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "CommandoBody":
            hatSize = new Vector3(commandoSize.Value, commandoSize.Value, commandoSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItCommando,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "RailgunnerBody":
            hatSize = new Vector3(railgunnerSize.Value, railgunnerSize.Value, railgunnerSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItRailgunner,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "MageBody":
            hatSize = new Vector3(artiSize.Value, artiSize.Value, artiSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItMage,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "HuntressBody":
            hatSize = new Vector3(huntressSize.Value, huntressSize.Value, huntressSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItHuntress,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "CrocoBody":
            hatSize = new Vector3(acridSize.Value, acridSize.Value, acridSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItCroco,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "EngiBody":
            hatSize = new Vector3(engiSize.Value, engiSize.Value, engiSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Chest",
                codeAfterApplying = PlaceItEngi,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "EngiWalkerTurretBody":
            hatSize = new Vector3(engiWalkerTurretSize.Value, engiWalkerTurretSize.Value, engiWalkerTurretSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItEngiWalker,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "EngiTurretBody":
            hatSize = new Vector3(engiTurretSize.Value, engiTurretSize.Value, engiTurretSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItEngiTurret,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "MercBody":
            hatSize = new Vector3(mercSize.Value, mercSize.Value, mercSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItMerc,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "VoidSurvivorBody":
            hatSize = new Vector3(fiendSize.Value, fiendSize.Value, fiendSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItVoid,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "LoaderBody":
            hatSize = new Vector3(loaderSize.Value, loaderSize.Value, loaderSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItLoader,
              };
              new ModelPart(modelPartInfo);
            }
            break;
          case "VoidRaidCrabBody":
            hatSize = new Vector3(voidlingSize.Value, voidlingSize.Value, voidlingSize.Value);
            if (hatSize != Vector3.zero)
            {
              ModelPartInfo modelPartInfo = new ModelPartInfo
              {
                bodyName = bodyName,
                gameObject = hat,
                inputString = "Head",
                codeAfterApplying = PlaceItVoidling,
              };
              new ModelPart(modelPartInfo);
            }
            break;
        }
      }
    }

    static void PlaceItVoidling(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 17f, -12f);
      modelObject.transform.localScale = new Vector3(voidlingSize.Value, voidlingSize.Value, voidlingSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = voidlingHatMat;
    }

    static void PlaceItMult(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 1.5f, 1.7f);
      modelObject.transform.localScale = new Vector3(multSize.Value, multSize.Value, multSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(315f, 180f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItChef(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      Transform chefHat = characterModel.transform.Find("meshChefPizzaCutter");
      if (chefHat)
        chefHat.gameObject.SetActive(false);

      modelObject.transform.localPosition = new Vector3(-0.35f, 0f, 0f);
      modelObject.transform.localScale = new Vector3(chefSize.Value, chefSize.Value, chefSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(60f, 90f, 180f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItFalseSon(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 0.34f, -0.05f);
      modelObject.transform.localScale = new Vector3(falseSonSize.Value, falseSonSize.Value, falseSonSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(10f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItSeeker(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 0.2f, 0f);
      modelObject.transform.localScale = new Vector3(seekerSize.Value, seekerSize.Value, seekerSize.Value);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = banditHatMat;
    }
    static void PlaceItRex(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 2f, 0f);
      modelObject.transform.localScale = new Vector3(rexSize.Value, rexSize.Value, rexSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(10f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItBandit(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0f, 0.15f, 0f);
      modelObject.transform.localScale = new Vector3(banditSize.Value, banditSize.Value, banditSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(10f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = banditHatMat;
    }
    static void PlaceItCaptain(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      Transform armature = characterModel.transform.Find("CaptainArmature");
      if (armature)
      {
        Transform captainHat = armature.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(2).GetChild(0).GetChild(4);
        if (captainHat)
          captainHat.gameObject.SetActive(false);
      }
      modelObject.transform.localPosition = new Vector3(0f, 0.15f, 0f);
      modelObject.transform.localScale = new Vector3(captainSize.Value, captainSize.Value, captainSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItCommando(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.3f, 0.0f);
      modelObject.transform.localScale = new Vector3(commandoSize.Value, commandoSize.Value, commandoSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItRailgunner(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.18f, -0.025f);
      modelObject.transform.localScale = new Vector3(railgunnerSize.Value, railgunnerSize.Value, railgunnerSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(30f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItMage(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.15f, -0.1f);
      modelObject.transform.localScale = new Vector3(artiSize.Value, artiSize.Value, artiSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItHuntress(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.3f, -0.05f);
      modelObject.transform.localScale = new Vector3(huntressSize.Value, huntressSize.Value, huntressSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItCroco(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0f, 1.6f);
      modelObject.transform.localScale = new Vector3(acridSize.Value, acridSize.Value, acridSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(55f, 180f, 180f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItEngi(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.65f, 0.0f);
      modelObject.transform.localScale = new Vector3(engiSize.Value, engiSize.Value, engiSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItEngiWalker(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 1f, 0.0f);
      modelObject.transform.localScale = new Vector3(engiWalkerTurretSize.Value, engiWalkerTurretSize.Value, engiWalkerTurretSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItEngiTurret(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.3f, 0.0f);
      modelObject.transform.localScale = new Vector3(engiTurretSize.Value, engiTurretSize.Value, engiTurretSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItMerc(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
      modelObject.transform.localScale = new Vector3(mercSize.Value, mercSize.Value, mercSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItVoid(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.1f, 0.0f);
      modelObject.transform.localScale = new Vector3(fiendSize.Value, fiendSize.Value, fiendSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
    static void PlaceItLoader(GameObject modelObject, ChildLocator childLocator, CharacterModel characterModel, ActivePartsComponent activePartsComponent)
    {
      modelObject.transform.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
      modelObject.transform.localScale = new Vector3(loaderSize.Value, loaderSize.Value, loaderSize.Value);
      modelObject.transform.localEulerAngles = new Vector3(15f, 0.0f, 0.0f);
      modelObject.transform.GetChild(0).GetComponent<MeshRenderer>().material = hatMat;
    }
  }
}