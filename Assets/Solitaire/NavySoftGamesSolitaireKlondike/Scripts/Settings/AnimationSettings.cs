using UnityEngine;
public class AnimationSettings : ScriptableObject
{
	private static AnimationSettings _instance = null;
	public static AnimationSettings Instance
	{
		get 
		{
			if (_instance == null) _instance = (AnimationSettings)Resources.Load("AnimationSettings");
			return _instance;
		}
	}
	//Magindovid
	public Vector2[] TriangleUp;
	public Vector2[] TriangleDown;
	//Eye
	public Vector2[] LeftArrow;
	public Vector2[] RightArrow;
	public Vector2[] CenterCircl;
	//Perimetre
	public Vector2[] Perimetre;
	//Star
	public Vector2[] Star;
	//SquareRotation
//	public Vector2[] Perimetre;
	public Vector2[] Perimetre2;
	public Vector2[] Perimetre3;
	public Vector2[] Perimetre4;
	//TwoTraingles
	public Vector2[] PerTraingles1;
	public Vector2[] PerTraingles2;
	//FourTraingles
//	public Vector2[] PerTraingles1;
//	public Vector2[] PerTraingles2;
	public Vector2[] PerTraingles3;
	public Vector2[] PerTraingles4;
	//TwoScheduleTraingles
	public Vector2[] ScheduleTraingles1;
	public Vector2[] ScheduleTraingles2;
	//FourScheduleTraingles
//	public Vector2[] ScheduleTraingles1;
//	public Vector2[] ScheduleTraingles2;
	public Vector2[] ScheduleTraingles3;
	public Vector2[] ScheduleTraingles4;
	//ScheduleUpDown
	public Vector2[] ScheduleUp;
	public Vector2[] ScheduleDown;
	//OECD
	public Vector2[] OECDCircl;
	public Vector2[] OECDArrow1;
	public Vector2[] OECDArrow2;
	//EngFlag
	public Vector2[] VerLine;
	public Vector2[] HorLine;
	//Mountains
	public Vector2[] MountUp;
	public Vector2[] MountDown;
	//Fountain
	public Vector2[] Fountain;
	//MalteseCross
	public Vector2[] MalteseCross1;
	public Vector2[] MalteseCross2;
	public Vector2[] MalteseCross3;
	public Vector2[] MalteseCross4;
	//BigMalCross
	public Vector2[] BigMalCross1;
	public Vector2[] BigMalCross2;
	public Vector2[] BigMalCross3;
	public Vector2[] BigMalCross4;
	//CirclVerHor
	public Vector2[] CirclVerHor1;
	public Vector2[] CirclVer2;
	public Vector2[] CirclHor3;
	//Liliya
	public Vector2[] LiliyaLeft1;
	public Vector2[] LiliyaRight2;
	public Vector2[] LiliyaCentr3;
	public Vector2[] LiliyaHor4;
	//CirclLines
	public Vector2[] CirclLines1;
	public Vector2[] CirclLines2;
	public Vector2[] CirclLines3;
	public Vector2[] CirclLines4;
	public Vector2[] CirclLines5;
	public Vector2[] CirclLines6;
	public Vector2[] CirclLines7;
	public Vector2[] CirclLines8;
	public Vector2[] CirclLines9;
	//Atom
	public Vector2[] Atom1;
	public Vector2[] Atom2;
	public Vector2[] Atom3;
	public Vector2[] Atom4;
	//Triquctrum
	public Vector2[] Triquctrum1;
	public Vector2[] Triquctrum2;
	public Vector2[] Triquctrum3;
	public Vector2[] Triquctrum4;
	//Circulation
	public Vector2[] Circulation;
	//FourSquare
	public Vector2[] FourSquare1;
	public Vector2[] FourSquare2;
	public Vector2[] FourSquare3;
	public Vector2[] FourSquare4;
	//QuadroStar
	public Vector2[] QuadroStar1;
	public Vector2[] QuadroStar2;
	public Vector2[] QuadroStar3;
	//Labirint
	public Vector2[] Labirint1;
	public Vector2[] Labirint2;
	//Magendovid2
	public Vector2[] Magendovid1;
	public Vector2[] Magendovid2;
	//Hurt
	public Vector2[] Hurt1;
	public Vector2[] Hurt2;
	//Peak
	public Vector2[] Peak1;
	public Vector2[] Peak2;
	//Cross
	public Vector2[] Cross1;
	public Vector2[] Cross2;
	public Vector2[] Cross3;
	public Vector2[] Cross4;
	//ThreeCross
	public Vector2[] ThreeCross;
}