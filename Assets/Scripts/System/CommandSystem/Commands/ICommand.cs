// 所有指令都需实现该接口
// 指令默认为可以并发执行的，因此需要按顺序执行的指令需另外实现 IOrderedCommand 接口
public interface ICommand
{
        void Execute(RTSUnit unit);
}