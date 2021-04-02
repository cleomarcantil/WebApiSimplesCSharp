using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSimplesCSharp.Data.Entities
{
	public class Role : Entity<int>
	{
		protected Role() : base(default) { }

		public Role(int id, string nome, string? descricao)
			: base(id)
		{
			this.Nome = nome;
			this.Descricao = descricao;
		}

		public string Nome { get; set; }

		public string? Descricao { get; set; }

		public ICollection<RolePermissao> _permissoes = new HashSet<RolePermissao>();
		public virtual IEnumerable<RolePermissao> Permissoes => _permissoes.AsEnumerable();

		public void AddPermissao(string nome)
		{
			if (_permissoes.Any(p => p.Nome == nome)) {
				throw new Exception($"Permissão já adicionada '{nome}'!");
			}

			_permissoes.Add(new RolePermissao(Id, nome));
		}

		public void RemovePermissao(string nome)
		{
			var prm = _permissoes.SingleOrDefault(p => p.Nome == nome);

			if (prm is not null) {
				_permissoes.Remove(prm);
			}
		}

		public virtual ICollection<Usuario> Usuarios { get; private set; } = new HashSet<Usuario>();


		public static Role Create(string nome, string? descricao)
			=> new Role(default, nome, descricao);

	}
}
