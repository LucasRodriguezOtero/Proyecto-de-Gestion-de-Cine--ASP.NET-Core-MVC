
namespace TP_FINAL_GRUPO_C.Herramientas.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        (bool Verified, bool NeedsUpgrade) Check (string hash, string password);
    }
}
