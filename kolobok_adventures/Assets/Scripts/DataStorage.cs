using UnityEngine;
using System.Collections.Generic;

public class DataStorage : MonoBehaviour
{
	Dictionary<string, System.Object> storage = new Dictionary<string, System.Object> ();

	/// <summary>
	/// Adds the entry.
	/// </summary>
	/// <returns><c>true</c>, if entry was added, <c>false</c> otherwise.</returns>
	/// <param name="key">Key.</param>
	/// <param name="value">Value.</param>
	public bool AddEntry(string key, System.Object value) {
		if (storage.ContainsKey (key)) return false;

		storage.Add (key, value);
		return true;
	}

	/// <summary>
	/// Gets the entry.
	/// </summary>
	/// <returns>The entry, or null if nothing found.</returns>
	/// <param name="key">Key.</param>
	public System.Object GetEntry(string key) {
		if (!storage.ContainsKey (key)) return null;
		return storage[key];
	}
}

