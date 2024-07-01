using Flunt.Notifications;
using Flunt.Validations;

public class UsuarioLoginDTO : Notifiable<Notification>
{
    public string Login { get; set; }
    public string Password { get; set; }

    public void Validate()
    {
        AddNotifications(new Contract<Notification>()
            .Requires()
            .IsNotNullOrEmpty(Login, nameof(Login), "Login é obrigatório.")
            .IsNotNullOrEmpty(Password, nameof(Password), "Senha é obrigatória.")
            .IsGreaterThan(Password?.Length ?? 0, 5, nameof(Password), "A senha deve ter no mínimo 6 caracteres.")
        );
    }
}
