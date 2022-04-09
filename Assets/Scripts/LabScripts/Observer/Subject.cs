using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subject {

    readonly object sender; 
    readonly List<IObserver> observers = new List<IObserver>();

    public Subject(object sender) {
        this.sender = sender;
    }

    // * AddObserver - Used by observers to register to this subject
    public void AddObserver(IObserver obs)
    {
        observers.Add(obs);
    }

    // * RemoveObserver - Used by observers to deregister from this subject
    public void RemoveObserver(IObserver obs)
    {
        observers.Remove(obs);
    }

    // * Notify - Used by owner of this subject to notify observers, that something has happened.   
    public void Notify()
    {
        foreach (IObserver o in observers)
        {
            o.SubjectUpdate(sender);
        }
            
    }
}