using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable()]
public class RepatedCoordsException : Exception {
	public RepatedCoordsException() : base() { }
	public RepatedCoordsException(string message) : base(message) { }
	public RepatedCoordsException(string message, System.Exception inner) : base(message, inner) { }

//	protected RepatedCoordsException(SerializationInfo info, StreamingContext context){
//
//	}
}
