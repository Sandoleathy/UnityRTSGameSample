// 所有需要顺序执行的命令都需要实现这个接口
public interface IOrderedCommand
{
    // 获取命令的状态
    CommandStatus GetStatus();
    
}