using MediatR;

namespace BuildingBlocks.CQRS;

/// <summary>
/// Trình xử lý và không trả về bất kỳ phản hồi nào
/// </summary>
/// <typeparam name="TCommand"></typeparam>
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand<Unit>
{

}

/// <summary>
/// Trình xử lý trả về phản hồi chung
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public interface ICommandHandler<in TCommand, TResponse> 
    : IRequestHandler<TCommand, TResponse> 
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}

