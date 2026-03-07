using UnityEngine;

public class MoveCommand: ICommand
{
    private Vector3 destination;
    public MoveCommand(Vector3 destination)
    {
        this.destination = destination;
    }
    public void Execute(RTSUnit unit)
    {
        NavigationModule navModule = unit.moduleContainer.Get<NavigationModule>();
        if(navModule != null){
            navModule.MoveTo(destination);
        }
    }
}