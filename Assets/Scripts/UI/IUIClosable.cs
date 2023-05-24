using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUIClosable
{
	void AddCloseListener();
	void RemoveCloseListener();

	bool Closable();
	void Close();
}
