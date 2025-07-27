using System;
using System.Xml.Serialization;
using UnityEngine;

namespace BmFont
{
	[Serializable]
	public class FontInfo
	{
		private Rect _Padding;

		private Vector2 _Spacing;

		[XmlAttribute("face")]
		public string Face { get; set; }

		[XmlAttribute("size")]
		public int Size { get; set; }

		[XmlAttribute("bold")]
		public int Bold { get; set; }

		[XmlAttribute("italic")]
		public int Italic { get; set; }

		[XmlAttribute("charset")]
		public string CharSet { get; set; }

		[XmlAttribute("unicode")]
		public int Unicode { get; set; }

		[XmlAttribute("stretchH")]
		public int StretchHeight { get; set; }

		[XmlAttribute("smooth")]
		public int Smooth { get; set; }

		[XmlAttribute("aa")]
		public int SuperSampling { get; set; }

		[XmlAttribute("padding")]
		public string Padding
		{
			get
			{
				return _Padding.x + "," + _Padding.y + "," + _Padding.width + "," + _Padding.height;
			}
			set
			{
				string[] array = value.Split(',');
				_Padding = new Rect(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]), Convert.ToInt32(array[2]), Convert.ToInt32(array[3]));
			}
		}

		[XmlAttribute("spacing")]
		public string Spacing
		{
			get
			{
				return _Spacing.x + "," + _Spacing.y;
			}
			set
			{
				string[] array = value.Split(',');
				_Spacing = new Vector2(Convert.ToInt32(array[0]), Convert.ToInt32(array[1]));
			}
		}

		[XmlAttribute("outline")]
		public int OutLine { get; set; }
	}
}
