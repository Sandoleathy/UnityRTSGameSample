
public class ChangeFireStateCommand: ICommand
{
    private bool isCeaseFire;
    public ChangeFireStateCommand(bool isCeaseFire)
    {
        this.isCeaseFire = isCeaseFire;
    }
    public void Execute(RTSUnit unit)
    {
        MilitaryModule militaryModule = unit.moduleContainer.Get<MilitaryModule>();
        if(militaryModule != null){
            militaryModule.SetIsOpenFire(isCeaseFire);
        }
    }
}