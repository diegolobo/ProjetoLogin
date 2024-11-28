using System.Text;
using Konscious.Security.Cryptography;

namespace ProjetoLogin.Utils;

public static class Cryptography
{
	private const int ByteArraySize = 128;
	private static Argon2id GetConfig(string password, string salt)
	{
		return new Argon2id(Encoding.UTF8.GetBytes(password))
		{
			Salt = Encoding.UTF8.GetBytes(salt),
			KnownSecret = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable(AppConstants.AppSecretKey)!),
			DegreeOfParallelism = 20,
			MemorySize = 65536,
			Iterations = 20,
		};
	}

	public static string GetHashPassword(string password, string salt)
	{
		var hasher = GetConfig(password, salt);

		var hashBytes = hasher.GetBytes(ByteArraySize);

		return Convert.ToBase64String(hashBytes);
	}

	public static bool CheckHashPassword(string hash, string password, string salt)
	{
		try
		{
			var hashBytes = Convert.FromBase64String(hash);

			var verifier = GetConfig(password, salt);

			return verifier.GetBytes(ByteArraySize).AsSpan().SequenceEqual(hashBytes);
		}
		catch
		{
			return false;
		}
	}
}
