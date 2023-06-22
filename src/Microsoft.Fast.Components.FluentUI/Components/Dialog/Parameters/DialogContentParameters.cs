namespace Microsoft.Fast.Components.FluentUI
{
    internal class DialogContentParameters<T> : ComponentParameters, IDialogContentParameters<T>
    {
        public T Data { get; set; } = default!;
    }

}
