﻿using Contract;

namespace LowerCaseRemoveSpacesRule
{
	public class LowerCaseRemoveSpaces : IRule
	{
		public string Name => "LowerCaseRemoveSpaces";

		public string Config => "";

		public string Parse { get; set; }

		public string Rename(string origin)
		{
			origin = origin.ToLower();
			origin = origin.Replace(" ", "");
			return origin;
		}
	}
}