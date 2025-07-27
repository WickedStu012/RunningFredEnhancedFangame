public struct iCadeState
{
	public bool JoystickUp;

	public bool JoystickRight;

	public bool JoystickDown;

	public bool JoystickLeft;

	public bool ButtonA;

	public bool ButtonB;

	public bool ButtonC;

	public bool ButtonD;

	public bool ButtonE;

	public bool ButtonF;

	public bool ButtonG;

	public bool ButtonH;

	public override string ToString()
	{
		return string.Format("up: {0}, right: {1}, down: {2}, left: {3}, a: {4}, b: {5}, c: {6}, d: {7}, e: {8}, f: {9}, g: {10}, h: {11}", JoystickUp, JoystickRight, JoystickDown, JoystickLeft, ButtonA, ButtonB, ButtonC, ButtonD, ButtonE, ButtonF, ButtonG, ButtonH);
	}
}
