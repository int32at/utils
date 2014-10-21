namespace int32.Utils.Core.Generic.ViewEngine.Contracts
{
    public interface IViewEngine
    {
        string Render(string content, dynamic model);
    }
}
