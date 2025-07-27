using System.Collections.Generic;
using System.IO;
using UnityEngine;

[ExecuteInEditMode]
public class LevelRandomSet : MonoBehaviour
{
	public List<GameObject> chunkObjects;

	public Dictionary<int, List<Vector3>> objPos;

	public int AlwaysUseSet = -1;

	public string trapsPosEncoded;

	public Dictionary<int, List<Quaternion>> objRot;

	public string objRotEncoded;

	public Dictionary<int, List<Vector3>> objScale;

	public string objScaleEncoded;

	public int currentSet;

	private void Start()
	{
		RandomizeSet();
	}

	private void Update()
	{
	}

	public void EncodeData()
	{
		int count;
		int num;
		MemoryStream memoryStream;
		BinaryWriter binaryWriter;
		if (objPos != null)
		{
			count = objPos.Count;
			num = ((count > 0) ? objPos[0].Count : 0);
			memoryStream = new MemoryStream();
			binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(count);
			binaryWriter.Write(num);
			Debug.Log(string.Format("SetNum: {0} ObjNum: {1}", count, num));
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < num; j++)
				{
					binaryWriter.Write(objPos[i][j].x);
					binaryWriter.Write(objPos[i][j].y);
					binaryWriter.Write(objPos[i][j].z);
				}
			}
			trapsPosEncoded = StringUtil.EncodeTo64(memoryStream.GetBuffer());
			binaryWriter.Close();
			memoryStream.Close();
		}
		else
		{
			trapsPosEncoded = string.Empty;
		}
		if (objRot != null)
		{
			count = objRot.Count;
			num = ((count > 0) ? objRot[0].Count : 0);
			memoryStream = new MemoryStream();
			binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(count);
			binaryWriter.Write(num);
			for (int k = 0; k < count; k++)
			{
				for (int l = 0; l < num; l++)
				{
					binaryWriter.Write(objRot[k][l].x);
					binaryWriter.Write(objRot[k][l].y);
					binaryWriter.Write(objRot[k][l].z);
					binaryWriter.Write(objRot[k][l].w);
				}
			}
			objRotEncoded = StringUtil.EncodeTo64(memoryStream.GetBuffer());
			binaryWriter.Close();
			memoryStream.Close();
		}
		else
		{
			trapsPosEncoded = string.Empty;
		}
		if (objScale == null)
		{
			return;
		}
		count = objScale.Count;
		num = ((count > 0) ? objScale[0].Count : 0);
		memoryStream = new MemoryStream();
		binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(count);
		binaryWriter.Write(num);
		for (int m = 0; m < count; m++)
		{
			for (int n = 0; n < num; n++)
			{
				binaryWriter.Write(objScale[m][n].x);
				binaryWriter.Write(objScale[m][n].y);
				binaryWriter.Write(objScale[m][n].z);
			}
		}
		objScaleEncoded = StringUtil.EncodeTo64(memoryStream.GetBuffer());
		binaryWriter.Close();
		memoryStream.Close();
	}

	public void DecodeData()
	{
		objPos = new Dictionary<int, List<Vector3>>();
		byte[] array;
		MemoryStream memoryStream;
		BinaryReader binaryReader;
		int num;
		int num2;
		if (trapsPosEncoded != string.Empty)
		{
			array = StringUtil.DecodeFrom64ToByteArray(trapsPosEncoded);
			if (array != null)
			{
				memoryStream = new MemoryStream(array);
				binaryReader = new BinaryReader(memoryStream);
				num = binaryReader.ReadInt32();
				num2 = binaryReader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					objPos.Add(i, new List<Vector3>());
					for (int j = 0; j < num2; j++)
					{
						Vector3 item = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
						objPos[i].Add(item);
					}
				}
				binaryReader.Close();
				memoryStream.Close();
			}
		}
		objRot = new Dictionary<int, List<Quaternion>>();
		if (objRotEncoded != string.Empty)
		{
			array = StringUtil.DecodeFrom64ToByteArray(objRotEncoded);
			if (array != null)
			{
				memoryStream = new MemoryStream(array);
				binaryReader = new BinaryReader(memoryStream);
				num = binaryReader.ReadInt32();
				num2 = binaryReader.ReadInt32();
				for (int k = 0; k < num; k++)
				{
					objRot.Add(k, new List<Quaternion>());
					for (int l = 0; l < num2; l++)
					{
						Quaternion item2 = new Quaternion(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
						objRot[k].Add(item2);
					}
				}
				binaryReader.Close();
				memoryStream.Close();
			}
		}
		objScale = new Dictionary<int, List<Vector3>>();
		if (!(objScaleEncoded != string.Empty))
		{
			return;
		}
		array = StringUtil.DecodeFrom64ToByteArray(objScaleEncoded);
		if (array == null)
		{
			return;
		}
		memoryStream = new MemoryStream(array);
		binaryReader = new BinaryReader(memoryStream);
		num = binaryReader.ReadInt32();
		num2 = binaryReader.ReadInt32();
		for (int m = 0; m < num; m++)
		{
			objScale.Add(m, new List<Vector3>());
			for (int n = 0; n < num2; n++)
			{
				Vector3 item3 = new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
				objScale[m].Add(item3);
			}
		}
		binaryReader.Close();
		memoryStream.Close();
	}

	private void ShowArrayPos()
	{
		if (objPos != null)
		{
			int count = objPos.Count;
			int num = ((count > 0) ? objPos[0].Count : 0);
			Debug.Log(string.Format("setNumCount: {0} trapNumCount: {1}", count, num));
			for (int i = 0; i < objPos.Count; i++)
			{
				for (int j = 0; j < num; j++)
				{
					Debug.Log(string.Format("setNum: {0} pos: {1}", i, objPos[i][j]));
				}
			}
		}
		else
		{
			Debug.Log("trapsPos is null");
		}
	}

	public void RandomizeSet()
	{
		DecodeData();
		if (objPos == null)
		{
			return;
		}
		int count = objPos.Count;
		int key = ((AlwaysUseSet != -1) ? AlwaysUseSet : Random.Range(0, count));
		if (chunkObjects != null)
		{
			int count2 = chunkObjects.Count;
			for (int i = 0; i < count2; i++)
			{
				chunkObjects[i].transform.localPosition = objPos[key][i];
				chunkObjects[i].transform.localRotation = objRot[key][i];
				chunkObjects[i].transform.localScale = objScale[key][i];
			}
		}
	}
}
