

namespace MohamedSprint1V2.DLL.ModelVM.ResponseResult
{
    public record Response<T>(T result, string? Message, bool Successornot);
    
}
