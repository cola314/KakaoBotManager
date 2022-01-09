namespace KakaoBotManager.Config.Exceptions;

public class BadEnvironmentVariableException : Exception
{
    public BadEnvironmentVariableException(string message) : base(message) { }

    public BadEnvironmentVariableException(string key, string value) 
        : base($"{key}에 대한 환경변수가 올바르지 않습니다\n주어진 값 : {value}") { }
}
