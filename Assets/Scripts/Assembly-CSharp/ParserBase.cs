using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class ParserBase
{
	private enum State
	{
		None = 0,
		Group = 1,
		Assign = 2,
		Error = 3
	}

	protected Tokenizer tokenizer;

	private string text;

	private State state;

	private Stack<string> groupStack = new Stack<string>();

	private string groupName = string.Empty;

	private string varName = string.Empty;

	private bool ready;

	public bool IsReady
	{
		get
		{
			return ready;
		}
	}

	public ParserBase()
	{
	}

	public void Reset()
	{
		text = string.Empty;
		state = State.None;
		groupStack = new Stack<string>();
		groupName = string.Empty;
		varName = string.Empty;
		ready = false;
	}

	public bool LoadFile(string filename)
	{
		TextAsset textAsset = Resources.Load(filename, typeof(TextAsset)) as TextAsset;
		if (textAsset != null)
		{
			text = textAsset.text;
			return true;
		}
		OnError("Error opening file: " + filename);
		return false;
	}

	public bool ParseFile(string filename)
	{
		if (!LoadFile(filename))
		{
			return false;
		}
		return Parse();
	}

	public bool Parse()
	{
		return Parse(text);
	}

	public bool Parse(string text)
	{
		tokenizer = new Tokenizer(text);
		Tokenizer.Token nextToken;
		do
		{
			nextToken = tokenizer.GetNextToken();
			switch (nextToken.Type)
			{
			case Tokenizer.TokenType.Ident:
				Ident(nextToken.Value);
				break;
			case Tokenizer.TokenType.Assign:
				Assign();
				break;
			case Tokenizer.TokenType.OpenBracket:
				OpenGroup(groupName);
				break;
			case Tokenizer.TokenType.CloseBracket:
				CloseGroup();
				break;
			}
		}
		while (nextToken.Type != Tokenizer.TokenType.Eof && nextToken.Type != Tokenizer.TokenType.Invalid && state != State.Error);
		if (nextToken.Type == Tokenizer.TokenType.Invalid)
		{
			OnError("Tokenizer error: invalid token!");
		}
		if (nextToken.Type == Tokenizer.TokenType.Eof && state != State.Error)
		{
			ready = true;
			return true;
		}
		return false;
	}

	private void OpenGroup(string groupName)
	{
		groupStack.Push(groupName);
		OnOpenGroup(groupName);
		if (tokenizer.PeekNextToken().Type != Tokenizer.TokenType.Ident)
		{
			state = State.Error;
			OnError("Parser error: Identifier expected!");
		}
	}

	private void CloseGroup()
	{
		groupName = groupStack.Pop();
		OnCloseGroup(groupName);
		Tokenizer.Token token = tokenizer.PeekNextToken();
		if (token.Type == Tokenizer.TokenType.CloseBracket && groupStack.Count == 0)
		{
			state = State.Error;
			OnError("Parser error: Closing a non existent block!");
		}
		else if (token.Type != Tokenizer.TokenType.Ident && token.Type != Tokenizer.TokenType.CloseBracket && token.Type != Tokenizer.TokenType.Eof)
		{
			state = State.Error;
			OnError("Parser error: Identifier expected!");
		}
	}

	private void Ident(string ident)
	{
		Tokenizer.Token token = tokenizer.PeekNextToken();
		if (token.Type == Tokenizer.TokenType.OpenBracket)
		{
			groupName = ident;
			state = State.Group;
		}
		else if (token.Type == Tokenizer.TokenType.Assign)
		{
			varName = ident;
			state = State.Assign;
		}
		else
		{
			state = State.Error;
			OnError("Parser error: Open bracket or assignment expected!");
		}
	}

	private void Assign()
	{
		Tokenizer.Token token = tokenizer.PeekNextToken();
		if (token.Type == Tokenizer.TokenType.String || token.Type == Tokenizer.TokenType.Number || token.Type == Tokenizer.TokenType.Boolean)
		{
			if (token.Type == Tokenizer.TokenType.Boolean)
			{
				token = tokenizer.GetNextToken();
				OnAssign(varName, bool.Parse(token.Value));
			}
			else if (token.Type == Tokenizer.TokenType.String)
			{
				token = tokenizer.GetNextToken();
				OnAssign(varName, token.Value);
			}
			else if (token.Type == Tokenizer.TokenType.Number)
			{
				token = tokenizer.GetNextToken();
				OnAssign(varName, float.Parse(token.Value));
			}
			if (tokenizer.GetNextToken().Type != Tokenizer.TokenType.Semicolon)
			{
				state = State.Error;
				OnError("Parser error: Semicolon expected!");
			}
		}
		else
		{
			state = State.Error;
			OnError("Parser error: a number or a string was expected.");
		}
	}

	public void Write(string filename, Dictionary<string, object> data)
	{
		string empty = string.Empty;
		Write(filename, data, ref empty);
		FileStream fileStream = new FileStream(filename, FileMode.Create);
		StreamWriter streamWriter = new StreamWriter(fileStream);
		try
		{
			streamWriter.WriteLine(empty);
		}
		catch
		{
		}
		finally
		{
			streamWriter.Close();
			fileStream.Close();
		}
	}

	private void Write(string filename, Dictionary<string, object> data, ref string text, int indent = 0)
	{
		foreach (string key in data.Keys)
		{
			text = Indent(text, indent);
			text += key;
			if (data[key] is Dictionary<string, object>)
			{
				text += "\n";
				text = Indent(text, indent) + "{\n";
				Write(filename, (Dictionary<string, object>)data[key], ref text, indent + 1);
				text = Indent(text, indent) + "}";
			}
			else if (data[key] is string)
			{
				text = text + " = \"" + (string)data[key] + "\";";
			}
			else if (data[key] is float || data[key] is int || data[key] is bool)
			{
				text = text + " = " + data[key].ToString() + ";";
			}
			text += "\n";
		}
	}

	private string Indent(string text, int indent)
	{
		for (int i = 0; i < indent; i++)
		{
			text += "\t";
		}
		return text;
	}

	protected abstract void OnOpenGroup(string group);

	protected abstract void OnCloseGroup(string group);

	protected abstract void OnAssign(string varName, bool value);

	protected abstract void OnAssign(string varName, string value);

	protected abstract void OnAssign(string varName, float value);

	protected abstract void OnError(string error);
}
