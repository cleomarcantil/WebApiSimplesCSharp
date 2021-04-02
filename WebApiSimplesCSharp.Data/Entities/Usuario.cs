using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Data.Entities
{
	public class Usuario : Entity<int>
	{
		protected Usuario() : base(default) { }

		public Usuario(int id, string nome, string login, string hashSenha)
			: base(id)
		{
			this.Nome = nome;
			this.Login = login;
			this.HashSenha = hashSenha;
		}

		public string Nome { get; set; }
		public string Login { get; private set; }
		public string HashSenha { get; private set; }


		public void DefinirSenha(string senha)
			=> HashSenha = GerarHashSenha(senha);

		public bool CheckSenha(string senha)
			=> (HashSenha == GerarHashSenha(senha));


		public virtual ICollection<Role> Roles { get; private set; } = new HashSet<Role>();

		public static Usuario Create(string nome, string login, string senha)
			=> new Usuario(default, nome, login, GerarHashSenha(senha));


		public static string GerarHashSenha(string senha)
		{
			using var crypt = new MD5CryptoServiceProvider();

			var bytesHashed = crypt.ComputeHash(System.Text.Encoding.UTF8.GetBytes(senha));

			return Convert.ToBase64String(bytesHashed);
		}
	}
}
