using UnityEngine;

public class Hints : MonoBehaviour
{
	public static int id;

	public bool OverrideHint = true;

	public int ForceShowId = -1;

	public GUI3DObject Icon;

	public string[] AverageExcludeScenes;

	public string[] GoodExcludeScenes;

	private Hint[] hints;

	private GUI3DText text;

	private void OnEnable()
	{
		if (this.text == null)
		{
			this.text = GetComponent<GUI3DText>();
		}
		if (hints == null)
		{
			InitHints();
		}
		if (!(this.text != null))
		{
			return;
		}
		if (ShowHints())
		{
			if (ForceShowId != -1)
			{
				id = ForceShowId;
			}
			else if (OverrideHint)
			{
				id = Random.Range(0, hints.Length);
			}
			string text = MonoBehaviorSingleton<GUI3DLocalization>.Instance.GetText("EmptyProxyScene_GUI", hints[id].HintId, "!BAD_TEXT!");
			this.text.SetDynamicText(text);
			if (Icon != null)
			{
				Icon.TextureName = hints[id].Icon;
				Icon.transform.localScale *= hints[id].IconScale;
				Icon.CreateOwnMesh = true;
				Icon.CreateMesh();
			}
		}
		else
		{
			this.text.SetDynamicText(string.Empty);
		}
	}

	private bool ShowHints()
	{
		string[] array = null;
		array = ((!Profile.GreaterThan(PerformanceScore.AVERAGE)) ? AverageExcludeScenes : GoodExcludeScenes);
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (DedalordLoadLevel.GetLevel() == text)
			{
				return false;
			}
		}
		return true;
	}

	private void InitHints()
	{
		hints = new Hint[61]
		{
			new Hint("HINT_1", "StoreItem_Skill_Double_Jump", 0.6f),
			new Hint("HINT_2", "StoreItem_Skill_WallGrip", 0.6f),
			new Hint("HINT_3", "icon-survival"),
			new Hint("HINT_4", "StoreItem_Skill_Double_Jump", 0.6f),
			new Hint("HINT_5", "icon-survival"),
			new Hint("HINT_6", "StoreItem_Skill_LifeSlot", 0.6f),
			new Hint("HINT_7", "StoreItem_Skill_ChickenFlap", 0.6f),
			new Hint("HINT_8", "Hints/Dive", 0.8f),
			new Hint("HINT_9", "StoreItem_Skill_Double_Jump", 0.6f),
			new Hint("HINT_10", "StoreItem_Skill_WallGrip", 0.6f),
			new Hint("HINT_11", "StoreItem_Skill_WallBounce", 0.6f),
			new Hint("HINT_12", "Hints/Didyouknow"),
			new Hint("HINT_13", "Hints/Runyoufools"),
			new Hint("HINT_14", "StoreItem_Skill_Recovery", 0.6f),
			new Hint("HINT_15", "Hints/Didyouknow"),
			new Hint("HINT_16", "Hints/Didyouknow"),
			new Hint("HINT_17", "icon-getskullies"),
			new Hint("HINT_18", "Hints/Gore"),
			new Hint("HINT_19", "Fredname"),
			new Hint("HINT_20", "Hints/Landzones"),
			new Hint("HINT_21", "Hints/ERDoctors"),
			new Hint("HINT_22", "Hints/Irresistible"),
			new Hint("HINT_23", "Hints/Tentacles"),
			new Hint("HINT_24", "Hints/Summon"),
			new Hint("HINT_25", "StoreItem_Skill_SkullyMagnet", 0.6f),
			new Hint("HINT_26", "icon-challenge", 0.6f),
			new Hint("HINT_27", "icon-challenge", 0.6f),
			new Hint("HINT_28", "icon-challenge", 0.6f),
			new Hint("HINT_29", "StoreItem_Skill_LifeSlot", 0.6f),
			new Hint("HINT_30", "Treasure"),
			new Hint("HINT_31", "Treasure"),
			new Hint("HINT_32", "icon-survival"),
			new Hint("HINT_33", "Hints/Brains"),
			new Hint("HINT_34", "Rating"),
			new Hint("HINT_35", "MisteryBoxIcon", 0.8f),
			new Hint("HINT_36", "MisteryBoxIcon", 0.8f),
			new Hint("HINT_37", "Hints/Didyouknow"),
			new Hint("HINT_38", "Gore"),
			new Hint("HINT_39", "icon-store", 0.8f),
			new Hint("HINT_40", "StoreItem_Skill_WallBounce", 0.6f),
			new Hint("HINT_41", "Hints/TyF"),
			new Hint("HINT_42", "Hints/TyF"),
			new Hint("HINT_43", "icon-store", 0.8f),
			new Hint("HINT_44", "Gore"),
			new Hint("HINT_45", "StoreItem_Consumable_SafetySpring", 0.6f),
			new Hint("HINT_46", "StoreItem_Consumable_ProtectiveVest", 0.6f),
			new Hint("HINT_47", "StoreItem_Consumable_Resurrect", 0.6f),
			new Hint("HINT_48", "StoreItem_Consumable_AfterBurner", 0.6f),
			new Hint("HINT_49", "StoreItem_Consumable_AfterBurner", 0.6f),
			new Hint("HINT_50", "FredAndWings", 1f),
			new Hint("HINT_51", "GrimmyIdol", 0.6f),
			new Hint("HINT_52", "StoreItem_Gear_PipFred", 0.6f),
			new Hint("HINT_53", "BouncerIcon", 1f),
			new Hint("HINT_54", "BlowerIcon", 1f),
			new Hint("HINT_55", "CatapultIcon", 1f),
			new Hint("HINT_56", "VatIcon", 1f),
			new Hint("HINT_64", "Hints/Didyouknow"),
			new Hint("HINT_65", "Hints/Didyouknow"),
			new Hint("HINT_66", "Hints/icon-superfallingfred", 0.8f),
			new Hint("HINT_67", "Hints/icon-psychoban"),
			new Hint("HINT_68", "Hints/icon-skiingfred")
		};
	}
}
