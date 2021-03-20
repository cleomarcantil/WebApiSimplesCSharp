using System.Collections.Generic;
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
		public string HashSenha { get; set; }

		public static Usuario Create(string nome, string login, string hashSenha)
			=> new Usuario(default, nome, login, hashSenha);

	}
}
