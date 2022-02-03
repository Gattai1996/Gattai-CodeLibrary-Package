namespace Gattai.Runtime.Debug
{
    public class DebugCommandBase
    {
        private string _commandId;
        private string _commandDescription;
        private string _commandFormat;

        public string CommandId => _commandId;
        public string CommandDesciption => _commandDescription;
        public string CommandFormat => _commandFormat;

        public DebugCommandBase(string id, string description, string format)
        {
            _commandId = id;
            _commandDescription = description;
            _commandFormat = format;
        }
    }
}
