namespace WebApiSimplesCSharp.Data.Entities
{
	public class RolePermissao
	{
		protected RolePermissao() { }

		public RolePermissao(int roleId, string nome)
			=> (RoleId, Nome) = (roleId, nome);

		public int RoleId { get; private set; }

		public string Nome { get; private set; }
	}
}
