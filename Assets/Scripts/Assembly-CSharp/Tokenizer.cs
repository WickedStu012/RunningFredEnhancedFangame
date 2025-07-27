public class Tokenizer
{
	public enum TokenType
	{
		Invalid = 0,
		OpenBracket = 1,
		CloseBracket = 2,
		Ident = 3,
		Assign = 4,
		Semicolon = 5,
		String = 6,
		Number = 7,
		Boolean = 8,
		Eof = 9
	}

	public struct Token
	{
		public TokenType Type;

		public string Value;
	}

	private string text;

	private int startId;

	private int endId;

	public Tokenizer(string text)
	{
		this.text = text.Trim();
		startId = (endId = 0);
	}

	public void Reset()
	{
		startId = (endId = 0);
	}

	public Token GetNextToken()
	{
		Token result = new Token
		{
			Type = TokenType.Eof
		};
		if (MoveToNext(ref startId, ref endId))
		{
			return GetToken(GetLexem(startId, endId), ref startId, ref endId);
		}
		return result;
	}

	public Token PeekNextToken()
	{
		int sid = startId;
		int eid = endId;
		if (MoveToNext(ref sid, ref eid))
		{
			return GetToken(GetLexem(sid, eid), ref sid, ref eid);
		}
		Token result = default(Token);
		result.Value = string.Empty;
		result.Type = TokenType.Eof;
		return result;
	}

	private bool MoveToNext(ref int startId, ref int endId)
	{
		if (startId != 0 || endId != 0)
		{
			startId = endId + 1;
		}
		if (startId >= text.Length)
		{
			return false;
		}
		while (startId < text.Length - 1 && char.IsWhiteSpace(text[startId]))
		{
			startId++;
		}
		endId = startId;
		if (char.IsSeparator(text[endId]) || text[endId] == '=' || text[endId] == '"' || text[endId] == '{' || text[endId] == '}' || text[endId] == ';' || text[endId] == '"')
		{
			return true;
		}
		while (endId < text.Length - 1 && !char.IsSeparator(text[endId + 1]) && text[endId + 1] != '=' && text[endId + 1] != '"' && text[endId + 1] != '{' && text[endId + 1] != '}' && text[endId + 1] != ';')
		{
			endId++;
		}
		return true;
	}

	private Token GetToken(string lexem, ref int sid, ref int eid)
	{
		Token result = default(Token);
		result.Type = TokenType.Eof;
		result.Value = lexem.ToLower();
		switch (lexem)
		{
		case "\"":
			if (GetString(ref sid, ref eid, out result.Value))
			{
				result.Type = TokenType.String;
			}
			else
			{
				result.Type = TokenType.Invalid;
			}
			break;
		case "=":
			result.Type = TokenType.Assign;
			break;
		case ";":
			result.Type = TokenType.Semicolon;
			break;
		case "}":
			result.Type = TokenType.CloseBracket;
			break;
		case "{":
			result.Type = TokenType.OpenBracket;
			break;
		default:
		{
			bool result2;
			float result3;
			if (bool.TryParse(lexem, out result2))
			{
				result.Type = TokenType.Boolean;
			}
			else if (float.TryParse(lexem, out result3))
			{
				result.Type = TokenType.Number;
			}
			else
			{
				result.Type = TokenType.Ident;
			}
			break;
		}
		}
		return result;
	}

	private string GetLexem(int startId, int endId)
	{
		return text.Substring(startId, endId - startId + 1).Trim();
	}

	private bool GetString(ref int startId, ref int endId, out string lexem)
	{
		lexem = string.Empty;
		if (text[startId] == '"')
		{
			startId++;
		}
		if (startId >= text.Length)
		{
			return false;
		}
		endId = startId + 1;
		while (endId < text.Length - 1 && text[endId] != '"')
		{
			endId++;
		}
		if (text[endId] == '"')
		{
			lexem = text.Substring(startId, endId - startId);
			return true;
		}
		return false;
	}
}
