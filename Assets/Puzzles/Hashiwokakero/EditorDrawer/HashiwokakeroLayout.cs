using UnityEngine;
using System.Collections;

//Reference - "Unity3D - 2D Array In Inspector" by Sumeet Khobare
//	Youtube Link - https://www.youtube.com/watch?v=uoHc-Lz9Lsc
//	Dropbox Link - https://www.dropbox.com/scl/fi/ayg7mbir2s445arlyf2ua/2DArray-In-Inspector-Scripts.zip?dl=0&e=1&rlkey=2olpj5a5131ujryls9pmykk5l

[System.Serializable]
public class HashiwokakeroLayout  {


	[System.Serializable]
	public struct rowData{
		public int[] row;
	}

	public rowData[] rows = new rowData[7]; //Grid of 7x7

	public int getValueAt(int x, int y)
	{
		return rows[y].row[x];
	}
}
