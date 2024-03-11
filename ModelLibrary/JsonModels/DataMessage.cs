using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ModelLibrary.JsonModels
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MessageType
    {
        [EnumMember(Value = "ChatMessage")]
        ChatMessage,
        [EnumMember(Value = "RegisterMessage")]
        RegisterMessage,
        [EnumMember(Value = "LoginMessage")]
        LoginMessage,
        [EnumMember(Value = "LoginSuccess")]
        LoginSuccess,
        [EnumMember(Value = "LoginFailure")]
        LoginFailure,
        [EnumMember(Value = "RegisterSuccess")]
        RegisterSuccess,
        [EnumMember(Value = "RegisterFailure")]
        RegisterFailure,
        [EnumMember(Value = "ContactsRequest")]
        ContactsRequest,
        [EnumMember(Value = "GetContactMessages")]
        GetContactMessages,
        [EnumMember(Value = "FindContact")]
        FindContact,
        [EnumMember(Value = "ContactNotFound")]
        ContactNotFound,
        [EnumMember(Value = "ContactFound")]
        ContactFound,
        [EnumMember(Value = "GetChat")]
        GetChat,
        [EnumMember(Value = "ChangeProfile")]
        ChangeProfile,
        [EnumMember(Value = "ChangePassword")]
        ChangePassword,
        [EnumMember(Value = "PasswordChangeSuccess")]
        PasswordChangeSuccess,
        [EnumMember(Value = "PasswordChangeFailure")]
        PasswordChangeFailure,
        [EnumMember(Value = "ChangePasswordWithEmail")]
        ChangePasswordWithEmail,
        [EnumMember(Value = "EmailPasswordChangeFailure")]
        EmailPasswordChangeSuccess,
        [EnumMember(Value = "CodeRequest")]
        CodeRequest,
        [EnumMember(Value = "CodeRequestFailure")]
        CodeRequestFailure,
        [EnumMember(Value = "CodeRequestSuccess")]
        CodeRequestSuccess,
        [EnumMember(Value = "ResetPassword")]
        ResetPassword,
        [EnumMember(Value = "IncorrectEmailChange")]
        IncorrectEmailChange
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RegisterResults
    {
        [EnumMember(Value = "LoginExists")]
        LoginExists
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CodeResults
    {
        [EnumMember(Value = "WrongLogin")]
        WrongLogin,
        [EnumMember(Value = "LoginEmailIsWrong")]
        LoginEmailIsWrong,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LoginResults
    {
        [EnumMember(Value = "IncorrectPassword")]
        IncorrectPassword,
        [EnumMember(Value = "NoLogin")]
        NoLogin
    }
    public class DataMessage
    {
        public string Data { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MessageType Type { get; set; }

    }
}
