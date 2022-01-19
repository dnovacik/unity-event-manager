# unity-event-manager
Simple event manager for unity projects.

## Usage

```cs
// my custom event
public struct MyEvent
{
    public string MyProp { get;set; }
    
    public MyEvent(string prop)
    {
        this.MyProp = prop;
    }
}

// listener class example, you can listen to several events, just add IEventListener<T> for each event struct
public class MyManager : MonoBehaviour, IEventListener<MyEvent>
{
    // subscribe to the event inside OnEnable method and pass a listener
    public void OnEnable()
    {
        this.StartListening<MyEvent>(OnMyEventTriggered);
    }
    
    // stop listening to the event inside OnDisable
    public void OnDisable()
    {
        this.StopListening<MyEvent>(OnMyEventTriggered);
    }
    
    private void OnMyEventTriggered(MyEvent eventType)
    {
        // omitted code for brewity
    }
}

// triggering the event
public class MyExecuteClass : MonoBehaviour
{
    public void DoSomething()
    {
        // omitted code for brewity
        // execute the event
        EventManager.TriggerEvent<MyEvent>(new MyEvent("MyPropValue"));
    }
}

```
