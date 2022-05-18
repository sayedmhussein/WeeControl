namespace WeeControl.SharedKernel.Essential;

public static class Api
{
    public static class Essential
    {
        public static class User
        {
            public const string Login = nameof(Api) + "/" + nameof(Essential) + "/" + nameof(User) + "/Login";
            public const string Logout = nameof(Essential) + "/" + nameof(User) + "/Login";
            public const string Bla = nameof(Essential) + "/" + nameof(User) + "/Login{id}";
        }
    }
}