namespace CatalogoJogos.Settings;

// Classe que representa a seção "MongoDbSettings" do appsettings.json (Options Pattern)
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string JogosCollectionName { get; set; } = "jogos";
}
